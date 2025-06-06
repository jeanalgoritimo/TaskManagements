 
using System.Collections.Generic; 

namespace UserProTasks.Application.DTOs
{
    public class DetalhesProjetoDto : ProjetoDto
    {
        public List<TarefaDto> Tarefas { get; set; } = new();
    }
}