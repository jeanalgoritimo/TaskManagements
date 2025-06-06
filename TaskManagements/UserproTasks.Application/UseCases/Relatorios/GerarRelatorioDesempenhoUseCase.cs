using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Relatorios
{
    public class GerarRelatorioDesempenhoUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IProjetoRepository _projetoRepository; // Para obter nomes de usuários, se necessário

        public GerarRelatorioDesempenhoUseCase(ITarefaRepository tarefaRepository, IProjetoRepository projetoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
        }

        public async Task<RelatorioDesempenhoDto> ExecutarAsync(int diasRetroativos, string usuarioGerador)
        {
            var dataInicio = DateTime.UtcNow.AddDays(-diasRetroativos);

            // Uma abordagem simples para obter IDs de usuários de projetos existentes
            // Em um sistema real, teríamos um IUserRepository
            var todosProjetos = await _projetoRepository.GetAllByUserIdAsync(Guid.Empty); // Busca todos, pode ser melhorado
            var usuariosDistinct = todosProjetos
                                    .Select(p => new { p.UsuarioId, p.NomeUsuario })
                                    .Distinct()
                                    .ToList();

            var desempenhoPorUsuario = new List<DesempenhoUsuarioDto>();

            foreach (var user in usuariosDistinct)
            {
                var tarefasConcluidas = await _tarefaRepository.GetConcluidasPorUsuarioDesdeAsync(user.UsuarioId, dataInicio);
                var countConcluidas = tarefasConcluidas.Count();
                var mediaDiaria = countConcluidas > 0 ? (double)countConcluidas / diasRetroativos : 0.0;

                desempenhoPorUsuario.Add(new DesempenhoUsuarioDto
                {
                    UsuarioId = user.UsuarioId,
                    NomeUsuario = user.NomeUsuario,
                    TarefasConcluidasNoPeriodo = countConcluidas,
                    MediaTarefasConcluidasDiarias = mediaDiaria
                });
            }

            return new RelatorioDesempenhoDto
            {
                DesempenhoPorUsuario = desempenhoPorUsuario,
                DataGeracao = DateTime.UtcNow,
                PeriodoRelatorio = $"Últimos {diasRetroativos} dias."
            };
        }
    }
}