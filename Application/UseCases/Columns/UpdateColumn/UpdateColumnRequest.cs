namespace Application.UseCases.Columns.UpdateColumn
{
    public record UpdateColumnRequest
    (
        string? Name,
        Guid ColumnId
        );
}