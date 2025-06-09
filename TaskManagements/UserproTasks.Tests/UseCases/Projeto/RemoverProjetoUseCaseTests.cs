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


    }
}