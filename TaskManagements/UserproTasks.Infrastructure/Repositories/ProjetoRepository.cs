using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using UserProTasks.Application.Interfaces;
using UserProTasks.Infrastructure.Data;
using UserproTasks.Application.DTOs; 

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
                                 .Include(p => p.Tarefas)
                                 .FirstOrDefaultAsync(p => p.ProjetoId == id);
        }

        public async Task<IEnumerable<Projeto>> GetAllByUserIdAsync(Guid userId)
        { 
            return await _context.Projetos
                                 .Where(p => p.UsuarioId == userId)
                                 .Include(p => p.Tarefas)
                                 .ToListAsync();
        }
         
        public async Task<IEnumerable<Projeto>> GetAllAsync()
        {
            return await _context.Projetos
                                 .Include(p => p.Tarefas)  
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

        public async Task<IEnumerable<UsuarioDto>> GetUsuariosDosProjetosAsync()
        {
            return await _context.Projetos
                                 .GroupBy(p => new { p.UsuarioId, p.NomeUsuario })
                                 .Select(g => new UsuarioDto
                                 {
                                     UsuarioId = g.Key.UsuarioId,
                                     NomeUsuario = g.Key.NomeUsuario
                                 })
                                 .ToListAsync();
        }
    }
}