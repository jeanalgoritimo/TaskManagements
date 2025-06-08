using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace UserProTasks.Application.Interfaces
{
    public interface ITarefaRepository
    {
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Tarefa>> GetByProjetoIdAsync(Guid projetoId);
        Task<IEnumerable<Tarefa>> GetConcluidasPorUsuarioDesdeAsync(Guid usuarioId, DateTime dataInicio);
        Task<IEnumerable<Tarefa>> GetTarefasPendentesByProjetoIdAsync(Guid projetoId); // <--- Adicione esta linha
        Task AddAsync(Tarefa tarefa);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(Tarefa tarefa);
        Task SaveChangesAsync();
    }
}