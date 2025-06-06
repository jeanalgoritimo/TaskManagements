// UserProTasks.Application/DTOs/CriarProjetoDto.cs
using System.ComponentModel.DataAnnotations;

namespace UserProTasks.Application.DTOs
{
    public class CriarProjetoDto
    {
        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do projeto não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição do projeto não pode exceder 500 caracteres.")]
        public string Descricao { get; set; }
    }
}