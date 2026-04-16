using Domain.Interfaces;

namespace Application.UseCases.Tasks.GetTasksByColumn
{
    public class GetTaskByColumnUseCase(ITaskRepository taskRepository)
    {
        private readonly ITaskRepository taskRepository = taskRepository;

        public async Task<GetTaskByColumnResponse?> ExecuteAsync(GetTaskByColumnRequest request, CancellationToken cancellationToken = default)
        {
            var tasks = await taskRepository.GetByColumnIdAsync(request.ColumnId, cancellationToken);
            
            return tasks == null
                ? throw new Exception($"Колонка с id {request.ColumnId} не найдена")
                : new GetTaskByColumnResponse(tasks);
        }
    }
}
