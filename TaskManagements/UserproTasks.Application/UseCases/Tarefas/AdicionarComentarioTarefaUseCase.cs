using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Tarefas
{
    public class AdicionarComentarioTarefaUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;

        public AdicionarComentarioTarefaUseCase(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> ExecutarAsync(
            Guid tarefaId,
            string textoComentario,
            string usuarioComentario)
        {
            var tarefa = await _tarefaRepository.GetByIdWithDetailsAsync(tarefaId); 
            if (tarefa == null)
            {
                return (false, "Tarefa não encontrada.");
            }

            tarefa.AdicionarComentario(textoComentario, usuarioComentario);

            await _tarefaRepository.UpdateAsync(tarefa); 
            await _tarefaRepository.SaveChangesAsync();

            return (true, null);
        }
    }
}