 
using TaskManager.Domain.Enums;
using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Projetos
{
    public class ListarProjetosUsuarioUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public ListarProjetosUsuarioUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<IEnumerable<ProjetoDto>> ExecutarAsync(Guid usuarioId)
        {
            var projetos = await _projetoRepository.GetAllByUserIdAsync(usuarioId);

            return projetos.Select(p => new ProjetoDto
            {
                ProjetoId = p.ProjetoId,
                Nome = p.Nome,
                Descricao = p.Descricao,
                DataCriacao = p.DataCriacao,
                UsuarioId = p.UsuarioId,
                NomeUsuario = p.NomeUsuario,
                QuantidadeTarefasPendentes = p.Tarefas.Count(t => t.Status != StatusTarefa.Concluida),
                QuantidadeTarefasConcluidas = p.Tarefas.Count(t => t.Status == StatusTarefa.Concluida),
                FuncaoUsuario=p.FuncaoUsuario
            }).ToList();
        }


    }
}