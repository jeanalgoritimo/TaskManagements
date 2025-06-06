// UserProTasks.Domain/Entities/Projeto.cs
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

public class Projeto
{
    public Guid ProjetoId { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public Guid UsuarioId { get; private set; }
    public string NomeUsuario { get; private set; }

    public List<Tarefa> Tarefas { get; private set; } = new();

    public const int LimiteMaximoTarefas = 20; // Nova constante para a regra de negócio

    public Projeto(string nome, string descricao, Guid usuarioId, string nomeUsuario)
    {
        ProjetoId = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        DataCriacao = DateTime.UtcNow;
        UsuarioId = usuarioId;
        NomeUsuario = nomeUsuario;
    }

    public bool PodeSerRemovido() => Tarefas.All(t => t.Status == StatusTarefa.Concluida);
}