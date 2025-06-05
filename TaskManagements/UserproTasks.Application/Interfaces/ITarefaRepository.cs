using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITarefaRepository
    {
        Task<Tarefa> GetByIdAsync(Guid id);
        Task AddAsync(Tarefa tarefa);
        Task RemoveAsync(Tarefa tarefa);
        Task SaveChangesAsync();
    }
}
