using TaskManager.Domain.Enums; 

namespace TaskManager.Domain.Entities 
{
    public class Tarefa
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public Guid ProjetoId { get; set; }
        public Guid UsuarioId { get; set; } 
        public string NomeUsuario { get; set; } 
        public List<Comentario> Comentarios { get; set; } = new();
        public List<HistoricoTarefa> Historico { get; set; } = new();
        public Projeto Projeto { get; set; } 

        public Tarefa(string titulo, string descricao, DateTime dataVencimento, StatusTarefa status, PrioridadeTarefa prioridade, Guid projetoId, Guid usuarioId, string nomeUsuario)
        {
           
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título da tarefa não pode ser nulo ou vazio.", nameof(titulo));
            if (string.IsNullOrWhiteSpace(nomeUsuario))
                throw new ArgumentException("O nome do usuário criador não pode ser nulo ou vazio.", nameof(nomeUsuario));


            Id = Guid.NewGuid();
            Titulo = titulo;
            Descricao = descricao;
            DataVencimento = dataVencimento.ToUniversalTime();
            Prioridade = prioridade;
            Status = status; 
            ProjetoId = projetoId;
            UsuarioId = usuarioId; 
            NomeUsuario = nomeUsuario;
        }

      
        protected Tarefa() { }

        public void AtualizarStatus(StatusTarefa novoStatus, string usuario)
        {
            if (Status == novoStatus) return;
            var anterior = Status;
            Status = novoStatus;
            AdicionarHistorico($"Status alterado de '{anterior}' para '{novoStatus}'.", usuario);
        }

        public void AtualizarDetalhes(string novoTitulo, string novaDescricao, DateTime novaDataVencimento, string usuario)
        {
            bool changed = false;
            if (Titulo != novoTitulo)
            {
                Titulo = novoTitulo;
                changed = true;
            }
            if (Descricao != novaDescricao)
            {
                Descricao = novaDescricao;
                changed = true;
            }

            var novaDataVencimentoUtc = novaDataVencimento.Kind == DateTimeKind.Unspecified
                                            ? DateTime.SpecifyKind(novaDataVencimento, DateTimeKind.Utc)
                                            : novaDataVencimento.ToUniversalTime();

            if (DataVencimento != novaDataVencimentoUtc)
            {
                DataVencimento = novaDataVencimentoUtc;
                changed = true;
            }

            if (changed)
            {
                AdicionarHistorico($"Detalhes da tarefa atualizados.", usuario);
            }
        }

        public void AdicionarComentario(string texto, string usuario)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("O texto do comentário não pode ser nulo ou vazio.", nameof(texto));

            Comentarios.Add(new Comentario(texto, usuario, Id));
            AdicionarHistorico($"Comentário adicionado: '{texto}'.", usuario);
        }

        private void AdicionarHistorico(string descricao, string usuario)
        {
            Historico.Add(new HistoricoTarefa(descricao, usuario, Id));
        }
    }
}