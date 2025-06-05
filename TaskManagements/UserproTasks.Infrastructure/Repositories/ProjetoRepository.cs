using Microsoft.EntityFrameworkCore;
using System;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
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

        public async Task AddAsync(Projeto projeto)
        {
            await _context.Projetos.AddAsync(projeto);
        }

        public async Task<Projeto> GetByIdAsync(Guid id)
        {
            return await _context.Projetos.FindAsync(id);
        }

        public async Task<List<Projeto>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Projetos
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task RemoveAsync(Projeto projeto)
        {
            _context.Projetos.Remove(projeto);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
