using System.ComponentModel.DataAnnotations;

namespace UserProTasks.Application.DTOs
{
    public class AdicionarComentarioDto
    {
        [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
        [MaxLength(500, ErrorMessage = "O texto do comentário não pode exceder 500 caracteres.")]
        public string Texto { get; set; }
    }
}