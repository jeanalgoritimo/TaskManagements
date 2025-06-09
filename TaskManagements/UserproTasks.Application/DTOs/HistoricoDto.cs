namespace UserProTasks.Application.DTOs
{
    public class HistoricoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
    }
}