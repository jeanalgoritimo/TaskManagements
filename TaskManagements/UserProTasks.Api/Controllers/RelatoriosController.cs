using Microsoft.AspNetCore.Mvc; 
using UserProTasks.Application.DTOs;
using UserProTasks.Application.UseCases.Relatorios;
// using Microsoft.AspNetCore.Authorization; // Descomente quando implementar autenticação

namespace UserProTasks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Gerente")] // Protege o endpoint. Requer configuração de autenticação/autorização
    public class RelatoriosController : ControllerBase
    {
        private readonly GerarRelatorioDesempenhoUseCase _gerarRelatorioDesempenhoUseCase;

        public RelatoriosController(GerarRelatorioDesempenhoUseCase gerarRelatorioDesempenhoUseCase)
        {
            _gerarRelatorioDesempenhoUseCase = gerarRelatorioDesempenhoUseCase;
        }

        /// <summary>
        /// Gera um relatório de desempenho, mostrando a média de tarefas concluídas por usuário.
        /// Apenas acessível por usuários com a função 'Gerente'.
        /// </summary>
        /// <param name="diasRetroativos">Número de dias para analisar o histórico (padrão: 30).</param>
        [HttpGet("desempenho")]
        public async Task<ActionResult<RelatorioDesempenhoDto>> GerarRelatorioDesempenho([FromQuery] int diasRetroativos = 30)
        {
            // Em um sistema real, o 'usuarioGerador' viria do token JWT do usuário logado.
            var usuarioGerador = "GerenteTeste"; // Simulação

            var relatorio = await _gerarRelatorioDesempenhoUseCase.ExecutarAsync(diasRetroativos, usuarioGerador);
            return Ok(relatorio);
        }
    }
}