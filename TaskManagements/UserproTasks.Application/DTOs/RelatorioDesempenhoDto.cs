namespace UserProTasks.Application.DTOs
{
    public class RelatorioDesempenhoDto
    {
        public List<DesempenhoUsuarioDto> DesempenhoPorUsuario { get; set; } = new();
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
        public string PeriodoRelatorio { get; set; } // Ex: "Últimos 30 dias"
        public double NumeroMedioTarefasConcluidasGeral { get; set; }
    }

    public class DesempenhoUsuarioDto
    {
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public int TarefasConcluidasNoPeriodo { get; set; }
        public double MediaTarefasConcluidasDiarias { get; set; }
    }
}
