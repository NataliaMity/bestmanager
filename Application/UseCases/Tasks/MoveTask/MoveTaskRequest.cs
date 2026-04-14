namespace Application.UseCases.Tasks.MoveTask
{
    public record MoveTaskRequest
    (
        Guid TaskId,
        Guid ColumnId);
}