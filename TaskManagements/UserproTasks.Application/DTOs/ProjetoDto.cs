using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserproTasks.Application.DTOs
{
    public class ProjetoDto
    {
        public Guid ProjetoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
    }
}
