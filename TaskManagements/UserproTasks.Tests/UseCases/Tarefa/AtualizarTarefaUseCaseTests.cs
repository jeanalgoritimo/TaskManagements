using Xunit;
using Moq;
using FluentAssertions;
using UserProTasks.Application.Interfaces;
using UserProTasks.Application.UseCases.Tarefas; // Ajustado para o namespace correto (plural)
using TaskManager.Domain.Entities; // Para Tarefa e HistoricoTarefa
using TaskManager.Domain.Enums; // Para StatusTarefa e PrioridadeTarefa
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UserProTasks.Application.DTOs;

namespace UserProTasks.Tests.UseCases.Tarefa
{
    public class AtualizarTarefaUseCaseTests
    {
        private readonly Mock<ITarefaRepository> _mockTarefaRepository;
        private readonly AtualizarTarefaUseCase _atualizarTarefaUseCase;

        public AtualizarTarefaUseCaseTests()
        {
            _mockTarefaRepository = new Mock<ITarefaRepository>();
            _atualizarTarefaUseCase = new AtualizarTarefaUseCase(
                _mockTarefaRepository.Object
            );
        }

        [Fact]
        public async Task DeveAtualizarStatusEDetalhesDeTarefaComSucessoERegistrarHistorico()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var usuarioAtualizacaoId = Guid.NewGuid();
            var nomeUsuarioAtualizacao = "Updater";
            var tarefaExistente = new TaskManager.Domain.Entities.Tarefa(
                "Tarefa Antiga", "Desc Antiga", DateTime.Today.AddDays(5),
                StatusTarefa.Pendente, PrioridadeTarefa.Media, Guid.NewGuid(), Guid.NewGuid(), "Criador");

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaId)).ReturnsAsync(tarefaExistente);
            _mockTarefaRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>())).Returns(Task.CompletedTask);
            _mockTarefaRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var novoTitulo = "Novo Título";
            var novaDescricao = "Nova Descrição";
            var novaDataVencimento = DateTime.Today.AddDays(10);
            var novoStatus = StatusTarefa.EmAndamento;
            // Prioridade deve ser a mesma para não tentar alterar
            var prioridadeManter = tarefaExistente.Prioridade;

            AtualizarTarefaDto dto = new AtualizarTarefaDto()
            {
                Titulo = novoTitulo,
                Descricao = novaDescricao,
                DataVencimento = novaDataVencimento,
                Status = novoStatus
            };

            // Act
            var tarefaAtualizada = await _atualizarTarefaUseCase.ExecutarAsync(
                tarefaId, dto, nomeUsuarioAtualizacao);
             
            // Assert
            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.UpdateAsync(It.Is<TaskManager.Domain.Entities.Tarefa>(t =>
                t.Id == tarefaId &&
                t.Titulo == novoTitulo &&
                t.Descricao == novaDescricao &&
                t.DataVencimento == novaDataVencimento &&
                t.Status == novoStatus &&
                t.Prioridade == prioridadeManter // Prioridade deve permanecer a mesma
            )), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task NaoDevePermitirAlterarAPrioridadeDeUmaTarefaExistente()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var usuarioAtualizacaoId = Guid.NewGuid();
            var nomeUsuarioAtualizacao = "Updater";
            var tarefaExistente = new TaskManager.Domain.Entities.Tarefa(
                "Tarefa Com Prioridade", "Desc", DateTime.Today.AddDays(5),
                StatusTarefa.Pendente, PrioridadeTarefa.Media, Guid.NewGuid(), Guid.NewGuid(), "Criador");

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaId)).ReturnsAsync(tarefaExistente);

            var novaPrioridade = PrioridadeTarefa.Alta; // Diferente da original (Media)

            AtualizarTarefaDto dto = new AtualizarTarefaDto()
            {
                Titulo = tarefaExistente.Titulo,
                Descricao = tarefaExistente.Descricao,
                DataVencimento = tarefaExistente.DataVencimento,
                Status = tarefaExistente.Status,
                Prioridade= novaPrioridade
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => // Ou BusinessRuleException
                _atualizarTarefaUseCase.ExecutarAsync(
                    tarefaId,dto,nomeUsuarioAtualizacao)
            );

            exception.Message.Should().Contain("Não é permitido alterar a prioridade de uma tarefa.");

            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaId), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.UpdateAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeTarefaNaoEncontradaAoAtualizar()
        {
            // Arrange
            var tarefaIdInexistente = Guid.NewGuid();
            var usuarioAtualizacaoId = Guid.NewGuid();


            AtualizarTarefaDto dto = new AtualizarTarefaDto()
            {
                Titulo = "Titulo",
                Descricao = "Desc",
                DataVencimento = DateTime.Today,
                Status = StatusTarefa.Pendente,
                Prioridade = PrioridadeTarefa.Baixa
            };

            _mockTarefaRepository.Setup(repo => repo.GetByIdAsync(tarefaIdInexistente)).ReturnsAsync((TaskManager.Domain.Entities.Tarefa)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => // Ou NotFoundException
                _atualizarTarefaUseCase.ExecutarAsync(
                    tarefaIdInexistente, dto, "User")
            );

            exception.Message.Should().Contain("Tarefa não encontrada.");

            _mockTarefaRepository.Verify(repo => repo.GetByIdAsync(tarefaIdInexistente), Times.Once);
            _mockTarefaRepository.Verify(repo => repo.UpdateAsync(It.IsAny<TaskManager.Domain.Entities.Tarefa>()), Times.Never);
            _mockTarefaRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}