using Microsoft.AspNetCore.Mvc; 
using UserProTasks.Application.DTOs;
using UserProTasks.Application.UseCases.Relatorios;

namespace UserProTasks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<RelatorioDesempenhoDto>> GerarRelatorioDesempenho([FromQuery] int diasRetroativos = 30, string usuarioGerador ="")
        {
            // Relatório de desempenho. 
            var relatorio = await _gerarRelatorioDesempenhoUseCase.ExecutarAsync(diasRetroativos, usuarioGerador);
            return Ok(relatorio);
        }
    }
}