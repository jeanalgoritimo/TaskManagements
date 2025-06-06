using UserProTasks.Application.DTOs;
using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Tarefas
{
    public class VisualizarTarefaUseCase
    {
        private readonly ITarefaRepository _tarefaRepository;

        public VisualizarTarefaUseCase(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<DetalhesTarefaDto> ExecutarAsync(Guid tarefaId)
        {
            var tarefa = await _tarefaRepository.GetByIdWithDetailsAsync(tarefaId); // Carregar detalhes
            if (tarefa == null)
            {
                return null;
            }

            return new DetalhesTarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataVencimento = tarefa.DataVencimento,
                Status = tarefa.Status,
                Prioridade = tarefa.Prioridade,
                ProjetoId = tarefa.ProjetoId,
                // UsuarioCriacao: Precisa ser passado ou inferido de alguma forma
                Comentarios = tarefa.Comentarios.Select(c => new ComentarioDto
                {
                    Id = c.Id,
                    Texto = c.Texto,
                    Usuario = c.Usuario,
                    DataCriacao = c.DataCriacao
                }).ToList(),
                Historico = tarefa.Historico.Select(h => new HistoricoDto
                {
                    Id = h.Id,
                    Descricao = h.Descricao,
                    Usuario = h.Usuario,
                    Data = h.Data
                }).ToList()
            };
        }
    }
}