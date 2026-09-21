using Domain.Entities;
using Domain.Interfaces;

namespace Application.Handlers.Boards.CreateBoard
{
    public class CreateBoardHandler(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        public record CreateBoardCommand(string Name, string? Description);

        public async Task<Guid> Handle(CreateBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = new Board(command.Name, command.Description);

            boardRepository.Add(board);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return board.Id;
        }
    }
}
