using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.GetBoard
{
    public class GetBoardHandler(IBoardRepository boardRepository)
    {
        public record GetBoardCommand(Guid BoardId);
        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task<Board> Handle(GetBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);
            
            return board ?? throw new InvalidOperationException($"Доска с id {command.BoardId} не найдена");
        }
    }
}
