using Domain.Interfaces;

namespace Application.Handlers.Boards.GetBoards
{
    public class GetBoardsHandler(IBoardRepository boardRepository)
    {
        public async Task<List<BoardDto>> Handle(CancellationToken cancellationToken = default)
        {
            var boards = await boardRepository.GetAllAsync(cancellationToken);
            return boards.Select(BoardDto.From).ToList();
        }
    }
}
