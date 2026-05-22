using Domain.Interfaces;

namespace Application.UseCases.Tasks.UpdateTask
{
    public class UpdateTaskHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        public record UpdateTaskCommand(string? Name, string? Description, Guid TaskId);

        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateTaskCommand command, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(command.TaskId, cancellationToken);
            if(task == null)
                throw new InvalidOperationException($"Не удалось найти задачу с Id {command.TaskId}");

            if (command.Name is not null)
                task.Rename(command.Name);

            if (command.Description is not null)
                task.ChangeDescription(command.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}