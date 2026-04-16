using Domain.Interfaces;

namespace Application.UseCases.Boards.UpdateBoard
{
    public class UpdateBoardUseCase(IBoardRepository boardRepository)
    {
        private readonly IBoardRepository boardRepository = boardRepository;

        public async Task ExecuteAsync(UpdateBoardRequest request, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIDAsync(request.BoardId, cancellationToken) 
                ?? throw new Exception($"Не удалось найти доску с Id {request.BoardId}");

            if (request.Name is not null)
                board.UpdateName(request.Name);

            await boardRepository.UpdateAsync(board.Id, cancellationToken);
        }
    }
}