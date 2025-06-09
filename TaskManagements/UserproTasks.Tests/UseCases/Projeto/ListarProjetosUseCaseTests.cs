using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Projetos; // O namespace do UseCase
 
namespace UserProTasks.Tests.UseCases.Projeto
{
    public class ListarProjetosUseCaseTests
    {
        private readonly Mock<IProjetoRepository> _mockProjetoRepository;
        // Mude para o tipo correto do UseCase e não o mock dele
        private readonly ListarProjetosUsuarioUseCase _listarProjetosUsuarioUseCase; // Nome correto da classe do UseCase

        public ListarProjetosUseCaseTests()
        {
            _mockProjetoRepository = new Mock<IProjetoRepository>();
            // Instancie a classe real que você quer testar, passando o mock do repositório
            _listarProjetosUsuarioUseCase = new ListarProjetosUsuarioUseCase(_mockProjetoRepository.Object);
        }

        [Fact]
        public async Task DeveRetornarTodosOsProjetosDoUsuario()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var projetos = new List<TaskManager.Domain.Entities.Projeto>
            {
                new TaskManager.Domain.Entities.Projeto("Projeto A", "Desc A", usuarioId, "User", "caixa"),
                new TaskManager.Domain.Entities.Projeto("Projeto B", "Desc B", usuarioId, "User", "caixa"),
            };

            // Setup correto: use o método que o UseCase realmente chama
            _mockProjetoRepository.Setup(repo => repo.GetAllByUserIdAsync(usuarioId)).ReturnsAsync(projetos);

            // Act
            // Chame o método correto do UseCase
            var resultado = await _listarProjetosUsuarioUseCase.ExecutarAsync(usuarioId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);

            // Ao comparar DTOs, você precisa se certificar que a conversão está correta.
            // Para simplificar, vou comparar as propriedades relevantes.
            // Se ProjetoDto tiver as mesmas propriedades que Projeto, BeEquivalentTo pode funcionar.
            // No entanto, como o UseCase retorna ProjetoDto, e você tem TarefasPendentes/Concluidas calculadas,
            // a comparação direta com a lista de entidades 'projetos' não será 100% equivalente para essas propriedades.
            // Se você quer testar a lógica de cálculo de tarefas, teria que criar DTOs esperados.
            // Por enquanto, vamos focar na existência e contagem.
            // resultado.Should().BeEquivalentTo(projetos); // Remova ou ajuste esta linha se o DTO tiver mais campos/lógica

            // Vamos verificar as propriedades que o DTO deve ter e a contagem.
            // Se você quer verificar os dados específicos, crie os DTOs esperados.
            // Exemplo de verificação de propriedade:
            resultado.Select(p => p.Nome).Should().ContainInOrder("Projeto A", "Projeto B");
            // Option 1: Use AllSatisfy for more complex assertions on each element
            resultado.Should().AllSatisfy(p => p.UsuarioId.Should().Be(usuarioId));

            // Option 2: Select the IDs and then assert all of them are equal to the expected ID
            resultado.Select(p => p.UsuarioId).Should().AllBeEquivalentTo(usuarioId); // This might be more appropriate if the DTOs are being compared, as BeEquivalentTo handles structural equality
                                                                                      // Or simply
            resultado.Select(p => p.UsuarioId).Should().OnlyContain(id => id == usuarioId);


            // Verify correto: use o método que o UseCase realmente chama
            _mockProjetoRepository.Verify(repo => repo.GetAllByUserIdAsync(usuarioId), Times.Once);
        }

        [Fact]
        public async Task DeveRetornarListaVaziaSeNaoHouverProjetosParaOUsuario()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            // Setup correto: use o método que o UseCase realmente chama
            _mockProjetoRepository.Setup(repo => repo.GetAllByUserIdAsync(usuarioId)).ReturnsAsync(new List<TaskManager.Domain.Entities.Projeto>());

            // Act
            // Chame o método correto do UseCase
            var resultado = await _listarProjetosUsuarioUseCase.ExecutarAsync(usuarioId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            // Verify correto: use o método que o UseCase realmente chama
            _mockProjetoRepository.Verify(repo => repo.GetAllByUserIdAsync(usuarioId), Times.Once);
        }
    }
}