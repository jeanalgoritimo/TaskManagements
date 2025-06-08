// UserProTasks.Application/DTOs/CriarProjetoDto.cs
using System.ComponentModel.DataAnnotations;

namespace UserProTasks.Application.DTOs
{
    public class CriarProjetoDto
    {
        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do projeto não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O nome do usuário é obrigatório")]
        [MaxLength(100, ErrorMessage = "O nome do usuário não pode exceder 100 caracteres.")]
        public string nomeUsuario { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição do projeto não pode exceder 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O nome da função do usuário é obrigatório")]
        [MaxLength(100, ErrorMessage = "O nome da função do 50 caracteres.")]
        public string funcaoUsuario { get; set; }
    }
}