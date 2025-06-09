using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Tarefas
{
    public class ListarTarefasProjetoUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IProjetoRepository _projetoRepository;

        public ListarTarefasProjetoUseCase(ITarefaRepository tarefaRepository, IProjetoRepository projetoRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
        }

        public async Task<(IEnumerable<TarefaDto> Tarefas, string ErrorMessage)> ExecutarAsync(Guid projetoId)
        {
            var projeto = await _projetoRepository.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                return (null, "Projeto não encontrado.");
            }

            var tarefas = await _tarefaRepository.GetByProjetoIdAsync(projetoId);

            var tarefasDto = tarefas.Select(t => new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                DataVencimento = t.DataVencimento,
                Status = t.Status,
                Prioridade = t.Prioridade,
                ProjetoId = t.ProjetoId
            }).ToList();

            return (tarefasDto, null);
        }
    }
}