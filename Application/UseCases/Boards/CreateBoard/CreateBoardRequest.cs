namespace Application.UseCases.Boards.CreateBoard
{
    public record CreateBoardRequest (
        string Name,
        string Description);
}