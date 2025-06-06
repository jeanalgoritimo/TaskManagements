 namespace UserProTasks.Application.DTOs
{
    public class ComentarioDto
    {
        public Guid Id { get; set; }
        public string Texto { get; set; }
        public string Usuario { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
