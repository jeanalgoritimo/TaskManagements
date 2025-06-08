using Xunit;
using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Projetos;

namespace UserProTasks.Tests.UseCases.Projeto
{
    public class RemoverProjetoUseCaseTests
    {
        private readonly Mock<IProjetoRepository> _mockProjetoRepository;
        private readonly RemoverProjetoUseCase _removerProjetoUseCase;

        public RemoverProjetoUseCaseTests()
        {
            _mockProjetoRepository = new Mock<IProjetoRepository>();
            _removerProjetoUseCase = new RemoverProjetoUseCase(
                _mockProjetoRepository.Object
            );
        }

        [Fact]
        public async Task DeveRemoverProjetoComSucessoSeNaoHouverTarefasPendentes()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Teste", "Desc", Guid.NewGuid(), "User", "caixa");

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
           
            _mockProjetoRepository.Setup(repo => repo.DeleteAsync(projetoExistente)).Returns(Task.CompletedTask);
            _mockProjetoRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _removerProjetoUseCase.ExecutarAsync(projetoId);

            // Assert
            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoId), Times.Once);
            _mockProjetoRepository.Verify(repo => repo.DeleteAsync(projetoExistente), Times.Once);
            _mockProjetoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task NaoDeveRemoverProjetoComTarefasPendentes()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Com Tarefas", "Desc", Guid.NewGuid(), "User", "caixa");
            var tarefasPendentes = new List<TaskManager.Domain.Entities.Tarefa>
            {
                new TaskManager.Domain.Entities.Tarefa("Tarefa Pendente 1", "Desc", DateTime.Today.AddDays(1), TaskManager.Domain.Enums.StatusTarefa.Pendente, TaskManager.Domain.Enums.PrioridadeTarefa.Media, projetoId, Guid.NewGuid(), "User")
            };

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
           

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _removerProjetoUseCase.ExecutarAsync(projetoId)
            );

            exception.Message.Should().Contain("O projeto não pode ser removido pois possui tarefas pendentes.");

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoId), Times.Once);
            // Garante que DeleteAsync e SaveChangesAsync NUNCA foram chamados
            _mockProjetoRepository.Verify(repo => repo.DeleteAsync(It.IsAny<TaskManager.Domain.Entities.Projeto>()), Times.Never);
            _mockProjetoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeProjetoNaoEncontrado()
        {
            // Arrange
            var projetoIdInexistente = Guid.NewGuid();

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoIdInexistente)).ReturnsAsync((TaskManager.Domain.Entities.Projeto)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => // Ou NotFoundException se você tiver uma
                _removerProjetoUseCase.ExecutarAsync(projetoIdInexistente)
            );

            exception.Message.Should().Contain("Projeto não encontrado.");

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoIdInexistente), Times.Once);
            _mockProjetoRepository.Verify(repo => repo.DeleteAsync(It.IsAny<TaskManager.Domain.Entities.Projeto>()), Times.Never);
            _mockProjetoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}