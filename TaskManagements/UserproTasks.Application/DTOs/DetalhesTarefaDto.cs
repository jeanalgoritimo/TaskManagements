 using TaskManager.Domain.Enums;
 
namespace UserProTasks.Application.DTOs
{
    public class DetalhesTarefaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public Guid ProjetoId { get; set; }
        public string UsuarioCriacao { get; set; } 
        public List<ComentarioDto> Comentarios { get; set; } = new();
        public List<HistoricoDto> Historico { get; set; } = new();
    }
}
