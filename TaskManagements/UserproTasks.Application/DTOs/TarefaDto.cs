using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Enums; 
namespace UserProTasks.Application.DTOs
{
    public class TarefaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public Guid ProjetoId { get; set; }
        public string UsuarioAtribuido { get; set; } // Se você tiver atribuição de usuário na tarefa
    }
}
