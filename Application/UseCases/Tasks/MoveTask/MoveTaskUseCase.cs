using Domain.Interfaces;

namespace Application.UseCases.Tasks.MoveTask
{
    public class MoveTaskUseCase
    {
        private readonly ITaskRepository taskRepository;
        private readonly IColumnRepository columnRepository;
        public MoveTaskUseCase(ITaskRepository taskRepository,
                                IColumnRepository columnRepository) 
        { 
            this.taskRepository = taskRepository;
            this.columnRepository = columnRepository;
        }

        public async Task ExecuteAsync(MoveTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
                throw new Exception($"Не удалось найти задачу с Id {request.TaskId}");

            var column = await columnRepository.GetByIDAsync(request.ColumnId, cancellationToken);
            if (column == null)
                throw new Exception($"Не удалось найти колонку с Id {request.ColumnId}");
            
            task.MoveTo(column);
            
            await taskRepository.UpdateAsync(task, cancellationToken);
        }
    }
}