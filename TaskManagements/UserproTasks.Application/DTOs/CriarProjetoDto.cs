using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserproTasks.Application.DTOs
{
    public class CriarProjetoDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Guid UsuarioId { get; set; }   // quem está criando
        public string NomeUsuario { get; set; }
    }
}
