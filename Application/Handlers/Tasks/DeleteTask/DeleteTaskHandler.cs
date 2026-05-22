using Domain.Interfaces;

namespace Application.UseCases.Tasks.DeleteTask
{
    public class DeleteTaskHandler(ITaskRepository taskRepository)
    {
        public record DeleteTaskCommand(Guid TaskId);

        private readonly ITaskRepository taskRepository = taskRepository;

        public async Task Handle(DeleteTaskCommand command, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(command.TaskId, cancellationToken) 
                ?? throw new InvalidOperationException($"Задача с id {command.TaskId} не найдена");

            await taskRepository.RemoveAsync(task, cancellationToken);
        }
    }
}
