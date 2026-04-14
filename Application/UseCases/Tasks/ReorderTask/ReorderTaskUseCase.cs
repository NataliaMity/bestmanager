using Domain.Interfaces;

namespace Application.UseCases.Tasks.ReorderTask
{
    public class ReorderTaskUseCase
    {
        private readonly ITaskRepository taskRepository;
        public ReorderTaskUseCase(ITaskRepository taskRepository) 
        {
            this.taskRepository = taskRepository;
        }

        public async Task ExecuteAsync(ReorderTaskRequest request, CancellationToken cancellationToken = default)
        {
            List<Domain.Entities.Task>? tasks = await taskRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);
            if (tasks == null || tasks.Count == 0)
                throw new Exception($"Не удалось найти задачи в колонке с Id {request.ColumnId}");

            var task = tasks.Find(task => task.ID == request.TaskId);
            if (task == null)
                throw new Exception($"Не удалось найти задачу с Id {request.TaskId}");

            tasks.Remove(task);
            tasks.Insert(request.Index, task);
        }
    }
}