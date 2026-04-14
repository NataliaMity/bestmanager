namespace Application.UseCases.Tasks.CreateTask
{
    public record CreateTaskResponse(
        Guid TaskId,
        Guid ColumnId
    );
}