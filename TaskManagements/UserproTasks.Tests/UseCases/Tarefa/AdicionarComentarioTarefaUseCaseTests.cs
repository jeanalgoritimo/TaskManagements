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

        [Fact]
        public async Task DeveAdicionarComentarioATarefaComSucessoERegistrarHistorico()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var nomeUsuarioComentario = "Commenter";
            var tarefaExistente = new TaskManager.Domain.Entities.Tarefa(
                "Tarefa Comentada", "Desc", DateTime.Today.AddDays(5),
                StatusTarefa.Pendente, PrioridadeTarefa.Media, Guid.NewGuid(), Guid.NewGuid(), "Criador");

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaId)).ReturnsAsync(tarefaExistente);
           

            var textoComentario = "Este é um novo comentário.";

            // Act
            var comentarioAdicionado = await _adicionarComentarioTarefaUseCase.ExecutarAsync(
                tarefaId, textoComentario, nomeUsuarioComentario);

            // Assert
            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaId), Times.Once);
           

            comentarioAdicionado.Should().NotBeNull();
            comentarioAdicionado.Success.Should();
        }

        [Fact]
        public async Task DeveLancarExcecaoSeTarefaNaoEncontradaAoAdicionarComentario()
        {
            // Arrange
            var tarefaIdInexistente = Guid.NewGuid();

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaIdInexistente)).ReturnsAsync((TaskManager.Domain.Entities.Tarefa)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => // Ou NotFoundException
                _adicionarComentarioTarefaUseCase.ExecutarAsync(
                    tarefaIdInexistente, "Comentário","User")
            );

            exception.Message.Should().Contain("Tarefa não encontrada.");

            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaIdInexistente), Times.Once); 
        }

        [Theory]
        [InlineData(null, "Comentário não pode ser nulo ou vazio.")]
        [InlineData("", "Comentário não pode ser nulo ou vazio.")]
        public async Task DeveLancarExcecaoSeTextoComentarioForInvalido(string textoInvalido, string mensagemEsperada)
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var tarefaExistente = new TaskManager.Domain.Entities.Tarefa(
                "Tarefa Válida", "Desc", DateTime.Today.AddDays(5),
                StatusTarefa.Pendente, PrioridadeTarefa.Media, Guid.NewGuid(), Guid.NewGuid(), "Criador");

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaId)).ReturnsAsync(tarefaExistente);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _adicionarComentarioTarefaUseCase.ExecutarAsync(tarefaId, textoInvalido, "User")
            );

            exception.Message.Should().Contain(mensagemEsperada); 
        }
    }
}