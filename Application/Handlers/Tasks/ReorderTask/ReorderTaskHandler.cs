using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.ReorderTask
{
    public class ReorderTaskHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        /// <param name="Index">Новая позиция задачи в её колонке (с 0).</param>
        public record ReorderTaskCommand(Guid TaskId, int Index);

        public async Task Handle(ReorderTaskCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByTaskIdAsync(command.TaskId, cancellationToken)
                ?? throw new NotFoundException("Задача", command.TaskId);

            column.ReorderTask(command.TaskId, command.Index);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
