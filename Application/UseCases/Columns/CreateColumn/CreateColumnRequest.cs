namespace Application.UseCases.Columns.CreateColumn
{
    public record CreateColumnRequest (
        Guid BoardId,
        string Name);
}