using Domain.Interfaces;

namespace Application.UseCases.Tasks.UpdateTask
{
    public class UpdateTaskUseCase
    {
        private readonly ITaskRepository taskRepository;

        public UpdateTaskUseCase(ITaskRepository taskRepository) 
        {
            this.taskRepository = taskRepository;
        }

        public async Task ExecuteAsync(UpdateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            if(task == null)
                throw new Exception($"Не удалось найти задачу с Id {request.TaskId}");

            if (request.Name is not null)
                task.UpdateName(request.Name);

            if (request.Description is not null)
                task.UpdateDescription(request.Description);

            await taskRepository.UpdateAsync(task, cancellationToken);
        }
    }
}