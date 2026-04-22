using Domain.Interfaces;

namespace Application.UseCases.Tasks.ReorderTask
{
    public class ReorderTaskHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        public record ReorderTaskCommand(Guid TaskId, Guid ColumnId, int Index);

        private readonly IColumnRepository _columnRepository = columnRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(ReorderTaskCommand command, CancellationToken cancellationToken = default)
        {
            var column = await _columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
            ?? throw new InvalidOperationException($"Колонка {command.ColumnId} не найдена");

            column.ReorderTask(command.TaskId, command.Index);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}