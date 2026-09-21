using Domain.Entities;

namespace Application.Handlers.Tasks
{
    public record TaskDto(
        Guid Id,
        Guid ColumnId,
        string Name,
        string Description,
        DateTime? Deadline,
        int Order,
        DateTime Created,
        DateTime Updated)
    {
        public static TaskDto From(TaskItem task) =>
            new(task.Id, task.ColumnId, task.Name, task.Description, task.Deadline,
                task.Order, task.Created, task.Updated);
    }
}
