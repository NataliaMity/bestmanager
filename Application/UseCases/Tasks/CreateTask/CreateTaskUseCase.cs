using Domain.Interfaces;
using Task = Domain.Entities.Task;

namespace Application.UseCases.Tasks.CreateTask
{
    public class CreateTaskUseCase
    {
        private readonly IColumnRepository columnRepository;
        private readonly ITaskRepository taskRepository;

        public CreateTaskUseCase(
        IColumnRepository columnRepository,
        ITaskRepository taskRepository)
        {
            this.columnRepository = columnRepository;
            this.taskRepository = taskRepository;
        }


        public async Task<CreateTaskResponse?> ExecuteAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(request.ColumnId, cancellationToken);
            if (column == null)
                throw new Exception($"Колонка c id {request.ColumnId} не была найдена");

            var task = new Task(request.Name, request.Description, column, request.Order);

            await taskRepository.AddAsync(task, cancellationToken);

            return new CreateTaskResponse(task.ID, task.Name, column.Id);
            
        }
    }
}
