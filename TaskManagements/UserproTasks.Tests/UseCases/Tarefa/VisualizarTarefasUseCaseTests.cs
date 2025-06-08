using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Tarefas; 
using TaskManager.Domain.Enums; 

namespace UserProTasks.Tests.UseCases.Tarefa
{
    public class ListarTarefasPorProjetoUseCaseTests
    {
        private readonly Mock<ITarefaRepository> _mockTarefaRepository;
        private readonly Mock<IProjetoRepository> _mockProjetoRepository;
        private readonly ListarTarefasProjetoUseCase _listarTarefasPorProjetoUseCase;

        public ListarTarefasPorProjetoUseCaseTests()
        {
            _mockTarefaRepository = new Mock<ITarefaRepository>();
            _mockProjetoRepository = new Mock<IProjetoRepository>();
            _listarTarefasPorProjetoUseCase = new ListarTarefasProjetoUseCase(
                _mockTarefaRepository.Object,
                _mockProjetoRepository.Object
            );
        }

        [Fact]
        public async Task DeveRetornarTodasAsTarefasDeUmProjetoEspecifico()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Teste", "Desc", usuarioId, "User", "caixa");
            var tarefas = new List<TaskManager.Domain.Entities.Tarefa>
            {
                new TaskManager.Domain.Entities.Tarefa("Tarefa 1", "Desc 1", DateTime.Today.AddDays(1), StatusTarefa.Pendente, PrioridadeTarefa.Baixa, projetoId, usuarioId, "User"),
                new TaskManager.Domain.Entities.Tarefa("Tarefa 2", "Desc 2", DateTime.Today.AddDays(2), StatusTarefa.EmAndamento, PrioridadeTarefa.Media, projetoId, usuarioId, "User"),
            };

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
            _mockTarefaRepository.Setup(repo => repo.GetByProjetoIdAsync(projetoId)).ReturnsAsync(tarefas);

            // Act
            // Desestrutura a tupla para obter as tarefas e a mensagem de erro
            var (tarefasDto, errorMessage) = await _listarTarefasPorProjetoUseCase.ExecutarAsync(projetoId);

            // Assert
            errorMessage.Should().BeNull(); // Garante que não há mensagem de erro
            tarefasDto.Should().NotBeNull();
            tarefasDto.Should().HaveCount(2);
            tarefasDto.Select(t => t.Titulo).Should().ContainInOrder("Tarefa 1", "Tarefa 2");
            tarefasDto.Select(t => t.ProjetoId).Should().OnlyContain(id => id == projetoId);

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetByProjetoIdAsync(projetoId), Times.Once);
        }

        [Fact]
        public async Task DeveRetornarListaVaziaSeProjetoNaoTiverTarefas()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Vazio", "Desc", usuarioId, "User", "caixa");

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
            _mockTarefaRepository.Setup(repo => repo.GetByProjetoIdAsync(projetoId)).ReturnsAsync(new List<TaskManager.Domain.Entities.Tarefa>());

            // Act
            var (tarefasDto, errorMessage) = await _listarTarefasPorProjetoUseCase.ExecutarAsync(projetoId);

            // Assert
            errorMessage.Should().BeNull(); // Garante que não há mensagem de erro
            tarefasDto.Should().NotBeNull();
            tarefasDto.Should().BeEmpty();
            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetByProjetoIdAsync(projetoId), Times.Once);
        }

        [Fact]
        public async Task DeveRetornarErroSeProjetoNaoEncontradoAoListarTarefas()
        {
            // Arrange
            var projetoIdInexistente = Guid.NewGuid();

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoIdInexistente)).ReturnsAsync((TaskManager.Domain.Entities.Projeto)null);

            // Act
            var (tarefasDto, errorMessage) = await _listarTarefasPorProjetoUseCase.ExecutarAsync(projetoIdInexistente);

            // Assert
            errorMessage.Should().NotBeNullOrEmpty(); // Garante que há uma mensagem de erro
            errorMessage.Should().Contain("Projeto não encontrado.");
            tarefasDto.Should().BeNull(); // Garante que a lista de tarefas é nula

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoIdInexistente), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetByProjetoIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}