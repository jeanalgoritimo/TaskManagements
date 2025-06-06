using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace UserProTasks.Application.DTOs
{
    public class AtualizarTarefaDto
    {
        [MaxLength(200, ErrorMessage = "O título da tarefa não pode exceder 200 caracteres.")]
        public string Titulo { get; set; }

        [MaxLength(1000, ErrorMessage = "A descrição da tarefa não pode exceder 1000 caracteres.")]
        public string Descricao { get; set; }

        public DateTime? DataVencimento { get; set; } // Nullable, para permitir atualização parcial

        public StatusTarefa? Status { get; set; } // Nullable, para permitir atualização parcial
        // PRIORIDADE NÃO INCLUÍDA AQUI PELA REGRA DE NEGÓCIO
    }
}