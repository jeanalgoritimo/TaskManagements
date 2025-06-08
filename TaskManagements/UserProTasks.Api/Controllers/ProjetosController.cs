
using Microsoft.AspNetCore.Mvc; 
using UserProTasks.Application.DTOs;
using UserProTasks.Application.UseCases.Projetos; // Incluir os novos use cases

namespace UserProTasks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Se você implementar autenticação
    public class ProjetosController : ControllerBase
    {
        private readonly CriarProjetoUseCase _criarProjetoUseCase;
        private readonly ListarProjetosUsuarioUseCase _listarProjetosUsuarioUseCase;
        private readonly RemoverProjetoUseCase _removerProjetoUseCase; 
        private readonly ListarUsuariosDosProjetosUseCase _listarUsuariosDosProjetosUseCase;
        public ProjetosController(
            CriarProjetoUseCase criarProjetoUseCase,
            ListarProjetosUsuarioUseCase listarProjetosUsuarioUseCase,
            RemoverProjetoUseCase removerProjetoUseCase,
            ListarUsuariosDosProjetosUseCase listarUsuariosDosProjetosUseCase)
        {
            _criarProjetoUseCase = criarProjetoUseCase;
            _listarProjetosUsuarioUseCase = listarProjetosUsuarioUseCase;
            _removerProjetoUseCase = removerProjetoUseCase;
            _listarUsuariosDosProjetosUseCase = listarUsuariosDosProjetosUseCase;
        }

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CriarProjeto([FromBody] CriarProjetoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Simulação de usuário logado. Em um cenário real, obteria do token JWT.
            var usuarioId = Guid.NewGuid(); // EX: Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

            var projeto = await _criarProjetoUseCase.ExecutarAsync(dto.Nome, dto.Descricao, usuarioId, dto.nomeUsuario, dto.funcaoUsuario);
            return CreatedAtAction(nameof(CriarProjeto), new { id = projeto.ProjetoId }, projeto);
        }

        /// <summary>
        /// Lista todos os projetos do usuário logado.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetoDto>>> ListarProjetos(Guid usuarioId)
        {
            // Simulação de usuário logado.
            //var usuarioId = Guid.NewGuid(); // EX: Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

            var projetos = await _listarProjetosUsuarioUseCase.ExecutarAsync(usuarioId);
            return Ok(projetos);
        }

        /// <summary>
        /// Remove um projeto.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProjeto(Guid id)
        {
            var (success, errorMessage) = await _removerProjetoUseCase.ExecutarAsync(id);

            if (!success)
            {
                if (errorMessage.Contains("não encontrado"))
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage }); // Para a regra de tarefas pendentes
            }

            return NoContent(); // 204 No Content
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuariosDosProjetos()
        {
            var usuarios = await _listarUsuariosDosProjetosUseCase.ExecutarAsync();
            return Ok(usuarios);
        }
    }
}