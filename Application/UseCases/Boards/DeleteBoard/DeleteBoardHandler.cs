using Domain.Interfaces;

namespace Application.UseCases.Boards.DeleteBoard
{
    public class DeleteBoardHandler(IBoardRepository boardRepository)
    {
        public record DeleteBoardCommand(Guid BoardId);
        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task Handle(DeleteBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);
            if (board == null)
                throw new InvalidOperationException($"Колонка с id {command.BoardId} не найдена");

            await _boardRepository.RemoveAsync(board, cancellationToken);
        }
    }
}