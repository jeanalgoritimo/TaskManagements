using Microsoft.EntityFrameworkCore;
using UserProTasks.Application.Interfaces; 
using UserProTasks.Infrastructure.Data;

namespace UserProTasks.Infrastructure.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly AppDbContext _context;

        public ProjetoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Projeto> GetByIdAsync(Guid id)
        {
            return await _context.Projetos.FirstOrDefaultAsync(p => p.ProjetoId == id);
        }

        public async Task<Projeto> GetByIdWithTasksAsync(Guid id)
        {
            return await _context.Projetos
                                 .Include(p => p.Tarefas) // Incluir tarefas para a regra de negócio de remoção
                                 .FirstOrDefaultAsync(p => p.ProjetoId == id);
        }

        public async Task<IEnumerable<Projeto>> GetAllByUserIdAsync(Guid userId)
        {
            // Se userId for Guid.Empty, significa que queremos todos os projetos para o relatório (apenas exemplo)
            // Em um sistema real, você teria um User/Role de autenticação/autorização
            if (userId == Guid.Empty)
            {
                return await _context.Projetos
                                     .Include(p => p.Tarefas) // Inclui tarefas para calcular pendentes/concluídas no DTO
                                     .ToListAsync();
            }

            return await _context.Projetos
                                 .Where(p => p.UsuarioId == userId)
                                 .Include(p => p.Tarefas) // Inclui tarefas
                                 .ToListAsync();
        }

        public async Task AddAsync(Projeto projeto)
        {
            await _context.Projetos.AddAsync(projeto);
        }

        public Task UpdateAsync(Projeto projeto)
        {
            _context.Projetos.Update(projeto);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Projeto projeto)
        {
            _context.Projetos.Remove(projeto);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}