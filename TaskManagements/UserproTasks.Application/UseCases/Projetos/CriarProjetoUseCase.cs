using UserProTasks.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace UserProTasks.Application.UseCases.Projetos
{
    public class CriarProjetoUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public CriarProjetoUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<Projeto> ExecutarAsync(string nome, string descricao, Guid usuarioId, string nomeUsuario,string funcaoUsuario)
        {
            var projeto = new Projeto(nome, descricao, usuarioId, nomeUsuario, funcaoUsuario);
            await _projetoRepository.AddAsync(projeto);
            await _projetoRepository.SaveChangesAsync();
            return projeto;
        }
    }
}