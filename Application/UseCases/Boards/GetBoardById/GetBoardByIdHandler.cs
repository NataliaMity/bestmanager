using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.GetBoardById
{
    public class GetBoardByIdHandler(IBoardRepository boardRepository)
    {
        public record GetBoardByIdCommand(Guid BoardId);

        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task<Board?> Handle(GetBoardByIdCommand command, CancellationToken cancellationToken = default)
        {
            var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);

            return board ?? throw new InvalidOperationException("Доска не найдена");
        }
    }
}