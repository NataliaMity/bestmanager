using Domain.Interfaces;

namespace Application.UseCases.Tasks.MoveTask
{
    public class MoveTaskHandler(ITaskRepository taskRepository,
                                    IColumnRepository columnRepository,
                                    IUnitOfWork unitOfWork)
    {
        public record MoveTaskCommand(Guid TaskId, Guid ColumnId);

        private readonly ITaskRepository taskRepository = taskRepository;
        private readonly IColumnRepository columnRepository = columnRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(MoveTaskCommand command, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(command.TaskId, cancellationToken);
            if (task == null)
                throw new InvalidOperationException($"Не удалось найти задачу с Id {command.TaskId}");

            var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken);
            if (column == null)
                throw new InvalidOperationException($"Не удалось найти колонку с Id {command.ColumnId}");

            task.SetColumn(column, 0);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}