using Moq; 
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Tarefas;   

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
    }
}