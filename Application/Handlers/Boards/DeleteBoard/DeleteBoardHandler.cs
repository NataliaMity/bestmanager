using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Boards.DeleteBoard
{
    public class DeleteBoardHandler(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        public record DeleteBoardCommand(Guid BoardId);

        public async Task Handle(DeleteBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIdAsync(command.BoardId, cancellationToken)
                ?? throw new NotFoundException("Доска", command.BoardId);

            boardRepository.Remove(board);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
