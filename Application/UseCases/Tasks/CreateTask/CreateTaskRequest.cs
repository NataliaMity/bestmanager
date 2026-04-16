
namespace Application.UseCases.Tasks.CreateTask
{
    public record CreateTaskRequest(
        string Name,
        string Description,
        Guid ColumnId,
        int Order
    );
}
