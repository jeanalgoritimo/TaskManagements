using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;
 
namespace UserProTasks.Application.UseCases.Tarefas
{
    public class AtualizarTarefaUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;

        public AtualizarTarefaUseCase(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> ExecutarAsync(
            Guid tarefaId,
            AtualizarTarefaDto dto,
            string usuarioAtualizacao) 
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(tarefaId);
            if (tarefa == null)
            {
                return (false, "Tarefa não encontrada.");
            }

            tarefa.AtualizarDetalhes(
                dto.Titulo ?? tarefa.Titulo, 
                dto.Descricao ?? tarefa.Descricao, 
                dto.DataVencimento ?? tarefa.DataVencimento, 
                usuarioAtualizacao);

            if (dto.Status.HasValue && dto.Status.Value != tarefa.Status)
            {
                tarefa.AtualizarStatus(dto.Status.Value, usuarioAtualizacao);
            }

            await _tarefaRepository.UpdateAsync(tarefa);
            await _tarefaRepository.SaveChangesAsync();

            return (true, null);
        }
    }
}