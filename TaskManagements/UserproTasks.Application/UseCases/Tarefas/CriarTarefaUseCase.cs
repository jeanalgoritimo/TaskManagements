using UserProTasks.Application.Interfaces;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Entities;

namespace UserProTasks.Application.UseCases.Tarefas
{
    public class CriarTarefaUseCase
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly ITarefaRepository _tarefaRepository;

        public CriarTarefaUseCase(IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository)
        {
            _projetoRepository = projetoRepository;
            _tarefaRepository = tarefaRepository;
        }

        public async Task<(Tarefa Tarefa, string ErrorMessage)> ExecutarAsync(
            string titulo,
            string descricao,
            DateTime dataVencimento,
            StatusTarefa statusTarefa,
            PrioridadeTarefa prioridade,
            Guid projetoId)
        {
            var projeto = await _projetoRepository.GetByIdWithTasksAsync(projetoId); // Carregar tarefas para verificar limite
            if (projeto == null)
            {
                return (null, "Projeto não encontrado.");
            }

            if (projeto.Tarefas.Count >= Projeto.LimiteMaximoTarefas) // Aplica a regra de negócio
            {
                return (null, $"Limite máximo de {Projeto.LimiteMaximoTarefas} tarefas por projeto atingido.");
            }

            var novaTarefa = new Tarefa(titulo, descricao, dataVencimento, statusTarefa, prioridade, projetoId, projeto.UsuarioId, projeto.NomeUsuario);
            await _tarefaRepository.AddAsync(novaTarefa);
            await _tarefaRepository.SaveChangesAsync();

            return (novaTarefa, null);
        }
    }
}