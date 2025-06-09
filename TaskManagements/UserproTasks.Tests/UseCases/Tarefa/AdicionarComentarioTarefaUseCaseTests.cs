using Xunit;
using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Tarefas; // Ajustado para o namespace correto (plural)
using TaskManager.Domain.Entities; // Para Tarefa, ComentarioTarefa, HistoricoTarefa
using TaskManager.Domain.Enums; // Para StatusTarefa, PrioridadeTarefa
using System.Threading.Tasks;
using System;
using System.Linq;

namespace UserProTasks.Tests.UseCases.Tarefa
{
    public class AdicionarComentarioTarefaUseCaseTests
    {
        private readonly Mock<ITarefaRepository> _mockTarefaRepository;
        private readonly AdicionarComentarioTarefaUseCase _adicionarComentarioTarefaUseCase;

        public AdicionarComentarioTarefaUseCaseTests()
        {
            _mockTarefaRepository = new Mock<ITarefaRepository>();
            _adicionarComentarioTarefaUseCase = new AdicionarComentarioTarefaUseCase(
                _mockTarefaRepository.Object
            );
        }

    }
}