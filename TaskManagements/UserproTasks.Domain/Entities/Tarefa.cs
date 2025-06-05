using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Tarefa
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public StatusTarefa Status { get; private set; }
        public PrioridadeTarefa Prioridade { get; private set; }
        public Guid ProjetoId { get; private set; }
        public List<Comentario> Comentarios { get; private set; } = new();
        public List<HistoricoTarefa> Historico { get; private set; } = new();

        public Tarefa(string titulo, string descricao, DateTime dataVencimento, PrioridadeTarefa prioridade, Guid projetoId)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Descricao = descricao;
            DataVencimento = dataVencimento;
            Prioridade = prioridade;
            Status = StatusTarefa.Pendente;
            ProjetoId = projetoId;
        }

        public void AtualizarStatus(StatusTarefa novoStatus, string usuario)
        {
            var anterior = Status;
            Status = novoStatus;
            AdicionarHistorico($"Status alterado de {anterior} para {novoStatus}", usuario);
        }

        public void AdicionarComentario(string texto, string usuario)
        {
            Comentarios.Add(new Comentario(texto, usuario));
            AdicionarHistorico($"Comentário adicionado: {texto}", usuario);
        }

        private void AdicionarHistorico(string descricao, string usuario)
        {
            Historico.Add(new HistoricoTarefa(descricao, usuario));
        }
    }

}
