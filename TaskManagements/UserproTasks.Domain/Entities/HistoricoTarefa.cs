using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class HistoricoTarefa
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public string Usuario { get; private set; }
        public DateTime Data { get; private set; }

        public HistoricoTarefa(string descricao, string usuario)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Usuario = usuario;
            Data = DateTime.UtcNow;
        }
    }
}
