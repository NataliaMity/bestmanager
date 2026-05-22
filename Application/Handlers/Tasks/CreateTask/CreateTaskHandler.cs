using Domain.Interfaces;
using Task = Domain.Entities.Task;

namespace Application.UseCases.Tasks.CreateTask
{
    public class CreateTaskHandler
    {
        public record CreateTaskCommand(string Name, string Description, Guid ColumnId, int Order);

        public record CreateTaskResult(Guid Id, string Name, Guid ColumnId);

        private readonly IColumnRepository _columnRepository;
        private readonly ITaskRepository _taskRepository;

        public CreateTaskHandler(IColumnRepository columnRepository, ITaskRepository taskRepository)
        {
            _columnRepository = columnRepository;
            _taskRepository = taskRepository;
        }

        public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken ct = default)
        {
            var column = await _columnRepository.GetByIdAsync(command.ColumnId, ct)
                ?? throw new InvalidOperationException($"Column {command.ColumnId} not found");

            var task = new Task(command.Name, command.Description, column, command.Order);

            await _taskRepository.AddAsync(task, ct);

            return new CreateTaskResult(task.Id, task.Name, task.ColumnId);
        }
    }
}