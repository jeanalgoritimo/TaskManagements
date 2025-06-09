using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using UserProTasks.Application.Interfaces;
using UserProTasks.Infrastructure.Data;

namespace UserProTasks.Infrastructure.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> GetByIdAsync(Guid id)
        {
            return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tarefa> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Tarefas
                                 .Include(t => t.Comentarios)
                                 .Include(t => t.Historico)
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tarefa>> GetByProjetoIdAsync(Guid projetoId)
        {
            return await _context.Tarefas
                                 .Where(t => t.ProjetoId == projetoId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Tarefa>> GetConcluidasPorUsuarioDesdeAsync(Guid usuarioId, DateTime dataInicio)
        { 
            return await _context.Tarefas
                                 .Where(t => t.Status == StatusTarefa.Concluida &&
                                             t.Historico.Any(h => h.Descricao.Contains("Status alterado para 'Concluida'") && h.Data >= dataInicio) &&
                                             t.Projeto.UsuarioId == usuarioId)  
                                 .ToListAsync();
        }

     
        public async Task<IEnumerable<Tarefa>> GetTarefasPendentesByProjetoIdAsync(Guid projetoId)
        {
            return await _context.Tarefas
                                 .Where(t => t.ProjetoId == projetoId && t.Status != StatusTarefa.Concluida)  
                                 .ToListAsync();
        }

        public async Task AddAsync(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
        }

        public Task UpdateAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}