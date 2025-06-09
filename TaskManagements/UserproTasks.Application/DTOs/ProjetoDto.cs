 
namespace UserProTasks.Application.DTOs
{
    public class ProjetoDto
    {
        public Guid ProjetoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string FuncaoUsuario { get; set; }
        public int QuantidadeTarefasPendentes { get; set; } // Para a regra de remoção
        public int QuantidadeTarefasConcluidas { get; set; }
    }
}