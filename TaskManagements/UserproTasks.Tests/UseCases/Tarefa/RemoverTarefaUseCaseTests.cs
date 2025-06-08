using Xunit;
using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Tarefas; // Ajustado para o namespace correto (plural)
using TaskManager.Domain.Entities; // Para Tarefa
using System.Threading.Tasks;
using System;

namespace UserProTasks.Tests.UseCases.Tarefa
{
    public class RemoverTarefaUseCaseTests
    {
        private readonly Mock<ITarefaRepository> _mockTarefaRepository;
        private readonly RemoverTarefaUseCase _removerTarefaUseCase;

        public RemoverTarefaUseCaseTests()
        {
            _mockTarefaRepository = new Mock<ITarefaRepository>();
            _removerTarefaUseCase = new RemoverTarefaUseCase(_mockTarefaRepository.Object);
        }

        [Fact]
        public async Task DeveRemoverTarefaDeProjetoComSucesso()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var tarefaExistente = new TaskManager.Domain.Entities.Tarefa(
                "Tarefa para Remover", "Desc", DateTime.Today, TaskManager.Domain.Enums.StatusTarefa.Concluida,
                TaskManager.Domain.Enums.PrioridadeTarefa.Baixa, Guid.NewGuid(), Guid.NewGuid(), "User");

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaId)).ReturnsAsync(tarefaExistente);
            _mockTarefaRepository.Setup(repo => repo.DeleteAsync(tarefaExistente)).Returns(Task.CompletedTask);
            _mockTarefaRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _removerTarefaUseCase.ExecutarAsync(tarefaId);

            // Assert
            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.DeleteAsync(tarefaExistente), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeTarefaNaoEncontradaAoRemover()
        {
            // Arrange
            var tarefaIdInexistente = Guid.NewGuid();

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaIdInexistente)).ReturnsAsync((TaskManager.Domain.Entities.Tarefa)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => // Ou NotFoundException
                _removerTarefaUseCase.ExecutarAsync(tarefaIdInexistente)
            );

            exception.Message.Should().Contain("Tarefa não encontrada.");

            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaIdInexistente), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.DeleteAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}