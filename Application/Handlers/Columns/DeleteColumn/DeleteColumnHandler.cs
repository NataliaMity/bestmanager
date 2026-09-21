using Application.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Handlers.Columns.DeleteColumn
{
    public class DeleteColumnHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        public record DeleteColumnCommand(Guid ColumnId);

        public async Task Handle(DeleteColumnCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
                ?? throw new NotFoundException("Колонка", command.ColumnId);

            columnRepository.Remove(column);

            // Закрываем «дыру» в порядке оставшихся колонок
            var boardColumns = await columnRepository.GetByBoardAsync(column.BoardId, cancellationToken: cancellationToken);
            Column.RenumberColumns(boardColumns.Where(c => c.Id != column.Id));

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
