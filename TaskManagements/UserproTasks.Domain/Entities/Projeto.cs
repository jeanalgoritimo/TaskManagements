using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Projeto
    {
        public Guid ProjetoId { get;  set; }
        public string Nome { get;  set; }
        public string Descricao { get;  set; }
        public DateTime DataCriacao { get;  set; }
        public Guid UsuarioId { get;  set; }
        public string NomeUsuario { get;  set; }
        public string FuncaoUsuario { get;  set; }

        public const int LimiteMaximoTarefas = 20;

        public ICollection<Tarefa> Tarefas { get;  set; } = new List<Tarefa>();

        // Construtor principal com validações
        public Projeto(string nome, string descricao, Guid usuarioId, string nomeUsuario, string funcaoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nomeUsuario))
                throw new ArgumentException("O nome do usuário é obrigatório.");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do projeto não pode ser nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição do projeto não pode ser nula ou vazia.");

            if (string.IsNullOrWhiteSpace(funcaoUsuario))
                throw new ArgumentException("A função do usuário é obrigatória.");

            if (usuarioId == Guid.Empty)
                throw new ArgumentException("O ID do usuário é inválido.");

            ProjetoId = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            UsuarioId = usuarioId;
            NomeUsuario = nomeUsuario;
            FuncaoUsuario = funcaoUsuario;
            DataCriacao = DateTime.UtcNow;
        }

        // Construtor protegido para EF
        protected Projeto() { }

        // Método de atualização
        public void AtualizarInformacoes(string novoNome, string novaDescricao)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("O nome do projeto não pode ser nulo ou vazio.", nameof(novoNome));

            Nome = novoNome;
            Descricao = novaDescricao;
        }

        // Verifica se ainda existem tarefas pendentes
        public virtual bool HasTarefasPendentes() // Adicione "virtual" aqui!
        {
            return Tarefas != null && Tarefas.Any(t => t.Status != StatusTarefa.Concluida);
        }
    }
}
