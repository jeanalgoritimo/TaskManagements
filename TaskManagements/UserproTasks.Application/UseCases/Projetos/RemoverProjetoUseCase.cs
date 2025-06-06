using UserProTasks.Application.Interfaces;

namespace UserProTasks.Application.UseCases.Projetos
{
    public class RemoverProjetoUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public RemoverProjetoUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> ExecutarAsync(Guid projetoId)
        {
            var projeto = await _projetoRepository.GetByIdWithTasksAsync(projetoId); // Carregar tarefas para a regra de negócio

            if (projeto == null)
            {
                return (false, "Projeto não encontrado.");
            }

            if (!projeto.PodeSerRemovido()) // Aplica a regra de negócio
            {
                return (false, "Não é possível remover o projeto, existem tarefas pendentes. Conclua ou remova-as primeiro.");
            }

            await _projetoRepository.DeleteAsync(projeto);
            await _projetoRepository.SaveChangesAsync();

            return (true, null);
        }
    }
}