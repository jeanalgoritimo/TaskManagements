using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IProjetoRepository
    {
        Task<Projeto> GetByIdAsync(Guid id);
        Task<List<Projeto>> GetByUsuarioIdAsync(Guid usuarioId);
        Task AddAsync(Projeto projeto);
        Task RemoveAsync(Projeto projeto);
        Task SaveChangesAsync();
    }
}
