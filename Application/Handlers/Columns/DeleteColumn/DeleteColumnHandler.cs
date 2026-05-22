using Domain.Interfaces;

namespace Application.UseCases.Columns.DeleteColumn
{
    public class DeleteColumnHandler(IColumnRepository columnRepository)
    {
        public record DeleteColumnCommand(Guid ColumnId);

        private readonly IColumnRepository _columnRepository = columnRepository;

        public async Task Handle(DeleteColumnCommand command, CancellationToken cancellationToken = default)
        {
            var column = await _columnRepository.GetByIdAsync(command.ColumnId, cancellationToken);
            if (column == null)
                throw new InvalidOperationException($"Колонка с id {command.ColumnId} не найдена");

            await _columnRepository.RemoveAsync(column, cancellationToken);
        }
    }
}