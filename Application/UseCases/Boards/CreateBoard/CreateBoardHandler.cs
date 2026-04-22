using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.CreateBoard
{
    public class CreateBoardHandler(IBoardRepository boardRepository)
    {
        public record CreateBoardCommand(string Name, string Description);
        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task<Guid> Handle(CreateBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = new Board(command.Name, command.Description);
            await _boardRepository.AddAsync(board, cancellationToken);

            return board.Id; 
        }
    }
}