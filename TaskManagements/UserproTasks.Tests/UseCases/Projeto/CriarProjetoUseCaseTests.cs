using Xunit;
using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Projetos; 

namespace UserProTasks.Tests.UseCases.Projeto 
{
    public class CriarProjetoUseCaseTests
    {
        private readonly Mock<IProjetoRepository> _mockProjetoRepository;
        private readonly CriarProjetoUseCase _criarProjetoUseCase;

        public CriarProjetoUseCaseTests()
        {
            _mockProjetoRepository = new Mock<IProjetoRepository>();
            _criarProjetoUseCase = new CriarProjetoUseCase(_mockProjetoRepository.Object);
        }

        [Fact]
        public async Task DeveCriarNovoProjetoComSucesso()
        {
            // Arrange
            var nomeProjeto = "Meu Novo Projeto Teste";
            var descricaoProjeto = "Descrição do meu novo projeto de teste.";
            var usuarioId = Guid.NewGuid();
            var nomeUsuario = "Teste User";

            // Configura o mock para não fazer nada quando AddAsync ou SaveChangesAsync for chamado
            _mockProjetoRepository.Setup(repo => repo.AddAsync(It.IsAny<TaskManager.Domain.Entities.Projeto>())).Returns(Task.CompletedTask);
            _mockProjetoRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);


            // Act
            // A chamada reflete a assinatura do seu ExecutarAsync: string nome, string descricao, Guid usuarioId, string nomeUsuario
            var projetoCriado = await _criarProjetoUseCase.ExecutarAsync(nomeProjeto, descricaoProjeto, usuarioId, nomeUsuario, "Caixa");

            // Assert
            // Verifica se o método AddAsync foi chamado no repositório com um objeto Projeto que corresponde aos dados de entrada
            _mockProjetoRepository.Verify(repo => repo.AddAsync(It.Is<TaskManager.Domain.Entities.Projeto>(p =>
                p.Nome == nomeProjeto &&
                p.Descricao == descricaoProjeto &&
                p.UsuarioId == usuarioId &&
                p.NomeUsuario == nomeUsuario
            )), Times.Once);

            // Verifica se SaveChangesAsync foi chamado para persistir as alterações
            _mockProjetoRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            // Verifica se o projeto retornado não é nulo e suas propriedades estão corretas
            projetoCriado.Should().NotBeNull();
            projetoCriado.Nome.Should().Be(nomeProjeto);
            projetoCriado.Descricao.Should().Be(descricaoProjeto);
            projetoCriado.UsuarioId.Should().Be(usuarioId);
            projetoCriado.NomeUsuario.Should().Be(nomeUsuario);
            projetoCriado.ProjetoId.Should().NotBeEmpty(); // Se o ID for gerado no construtor
            projetoCriado.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5)); // Se DataCriacao for UtcNow no construtor
        }

 
        // Teste para o ListarProjetosUsuarioUseCase (revisado com base nos DTOs)
        [Fact]
        public async Task ListarProjetosUsuarioUseCase_DeveRetornarTodosOsProjetosDoUsuarioComContagensCorretas()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var projeto1Id = Guid.NewGuid();
            var projeto2Id = Guid.NewGuid();

            // Criação de projetos de domínio com tarefas mockadas para testar as contagens
            var projeto1 = new TaskManager.Domain.Entities.Projeto("Projeto A", "Desc A", usuarioId, "User", "caixa");
            projeto1.ProjetoId = projeto1Id; // Atribua o ID se não for feito no construtor
            // Adiciona tarefas ao projeto1. Tarefas precisa ser uma ICollection<Tarefa> na sua entidade Projeto
            projeto1.Tarefas.Add(new TaskManager.Domain.Entities.Tarefa("Tarefa 1-1", "D", DateTime.Now, TaskManager.Domain.Enums.StatusTarefa.Pendente, TaskManager.Domain.Enums.PrioridadeTarefa.Media, projeto1Id, usuarioId, "User"));
            projeto1.Tarefas.Add(new TaskManager.Domain.Entities.Tarefa("Tarefa 1-2", "D", DateTime.Now, TaskManager.Domain.Enums.StatusTarefa.Concluida, TaskManager.Domain.Enums.PrioridadeTarefa.Media, projeto1Id, usuarioId, "User"));
            projeto1.Tarefas.Add(new TaskManager.Domain.Entities.Tarefa("Tarefa 1-3", "D", DateTime.Now, TaskManager.Domain.Enums.StatusTarefa.Pendente, TaskManager.Domain.Enums.PrioridadeTarefa.Media, projeto1Id, usuarioId, "User"));

            var projeto2 = new TaskManager.Domain.Entities.Projeto("Projeto B", "Desc B", usuarioId, "User", "caixa");
            projeto2.ProjetoId = projeto2Id; // Atribua o ID
            projeto2.Tarefas.Add(new TaskManager.Domain.Entities.Tarefa("Tarefa 2-1", "D", DateTime.Now, TaskManager.Domain.Enums.StatusTarefa.Concluida, TaskManager.Domain.Enums.PrioridadeTarefa.Media, projeto2Id, usuarioId, "User"));

            var projetosDoDominio = new List<TaskManager.Domain.Entities.Projeto> { projeto1, projeto2 };

            var mockListarProjetosUsuarioUseCase = new Mock<ListarProjetosUsuarioUseCase>(_mockProjetoRepository.Object); // Mockar o próprio UseCase ou o repositório

            _mockProjetoRepository.Setup(repo => repo.GetAllByUserIdAsync(usuarioId)).ReturnsAsync(projetosDoDominio);

            // Act
            // Chame o método ExecutarAsync do ListarProjetosUsuarioUseCase
            var listarProjetosUseCase = new ListarProjetosUsuarioUseCase(_mockProjetoRepository.Object);
            var resultado = await listarProjetosUseCase.ExecutarAsync(usuarioId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);

            var projetoDto1 = resultado.FirstOrDefault(p => p.ProjetoId == projeto1Id);
            projetoDto1.Should().NotBeNull();
            projetoDto1.Nome.Should().Be("Projeto A");
            projetoDto1.QuantidadeTarefasPendentes.Should().Be(2); // Tarefa 1-1, Tarefa 1-3
            projetoDto1.QuantidadeTarefasConcluidas.Should().Be(1); // Tarefa 1-2

            var projetoDto2 = resultado.FirstOrDefault(p => p.ProjetoId == projeto2Id);
            projetoDto2.Should().NotBeNull();
            projetoDto2.Nome.Should().Be("Projeto B");
            projetoDto2.QuantidadeTarefasPendentes.Should().Be(0);
            projetoDto2.QuantidadeTarefasConcluidas.Should().Be(1); // Tarefa 2-1

            _mockProjetoRepository.Verify(repo => repo.GetAllByUserIdAsync(usuarioId), Times.Once);
        }

        [Fact]
        public async Task ListarProjetosUsuarioUseCase_DeveRetornarListaVaziaSeNaoHouverProjetosParaOUsuario()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            _mockProjetoRepository.Setup(repo => repo.GetAllByUserIdAsync(usuarioId)).ReturnsAsync(Enumerable.Empty<TaskManager.Domain.Entities.Projeto>());

            // Act
            var listarProjetosUseCase = new ListarProjetosUsuarioUseCase(_mockProjetoRepository.Object);
            var resultado = await listarProjetosUseCase.ExecutarAsync(usuarioId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            _mockProjetoRepository.Verify(repo => repo.GetAllByUserIdAsync(usuarioId), Times.Once);
        }
    }
}