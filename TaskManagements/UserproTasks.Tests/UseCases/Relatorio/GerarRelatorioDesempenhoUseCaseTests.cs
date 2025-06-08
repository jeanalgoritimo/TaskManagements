using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Relatorios;
using TaskManager.Domain.Enums;
using DomainProjeto = TaskManager.Domain.Entities.Projeto;
using DomainTarefa = TaskManager.Domain.Entities.Tarefa;

namespace UserProTasks.Tests.UseCases.Relatorio
{
    public class GerarRelatorioDesempenhoUseCaseTests
    {
        private readonly Mock<ITarefaRepository> _mockTarefaRepository;
        private readonly Mock<IProjetoRepository> _mockProjetoRepository;
        private readonly GerarRelatorioDesempenhoUseCase _gerarRelatorioDesempenhoUseCase;

        public GerarRelatorioDesempenhoUseCaseTests()
        {
            _mockTarefaRepository = new Mock<ITarefaRepository>();
            _mockProjetoRepository = new Mock<IProjetoRepository>();
            _gerarRelatorioDesempenhoUseCase = new GerarRelatorioDesempenhoUseCase(
                _mockTarefaRepository.Object,
                _mockProjetoRepository.Object
            );
        }

        [Fact]
        public async Task DeveGerarRelatorioDeDesempenhoCorretamente()
        {
            // Arrange
            var gerenteId = Guid.NewGuid();
            var usuarioNormalId = Guid.NewGuid();
            var diasRetroativos = 30;

            // Dados de tarefas para usuários
            var tarefasConcluidasGerente = new List<DomainTarefa> // Use DomainTarefa
            {
                new DomainTarefa("T1", "D", DateTime.UtcNow.AddDays(-5), StatusTarefa.Concluida, PrioridadeTarefa.Baixa, Guid.NewGuid(), gerenteId, "Gerente"),
                new DomainTarefa("T2", "D", DateTime.UtcNow.AddDays(-10), StatusTarefa.Concluida, PrioridadeTarefa.Media, Guid.NewGuid(), gerenteId, "Gerente"),
                new DomainTarefa("T3", "D", DateTime.UtcNow.AddDays(-15), StatusTarefa.Concluida, PrioridadeTarefa.Alta, Guid.NewGuid(), gerenteId, "Gerente")
            };
            var tarefasConcluidasUsuarioNormal = new List<DomainTarefa> // Use DomainTarefa
            {
                new DomainTarefa("T4", "D", DateTime.UtcNow.AddDays(-2), StatusTarefa.Concluida, PrioridadeTarefa.Baixa, Guid.NewGuid(), usuarioNormalId, "Normal User"),
                new DomainTarefa("T5", "D", DateTime.UtcNow.AddDays(-25), StatusTarefa.Concluida, PrioridadeTarefa.Media, Guid.NewGuid(), usuarioNormalId, "Normal User")
            };

            // Mocar o GetConcluidasPorUsuarioDesdeAsync para cada usuário relevante
            _mockTarefaRepository.Setup(repo => repo.GetConcluidasPorUsuarioDesdeAsync(gerenteId, It.IsAny<DateTime>()))
                                 .ReturnsAsync(tarefasConcluidasGerente); // ReturnsAsync funciona com List<T>
            _mockTarefaRepository.Setup(repo => repo.GetConcluidasPorUsuarioDesdeAsync(usuarioNormalId, It.IsAny<DateTime>()))
                                 .ReturnsAsync(tarefasConcluidasUsuarioNormal); // ReturnsAsync funciona com List<T>


            // Mocar IProjetoRepository.GetAllAsync para que o UseCase encontre os usuários
            var projetos = new List<DomainProjeto> // Use DomainProjeto
            {
                new DomainProjeto("Proj do Gerente", "Desc", gerenteId, "Gerente novo","Gerente"),
                new DomainProjeto("Proj do Normal", "Desc", usuarioNormalId,"Gerente novo 2","Gerente"),
            };
            _mockProjetoRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projetos); // ReturnsAsync funciona com List<T>


            // Act
            var resultado = await _gerarRelatorioDesempenhoUseCase.ExecutarAsync(diasRetroativos, "Normal User");

            // Assert
            resultado.Should().NotBeNull();
            resultado.DesempenhoPorUsuario.Should().HaveCount(2);

            var gerenteDesempenho = resultado.DesempenhoPorUsuario.Should().ContainSingle(d => d.UsuarioId == gerenteId).Subject;
            gerenteDesempenho.TarefasConcluidasNoPeriodo.Should().Be(3);
            gerenteDesempenho.MediaTarefasConcluidasDiarias.Should().BeApproximately((double)3 / diasRetroativos, 0.001);

            var usuarioNormalDesempenho = resultado.DesempenhoPorUsuario.Should().ContainSingle(d => d.UsuarioId == usuarioNormalId).Subject;
            usuarioNormalDesempenho.TarefasConcluidasNoPeriodo.Should().Be(2);
            usuarioNormalDesempenho.MediaTarefasConcluidasDiarias.Should().BeApproximately((double)2 / diasRetroativos, 0.001);

            resultado.NumeroMedioTarefasConcluidasGeral.Should().BeApproximately((double)(3 + 2) / 2, 0.001);

            _mockProjetoRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetConcluidasPorUsuarioDesdeAsync(gerenteId, It.IsAny<DateTime>()), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetConcluidasPorUsuarioDesdeAsync(usuarioNormalId, It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task DeveRetornarRelatorioVazioSeNaoHouverUsuariosOuTarefas()
        {
            // Arrange
            var diasRetroativos = 30;

            _mockProjetoRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<DomainProjeto>()); // Use DomainProjeto
            _mockTarefaRepository.Setup(repo => repo.GetConcluidasPorUsuarioDesdeAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                                 .ReturnsAsync(new List<DomainTarefa>()); // Use DomainTarefa

            // Act
            var resultado = await _gerarRelatorioDesempenhoUseCase.ExecutarAsync(diasRetroativos, "Normal User");

            // Assert
            resultado.Should().NotBeNull();
            resultado.DesempenhoPorUsuario.Should().BeEmpty();
            resultado.NumeroMedioTarefasConcluidasGeral.Should().Be(0);
            resultado.DataGeracao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            resultado.PeriodoRelatorio.Should().Be($"Últimos {diasRetroativos} dias.");

            _mockProjetoRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.GetConcluidasPorUsuarioDesdeAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}