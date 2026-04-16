namespace Application.UseCases.Columns.GetColumnsByBoard
{
    public record GetColumnsByBoardResponse(
        List<Domain.Entities.Column> Columns);
}