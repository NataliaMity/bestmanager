namespace Application.UseCases.Boards.UpdateBoard
{
    public record UpdateBoardRequest
    (
        string? Name,
        Guid BoardId
        );
}