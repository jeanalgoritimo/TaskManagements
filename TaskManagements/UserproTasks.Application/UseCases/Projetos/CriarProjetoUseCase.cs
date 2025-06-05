using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;


namespace TaskManager.Application.UseCases.Projetos
{
    public class CriarProjetoUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public CriarProjetoUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<Guid> ExecutarAsync(string nome, string descricao, Guid usuarioId, string nomeUsuario)
        {
            // Gera UsuarioId se não for passado
            if (usuarioId == Guid.Empty)
            {
                usuarioId = Guid.NewGuid();
            }

            var projeto = new Projeto(
                nome: nome,
                descricao: descricao,
                usuarioId: usuarioId,
                nomeUsuario: nomeUsuario
            );

            await _projetoRepository.AddAsync(projeto);
            await _projetoRepository.SaveChangesAsync();

            return projeto.ProjetoId;
        }
    }

}
