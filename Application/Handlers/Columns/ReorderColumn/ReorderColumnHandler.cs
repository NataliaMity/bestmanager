using Application.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Handlers.Columns.ReorderColumn
{
    public class ReorderColumnHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        /// <param name="Index">Новая позиция колонки на доске (с 0).</param>
        public record ReorderColumnCommand(Guid ColumnId, int Index);

        public async Task Handle(ReorderColumnCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
                ?? throw new NotFoundException("Колонка", command.ColumnId);

            var boardColumns = await columnRepository.GetByBoardAsync(column.BoardId, cancellationToken: cancellationToken);
            Column.Reorder(boardColumns, column.Id, command.Index);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
