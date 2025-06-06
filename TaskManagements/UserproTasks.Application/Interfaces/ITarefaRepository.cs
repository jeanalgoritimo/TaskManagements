using TaskManager.Domain.Entities; 

namespace UserProTasks.Application.Interfaces
{
    public interface ITarefaRepository
    {
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> GetByIdWithDetailsAsync(Guid id); // Novo: Incluir Comentarios e Historico
        Task<IEnumerable<Tarefa>> GetByProjetoIdAsync(Guid projetoId); // Novo: Listar tarefas de um projeto
        Task<IEnumerable<Tarefa>> GetConcluidasPorUsuarioDesdeAsync(Guid usuarioId, DateTime dataInicio); // Novo: Para relatórios
        Task AddAsync(Tarefa tarefa);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(Tarefa tarefa);
        Task SaveChangesAsync();
    }
}