using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserproTasks.Application.DTOs;
using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Projetos
{
    public class ListarUsuariosDosProjetosUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public ListarUsuariosDosProjetosUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<IEnumerable<UsuarioDto>> ExecutarAsync()
        {
            return await _projetoRepository.GetUsuariosDosProjetosAsync();
        }
    }
}
