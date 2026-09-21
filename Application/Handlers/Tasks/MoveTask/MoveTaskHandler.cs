using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.MoveTask
{
    public class MoveTaskHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        /// <param name="Index">Позиция в целевой колонке (с 0); null — в конец.</param>
        public record MoveTaskCommand(Guid TaskId, Guid ColumnId, int? Index);

        public async Task Handle(MoveTaskCommand command, CancellationToken cancellationToken = default)
        {
            var source = await columnRepository.GetByTaskIdAsync(command.TaskId, cancellationToken)
                ?? throw new NotFoundException("Задача", command.TaskId);

            var target = source.Id == command.ColumnId
                ? source
                : await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
                    ?? throw new NotFoundException("Колонка", command.ColumnId);

            source.MoveTaskTo(command.TaskId, target, command.Index);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
