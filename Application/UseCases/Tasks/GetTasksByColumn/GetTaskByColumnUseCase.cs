using Domain.Interfaces;

namespace Application.UseCases.Tasks.GetTasksByColumn
{
    public class GetTaskByColumnUseCase
    {
        private readonly ITaskRepository taskRepository;
        public GetTaskByColumnUseCase(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<GetTaskByColumnResponse?> ExecuteAsync(GetTaskByColumnRequest request, CancellationToken cancellationToken = default)
        {
            var tasks = await taskRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);
            
            return tasks == null
                ? throw new Exception($"Колонка с названием {request.ColumnId} не найдена")
                : new GetTaskByColumnResponse(tasks);
        }
    }
}
