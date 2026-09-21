using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.UpdateTask
{
    public class UpdateTaskHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        /// <summary>null в поле — не менять его. Чтобы снять дедлайн, передайте RemoveDeadline = true.</summary>
        public record UpdateTaskCommand(
            Guid TaskId,
            string? Name,
            string? Description,
            DateTime? Deadline,
            bool RemoveDeadline = false);

        public async Task Handle(UpdateTaskCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByTaskIdAsync(command.TaskId, cancellationToken)
                ?? throw new NotFoundException("Задача", command.TaskId);
            var task = column.GetTask(command.TaskId);

            if (command.Name is not null)
                task.Rename(command.Name);

            if (command.Description is not null)
                task.ChangeDescription(command.Description);

            if (command.RemoveDeadline)
                task.SetDeadline(null);
            else if (command.Deadline is not null)
                task.SetDeadline(command.Deadline);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
