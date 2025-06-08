// UserProTasks.Application.UseCases.Relatorios.GerarRelatorioDesempenhoUseCase.cs
using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProTasks.Application.UseCases.Relatorios
{
    // Seus DTOs (DesempenhoUsuarioDto, RelatorioDesempenhoDto) permanecem os mesmos

    public class GerarRelatorioDesempenhoUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IProjetoRepository _projetoRepository;

        // Construtor sem IAuthorizationService
        public GerarRelatorioDesempenhoUseCase(
            ITarefaRepository tarefaRepository,
            IProjetoRepository projetoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
        }

        // Assinatura do método: Agora recebe apenas diasRetroativos (se o usuário logado não for relevante para a lógica interna do relatório)
        // Se o relatório é "para todos os usuários", não precisamos do 'usuarioGeradorId' aqui.
        public async Task<RelatorioDesempenhoDto> ExecutarAsync(int diasRetroativos ,string usuario) // Assinatura simplificada
        {
            // A verificação de autorização foi removida daqui, pois será tratada externamente.

            var dataInicio = DateTime.UtcNow.AddDays(-diasRetroativos);

            // Obter a lista de usuários para os quais o relatório será gerado.
            var todosProjetos = await _projetoRepository.GetAllAsync();
            var usuariosParaRelatorio = todosProjetos
                                        .Select(p => new { p.UsuarioId, p.NomeUsuario })
                                        .Distinct()
                                        .ToList();

            if (!usuariosParaRelatorio.Any())
            {
                return new RelatorioDesempenhoDto
                {
                    DataGeracao = DateTime.UtcNow,
                    PeriodoRelatorio = $"Últimos {diasRetroativos} dias.",
                    NumeroMedioTarefasConcluidasGeral = 0
                };
            }

            var desempenhoPorUsuario = new List<DesempenhoUsuarioDto>();
            double totalTarefasConcluidas = 0;

            foreach (var user in usuariosParaRelatorio)
            {
                var tarefasConcluidas = await _tarefaRepository.GetConcluidasPorUsuarioDesdeAsync(user.UsuarioId, dataInicio);
                var countConcluidas = tarefasConcluidas.Count();
                var mediaDiaria = diasRetroativos > 0 ? (double)countConcluidas / diasRetroativos : 0.0;

                desempenhoPorUsuario.Add(new DesempenhoUsuarioDto
                {
                    UsuarioId = user.UsuarioId,
                    NomeUsuario = user.NomeUsuario,
                    TarefasConcluidasNoPeriodo = countConcluidas,
                    MediaTarefasConcluidasDiarias = mediaDiaria
                });
                totalTarefasConcluidas += countConcluidas;
            }

            var numeroMedioTarefasConcluidasGeral = desempenhoPorUsuario.Any() ? totalTarefasConcluidas / desempenhoPorUsuario.Count : 0;

            return new RelatorioDesempenhoDto
            {
                DesempenhoPorUsuario = desempenhoPorUsuario,
                NumeroMedioTarefasConcluidasGeral = numeroMedioTarefasConcluidasGeral,
                DataGeracao = DateTime.UtcNow,
                PeriodoRelatorio = $"Últimos {diasRetroativos} dias."
            };
        }
    }
}