namespace Application.UseCases.Tasks.ReorderTask
{
    public record ReorderTaskRequest
    (
        Guid TaskId,
        Guid ColumnId,
        int Index
    );
}