using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Boards.UpdateBoard
{
    public class UpdateBoardHandler(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        /// <summary>null в поле — не менять его.</summary>
        public record UpdateBoardCommand(Guid BoardId, string? Name, string? Description);

        public async Task Handle(UpdateBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIdAsync(command.BoardId, cancellationToken)
                ?? throw new NotFoundException("Доска", command.BoardId);

            if (command.Name is not null)
                board.Rename(command.Name);

            if (command.Description is not null)
                board.ChangeDescription(command.Description);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
