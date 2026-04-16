namespace Application.UseCases.Tasks.CreateTask
{
    public record CreateTaskResponse(
        Guid TaskId,
        string TaskName,
        Guid ColumnId
    );
}