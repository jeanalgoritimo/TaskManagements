// UserProTasks.Application.UseCases.Relatorios.GerarRelatorioDesempenhoUseCase.cs
using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;

using TaskManager.Domain.Entities;

namespace UserProTasks.Application.UseCases.Relatorios
{
    public class GerarRelatorioDesempenhoUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IProjetoRepository _projetoRepository;

       
        public GerarRelatorioDesempenhoUseCase(
            ITarefaRepository tarefaRepository,
            IProjetoRepository projetoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
        }

        public async Task<RelatorioDesempenhoDto> ExecutarAsync(int diasRetroativos, string usuario)
        {
            var dataInicio = DateTime.UtcNow.AddDays(-diasRetroativos);
            var todosProjetos = await _projetoRepository.GetAllAsync();

            var usuariosParaRelatorio = ObterUsuariosDistintosDosProjetos(todosProjetos);

            if (!usuariosParaRelatorio.Any())
                return CriarRelatorioVazio(diasRetroativos);

            var desempenhoPorUsuario = new List<DesempenhoUsuarioDto>();
            double totalTarefasConcluidas = 0;

            foreach (var user in usuariosParaRelatorio)
            {
                var tarefasConcluidas = await _tarefaRepository.GetConcluidasPorUsuarioDesdeAsync(user.UsuarioId, dataInicio);
                var countConcluidas = tarefasConcluidas.Count();
                var mediaDiaria = CalcularMediaDiaria(countConcluidas, diasRetroativos);

                desempenhoPorUsuario.Add(new DesempenhoUsuarioDto
                {
                    UsuarioId = user.UsuarioId,
                    NomeUsuario = user.NomeUsuario,
                    TarefasConcluidasNoPeriodo = countConcluidas,
                    MediaTarefasConcluidasDiarias = mediaDiaria
                });

                totalTarefasConcluidas += countConcluidas;
            }

            var mediaGeral = desempenhoPorUsuario.Any()
                ? totalTarefasConcluidas / desempenhoPorUsuario.Count
                : 0;

            return new RelatorioDesempenhoDto
            {
                DesempenhoPorUsuario = desempenhoPorUsuario,
                NumeroMedioTarefasConcluidasGeral = mediaGeral,
                DataGeracao = DateTime.UtcNow,
                PeriodoRelatorio = $"Últimos {diasRetroativos} dias."
            };
        }
        private static List<(Guid UsuarioId, string NomeUsuario)> ObterUsuariosDistintosDosProjetos(IEnumerable<Projeto> projetos)
        {
            return projetos
                .Select(p => (p.UsuarioId, p.NomeUsuario))
                .Distinct()
                .ToList();
        }

        private static double CalcularMediaDiaria(int total, int dias)
        {
            return dias > 0 ? (double)total / dias : 0.0;
        }

        private static RelatorioDesempenhoDto CriarRelatorioVazio(int diasRetroativos)
        {
            return new RelatorioDesempenhoDto
            {
                DataGeracao = DateTime.UtcNow,
                PeriodoRelatorio = $"Últimos {diasRetroativos} dias.",
                NumeroMedioTarefasConcluidasGeral = 0,
                DesempenhoPorUsuario = new List<DesempenhoUsuarioDto>()
            };
        }
    }
}