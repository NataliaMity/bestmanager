namespace Application.UseCases.Columns.ReorderColumn
{
    public record ReorderColumnRequest
    (
        Guid BoardId,
        Guid ColumnId,
        int Index);
}