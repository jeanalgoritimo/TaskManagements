using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Tarefas
{
    public class RemoverTarefaUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;

        public RemoverTarefaUseCase(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> ExecutarAsync(Guid tarefaId)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(tarefaId);
            if (tarefa == null)
            {
                return (false, "Tarefa não encontrada.");
            }

            await _tarefaRepository.DeleteAsync(tarefa);
            await _tarefaRepository.SaveChangesAsync();

            return (true, null);
        }
    }
}