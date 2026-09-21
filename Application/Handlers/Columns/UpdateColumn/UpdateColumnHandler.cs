using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Columns.UpdateColumn
{
    public class UpdateColumnHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        /// <summary>null в поле — не менять его.</summary>
        public record UpdateColumnCommand(Guid ColumnId, string? Name);

        public async Task Handle(UpdateColumnCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
                ?? throw new NotFoundException("Колонка", command.ColumnId);

            if (command.Name is not null)
                column.Rename(command.Name);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
