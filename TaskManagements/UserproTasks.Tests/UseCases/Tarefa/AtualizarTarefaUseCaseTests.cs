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
         
    }
}