using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums; 

namespace UserProTasks.Application.DTOs
{
    public class CriarTarefaDto
    {
        [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O título da tarefa não pode exceder 200 caracteres.")]
        public string Titulo { get; set; }

        [MaxLength(1000, ErrorMessage = "A descrição da tarefa não pode exceder 1000 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de vencimento da tarefa é obrigatória.")]
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "A prioridade da tarefa é obrigatória.")]
        public PrioridadeTarefa Prioridade { get; set; } // Prioridade só na criação

        [Required(ErrorMessage = "O ID do projeto é obrigatório.")]
        public Guid ProjetoId { get; set; }
        // UsuarioCriacao será inferido na camada de API ou Use Case
    }
}