using Xunit;
using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Tarefas;  
using TaskManager.Domain.Enums;  
 

namespace UserProTasks.Tests.UseCases.Tarefa
{
    public class CriarTarefaUseCaseTests
    {
        private readonly Mock<ITarefaRepository> _mockTarefaRepository;
        private readonly Mock<IProjetoRepository> _mockProjetoRepository;
        private readonly CriarTarefaUseCase _criarTarefaUseCase;

        public CriarTarefaUseCaseTests()
        {
            _mockTarefaRepository = new Mock<ITarefaRepository>();
            _mockProjetoRepository = new Mock<IProjetoRepository>();
            _criarTarefaUseCase = new CriarTarefaUseCase(
                 _mockProjetoRepository.Object,
                 _mockTarefaRepository.Object
            );
        }

        [Fact]
        public async Task DeveAdicionarNovaTarefaAProjetoComSucesso()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Teste", "Desc", usuarioId, "User", "caixa");
            // Simula menos de 20 tarefas existentes
            var tarefasExistentes = Enumerable.Range(1, 5).Select(i =>
                new TaskManager.Domain.Entities.Tarefa($"Tarefa {i}", "Desc", DateTime.Today.AddDays(i), StatusTarefa.Pendente, PrioridadeTarefa.Baixa, projetoId, usuarioId, "User")).ToList();

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
            // CORREÇÃO: Use GetByProjetoIdAsync se é o que o UseCase chama para contar tarefas
            _mockTarefaRepository.Setup(repo => repo.GetByProjetoIdAsync(projetoId)).ReturnsAsync(tarefasExistentes);
            _mockTarefaRepository.Setup(repo => repo.AddAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>())).Returns(Task.CompletedTask);
            _mockTarefaRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var nomeTarefa = "Nova Tarefa";
            var descricaoTarefa = "Descrição da nova tarefa.";
            var dataVencimento = DateTime.Today.AddDays(7);
            var status = StatusTarefa.Pendente;
            var prioridade = PrioridadeTarefa.Alta;
            var nomeUsuario = "Usuario Teste";

            // Act
            // Desestruturando a tupla
            var (tarefaResultante, errorMessage) = await _criarTarefaUseCase.ExecutarAsync(
                nomeTarefa, descricaoTarefa, dataVencimento, status, prioridade, projetoId);

            // Assert
            errorMessage.Should().BeNull(); // Garante que não há erro
            tarefaResultante.Should().NotBeNull(); // Garante que a tarefa foi retornada

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoId), Times.Once);
            // CORREÇÃO: Verifique o método correto que o UseCase chamaria para contar tarefas
            _mockTarefaRepository.Verify(repo => repo.GetByProjetoIdAsync(projetoId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.AddAsync(It.Is<TaskManager.Domain.Entities.Tarefa>(t =>
                t.Titulo == nomeTarefa &&
                t.Descricao == descricaoTarefa &&
                t.DataVencimento == dataVencimento &&
                t.Prioridade == prioridade &&
                t.ProjetoId == projetoId &&
                t.UsuarioId == usuarioId && // Certifique-se que sua entidade Tarefa tem UsuarioId
                t.NomeUsuario == nomeUsuario &&
                t.Status == StatusTarefa.Pendente // Deve iniciar como Pendente
            )), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            // CORREÇÃO: Acessar a propriedade Tarefa da tupla para verificar suas propriedades
            tarefaResultante.Titulo.Should().Be(nomeTarefa);
            tarefaResultante.Prioridade.Should().Be(prioridade);
            tarefaResultante.Status.Should().Be(StatusTarefa.Pendente);
        }

        [Fact]
        public async Task NaoDevePermitirAdicionarMaisDe20TarefasAProjeto()
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Cheio", "Desc", usuarioId, "User", "caixa");
            // Simula 20 tarefas existentes
            var tarefasExistentes = Enumerable.Range(1, 20).Select(i =>
                new TaskManager.Domain.Entities.Tarefa($"Tarefa {i}", "Desc", DateTime.Today.AddDays(i), StatusTarefa.Pendente, PrioridadeTarefa.Baixa, projetoId, usuarioId, "User")).ToList();

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
            // CORREÇÃO: Use GetByProjetoIdAsync se é o que o UseCase chama para contar tarefas
            _mockTarefaRepository.Setup(repo => repo.GetByProjetoIdAsync(projetoId)).ReturnsAsync(tarefasExistentes);

            // Act
            var (tarefaResultante, errorMessage) = await _criarTarefaUseCase.ExecutarAsync( // Desestruturando a tupla
                    "Tarefa Extra", "Desc", DateTime.Today, StatusTarefa.Pendente, PrioridadeTarefa.Baixa, projetoId);

            // Assert
            errorMessage.Should().NotBeNullOrEmpty(); // Garante que há mensagem de erro
            errorMessage.Should().Contain("O projeto atingiu o limite máximo de 20 tarefas.");
            tarefaResultante.Should().BeNull(); // Garante que nenhuma tarefa foi criada

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetByProjetoIdAsync(projetoId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.AddAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeProjetoNaoEncontradoAoCriarTarefa()
        {
            // Arrange
            var projetoIdInexistente = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoIdInexistente)).ReturnsAsync((TaskManager.Domain.Entities.Projeto)null);

            // Act
            var (tarefaResultante, errorMessage) = await _criarTarefaUseCase.ExecutarAsync( // Desestruturando a tupla
                    "Nova Tarefa", "Desc", DateTime.Today, StatusTarefa.Pendente, PrioridadeTarefa.Media, projetoIdInexistente);

            // Assert
            errorMessage.Should().NotBeNullOrEmpty(); // Garante que há mensagem de erro
            errorMessage.Should().Contain("Projeto não encontrado.");
            tarefaResultante.Should().BeNull(); // Garante que nenhuma tarefa foi criada

            _mockProjetoRepository.Verify(repo => repo.GetByIdAsync(projetoIdInexistente), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetByProjetoIdAsync(It.IsAny<Guid>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.AddAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(null, "Desc", "Nome da tarefa não pode ser nulo ou vazio.")]
        [InlineData("", "Desc", "Nome da tarefa não pode ser nulo ou vazio.")]
        public async Task DeveRetornarErroSeTituloTarefaForInvalido(string tituloInvalido, string descricao, string mensagemEsperada)
        {
            // Arrange
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var projetoExistente = new TaskManager.Domain.Entities.Projeto("Projeto Teste", "Desc", usuarioId, "User", "caixa");
            _mockProjetoRepository.Setup(repo => repo.GetByIdAsync(projetoId)).ReturnsAsync(projetoExistente);
            _mockTarefaRepository.Setup(repo => repo.GetByProjetoIdAsync(projetoId)).ReturnsAsync(new List<TaskManager.Domain.Entities.Tarefa>());

            // Act
            var (tarefaResultante, errorMessage) = await _criarTarefaUseCase.ExecutarAsync( // Desestruturando a tupla
                    tituloInvalido, descricao, DateTime.Today, StatusTarefa.Pendente, PrioridadeTarefa.Baixa, projetoId);

            // Assert
            errorMessage.Should().NotBeNullOrEmpty(); // Garante que há mensagem de erro
            errorMessage.Should().Contain(mensagemEsperada);
            tarefaResultante.Should().BeNull(); // Garante que nenhuma tarefa foi criada

            _mockTarefaRepository.Verify(repo => repo.AddAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}