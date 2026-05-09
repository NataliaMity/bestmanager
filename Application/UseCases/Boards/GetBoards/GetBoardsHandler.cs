using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.GetBoard
{
    public class GetBoardsHandler(IBoardRepository boardRepository)
    {
        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task<List<Board>?> Handle(CancellationToken cancellationToken = default)
        {
            var boards = await _boardRepository.GetAsync(cancellationToken);
            
            return boards ?? throw new InvalidOperationException("Доски не найдены");
        }
    }
}