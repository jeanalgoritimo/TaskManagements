using TaskManager.Domain.Entities;
using UserproTasks.Application.DTOs;

namespace UserProTasks.Application.Interfaces
{
    public interface IProjetoRepository
    {
        Task<Projeto> GetByIdAsync(Guid id);
        Task<Projeto> GetByIdWithTasksAsync(Guid id); // Novo: Incluir tarefas para verificação de remoção
        Task<IEnumerable<Projeto>> GetAllByUserIdAsync(Guid userId); // Novo: Listar projetos do usuário
        Task<IEnumerable<Projeto>> GetAllAsync();
        Task AddAsync(Projeto projeto);
        Task UpdateAsync(Projeto projeto);
        Task DeleteAsync(Projeto projeto);
        Task SaveChangesAsync();
        Task<IEnumerable<UsuarioDto>> GetUsuariosDosProjetosAsync();
    }
}