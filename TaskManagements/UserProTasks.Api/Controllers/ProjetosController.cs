using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Application.UseCases.Projetos;
using UserproTasks.Application.DTOs;

namespace UserProTasks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly CriarProjetoUseCase _criarProjetoUseCase;

        public ProjetosController(
            IProjetoRepository projetoRepository,
            CriarProjetoUseCase criarProjetoUseCase)
        {
            _projetoRepository = projetoRepository;
            _criarProjetoUseCase = criarProjetoUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> ListarProjetos(Guid usuarioId)
        {
            var projetos = await _projetoRepository.GetByUsuarioIdAsync(usuarioId);
            return Ok(projetos);
        }

        [HttpPost]
        public async Task<IActionResult> CriarProjeto([FromBody] CriarProjetoDto dto)
        {
            var id = await _criarProjetoUseCase.ExecutarAsync(
                dto.Nome,
                dto.Descricao,
                dto.UsuarioId,
                dto.NomeUsuario
            );

            return CreatedAtAction(nameof(ListarProjetos), new { usuarioId = dto.UsuarioId }, new { id });
        }

    }


}
