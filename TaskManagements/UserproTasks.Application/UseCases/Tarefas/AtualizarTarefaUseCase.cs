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
            string usuarioAtualizacao) // Quem está realizando a atualização
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(tarefaId);
            if (tarefa == null)
            {
                return (false, "Tarefa não encontrada.");
            }

            // Aplica atualizações e registra histórico
            // Note que Prioridade NÃO é atualizada aqui devido à regra de negócio
            tarefa.AtualizarDetalhes(
                dto.Titulo ?? tarefa.Titulo, // Usa o novo título se fornecido, senão mantém o existente
                dto.Descricao ?? tarefa.Descricao, // Usa a nova descrição se fornecida, senão mantém
                dto.DataVencimento ?? tarefa.DataVencimento, // Usa a nova data se fornecida, senão mantém
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