// TaskManager.Domain.Entities/Projeto.cs
using System;
using System.Collections.Generic;
using TaskManager.Domain.Enums; // Para StatusTarefa, se a entidade Projeto precisar dela diretamente para a lógica de negócio

namespace TaskManager.Domain.Entities
{
    public class Projeto
    {
        public Guid ProjetoId { get;  set; } // Ajustado para ProjetoId
        public string Nome { get;  set; }
        public string Descricao { get;  set; }
        public DateTime DataCriacao { get;  set; }
        public Guid UsuarioId { get;  set; }
        public string NomeUsuario { get;  set; }
        public string FuncaoUsuario { get; set; }

        public const int LimiteMaximoTarefas = 20; // Nova constante para a regra de negócio

        // Propriedade de navegação para Tarefas
        public ICollection<Tarefa> Tarefas { get;  set; } = new List<Tarefa>();

        // Construtor principal
        public Projeto(string nome, string descricao, Guid usuarioId, string nomeUsuario, string funcaoUsuario)
        {
            // Validações básicas no construtor da entidade
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do projeto não pode ser nulo ou vazio.", nameof(nome));
            if (string.IsNullOrWhiteSpace(nomeUsuario))
                throw new ArgumentException("O nome do usuário criador do projeto não pode ser nulo ou vazio.", nameof(nomeUsuario));

            ProjetoId = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            DataCriacao = DateTime.UtcNow; // Sempre armazenar em UTC
            UsuarioId = usuarioId;
            NomeUsuario = nomeUsuario;
            FuncaoUsuario = funcaoUsuario;
        }

        // Construtor sem parâmetros para Entity Framework Core
        protected Projeto() { }

        // Método para atualizar informações do projeto (exemplo, se houver)
        public void AtualizarInformacoes(string novoNome, string novaDescricao)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("O nome do projeto não pode ser nulo ou vazio.", nameof(novoNome));

            Nome = novoNome;
            Descricao = novaDescricao;
            // Poderia adicionar lógica de histórico aqui também, se aplicável
        }

        // Método para verificar se há tarefas pendentes (útil para o RemoverProjetoUseCase)
        public bool HasTarefasPendentes()
        {
            // Certifique-se de que a coleção Tarefas foi carregada (via Include no repositório)
            return Tarefas != null && Tarefas.Any(t => t.Status != StatusTarefa.Concluida);
        }
         
    }
}