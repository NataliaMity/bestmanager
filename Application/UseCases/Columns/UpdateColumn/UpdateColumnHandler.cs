using Domain.Interfaces;

namespace Application.UseCases.Columns.UpdateColumn
{
    public class UpdateColumnHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        public record UpdateColumnCommand(string? Name, Guid ColumnId);

        private readonly IColumnRepository columnRepository = columnRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateColumnCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken) 
                ?? throw new Exception($"Не удалось найти задачу с Id {command.ColumnId}");

            if (command.Name is not null)
                column.Rename(command.Name);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}