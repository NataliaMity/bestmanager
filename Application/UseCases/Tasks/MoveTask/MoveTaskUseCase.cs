using Domain.Interfaces;

namespace Application.UseCases.Tasks.MoveTask
{
    public class MoveTaskUseCase(ITaskRepository taskRepository,
                            IColumnRepository columnRepository)
    {
        private readonly ITaskRepository taskRepository = taskRepository;
        private readonly IColumnRepository columnRepository = columnRepository;

        public async Task ExecuteAsync(MoveTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
                throw new Exception($"Не удалось найти задачу с Id {request.TaskId}");

            var column = await columnRepository.GetByIdAsync(request.ColumnId, cancellationToken);
            if (column == null)
                throw new Exception($"Не удалось найти колонку с Id {request.ColumnId}");
            
            task.MoveTo(column);
            
            await taskRepository.UpdateAsync(task, cancellationToken);
        }
    }
}