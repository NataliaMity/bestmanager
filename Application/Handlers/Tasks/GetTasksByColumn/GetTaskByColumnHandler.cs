using Domain.Interfaces;

namespace Application.UseCases.Tasks.GetTasksByColumn
{
    public class GetTaskByColumnHandler(ITaskRepository taskRepository)
    {
        public record GetTaskByColumnCommand(Guid ColumnId);

        private readonly ITaskRepository taskRepository = taskRepository;

        public async Task<List<Domain.Entities.Task>> Handle(GetTaskByColumnCommand command, CancellationToken cancellationToken = default)
        {
            var tasks = await taskRepository.GetByColumnIdAsync(command.ColumnId, cancellationToken);
            return tasks ?? throw new InvalidOperationException($"Колонка с id {command.ColumnId} не найдена");
        }
    }
}