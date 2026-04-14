using Domain.Interfaces;

namespace Application.UseCases.Tasks.DeleteTask
{
    public class DeleteTaskUseCase
    {
        private readonly ITaskRepository taskRepository;

        public DeleteTaskUseCase(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task ExecuteAsync(DeleteTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new Exception($"Задача с id {request.TaskId} не найдена");

            await taskRepository.DeleteAsync(request.TaskId, cancellationToken);
        }
    }
}
