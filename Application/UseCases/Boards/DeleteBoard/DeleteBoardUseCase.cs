using Domain.Interfaces;

namespace Application.UseCases.Boards.DeleteBoard
{
    public class DeleteBoardUseCase(IBoardRepository boardRepository)
    {
        private readonly IBoardRepository boardRepository = boardRepository;

        public async Task ExecuteAsync(DeleteBoardRequest request, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIDAsync(request.BoardId, cancellationToken);
            if (board == null)
                throw new Exception($"Колонка с id {request.BoardId} не найдена");

            await boardRepository.DeleteAsync(request.BoardId, cancellationToken);
        }
    }
}