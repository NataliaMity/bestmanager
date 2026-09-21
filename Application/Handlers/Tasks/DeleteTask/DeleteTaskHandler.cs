using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.DeleteTask
{
    public class DeleteTaskHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        public record DeleteTaskCommand(Guid TaskId);

        public async Task Handle(DeleteTaskCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByTaskIdAsync(command.TaskId, cancellationToken)
                ?? throw new NotFoundException("Задача", command.TaskId);

            column.RemoveTask(command.TaskId);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
