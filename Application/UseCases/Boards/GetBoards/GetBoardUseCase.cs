using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.GetBoard
{
    public class GetBoardUseCase(IBoardRepository boardRepository)
    {
        private readonly IBoardRepository boardRepository = boardRepository;

        public async Task<GetBoardResponse?> ExecuteAsync(GetBoardRequest request, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIDAsync(request.BoardId, cancellationToken);
            
            return board == null
                ? throw new Exception($"Доска с id {request.BoardId} не найдена")
                : new GetBoardResponse(board);
        }
    }
}
