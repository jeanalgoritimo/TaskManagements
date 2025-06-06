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
        public Guid TarefaId { get; private set; } // Adicionado: Chave estrangeira para Tarefa
        public Tarefa Tarefa { get; private set; }

        public HistoricoTarefa(string descricao, string usuario, Guid tarefaId) // Construtor atualizado
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Usuario = usuario;
            Data = DateTime.UtcNow;
            TarefaId = tarefaId; // Atribuir a FK
        }
    }
}
