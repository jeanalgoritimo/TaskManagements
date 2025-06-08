 
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Enums;
using UserProTasks.Application.DTOs;
using UserProTasks.Application.UseCases.Tarefas; // Incluir novos use cases

namespace UserProTasks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Se você implementar autenticação
    public class TarefasController : ControllerBase
    {
        private readonly CriarTarefaUseCase _criarTarefaUseCase;
        private readonly ListarTarefasProjetoUseCase _listarTarefasProjetoUseCase;
        private readonly VisualizarTarefaUseCase _visualizarTarefaUseCase;
        private readonly AtualizarTarefaUseCase _atualizarTarefaUseCase;
        private readonly RemoverTarefaUseCase _removerTarefaUseCase;
        private readonly AdicionarComentarioTarefaUseCase _adicionarComentarioTarefaUseCase;

        public TarefasController(
            CriarTarefaUseCase criarTarefaUseCase,
            ListarTarefasProjetoUseCase listarTarefasProjetoUseCase,
            VisualizarTarefaUseCase visualizarTarefaUseCase,
            AtualizarTarefaUseCase atualizarTarefaUseCase,
            RemoverTarefaUseCase removerTarefaUseCase,
            AdicionarComentarioTarefaUseCase adicionarComentarioTarefaUseCase)
        {
            _criarTarefaUseCase = criarTarefaUseCase;
            _listarTarefasProjetoUseCase = listarTarefasProjetoUseCase;
            _visualizarTarefaUseCase = visualizarTarefaUseCase;
            _atualizarTarefaUseCase = atualizarTarefaUseCase;
            _removerTarefaUseCase = removerTarefaUseCase;
            _adicionarComentarioTarefaUseCase = adicionarComentarioTarefaUseCase;
        }

        /// <summary>
        /// Adiciona uma nova tarefa a um projeto existente.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CriarTarefa([FromBody] CriarTarefaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            var (tarefaCriada, errorMessage) = await _criarTarefaUseCase.ExecutarAsync(
                dto.Titulo, dto.Descricao, dto.DataVencimento,StatusTarefa.Pendente, dto.Prioridade, dto.ProjetoId);

            if (tarefaCriada == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            return CreatedAtAction(nameof(CriarTarefa), new { id = tarefaCriada.Id }, tarefaCriada);
        }

        /// <summary>
        /// Lista todas as tarefas de um projeto específico.
        /// </summary>
        [HttpGet("projeto/{projetoId}")]
        public async Task<ActionResult<IEnumerable<TarefaDto>>> ListarTarefasPorProjeto(Guid projetoId)
        {
            var (tarefas, errorMessage) = await _listarTarefasProjetoUseCase.ExecutarAsync(projetoId);

            if (tarefas == null)
            {
                return NotFound(new { message = errorMessage }); // Projeto não encontrado
            }
            return Ok(tarefas);
        }

        /// <summary>
        /// Visualiza os detalhes de uma tarefa específica, incluindo histórico e comentários.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalhesTarefaDto>> VisualizarTarefa(Guid id)
        {
            var tarefa = await _visualizarTarefaUseCase.ExecutarAsync(id);
            if (tarefa == null)
            {
                return NotFound(new { message = "Tarefa não encontrada." });
            }
            return Ok(tarefa);
        }

        /// <summary>
        /// Atualiza o status ou detalhes de uma tarefa.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarTarefa(Guid id, [FromBody] AtualizarTarefaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioAtualizacao = "UsuarioLogado"; // Simulação

            var (success, errorMessage) = await _atualizarTarefaUseCase.ExecutarAsync(id, dto, usuarioAtualizacao);

            if (!success)
            {
                return NotFound(new { message = errorMessage });
            }
            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// Remove uma tarefa de um projeto.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverTarefa(Guid id)
        {
            var (success, errorMessage) = await _removerTarefaUseCase.ExecutarAsync(id);

            if (!success)
            {
                return NotFound(new { message = errorMessage });
            }
            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// Adiciona um comentário a uma tarefa.
        /// </summary>
        [HttpPost("{id}/comentarios")]
        public async Task<IActionResult> AdicionarComentario(Guid id, [FromBody] AdicionarComentarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioComentario = "UsuarioLogado"; // Simulação

            var (success, errorMessage) = await _adicionarComentarioTarefaUseCase.ExecutarAsync(id, dto.Texto, usuarioComentario);

            if (!success)
            {
                return NotFound(new { message = errorMessage });
            }
            return Ok(new { message = "Comentário adicionado com sucesso." });
        }
    }
}