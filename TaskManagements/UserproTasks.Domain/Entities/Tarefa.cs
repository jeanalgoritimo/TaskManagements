using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums; 

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
    public Projeto Projeto { get; private set; }

    public Tarefa(string titulo, string descricao, DateTime dataVencimento, PrioridadeTarefa prioridade, Guid projetoId)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento.ToUniversalTime(); // Garante UTC
        Prioridade = prioridade;
        Status = StatusTarefa.Pendente;
        ProjetoId = projetoId;
    }

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
        Comentarios.Add(new Comentario(texto, usuario, Id));
        AdicionarHistorico($"Comentário adicionado: '{texto}'.", usuario);
    }

    private void AdicionarHistorico(string descricao, string usuario)
    {
        Historico.Add(new HistoricoTarefa(descricao, usuario, Id));
    }
}