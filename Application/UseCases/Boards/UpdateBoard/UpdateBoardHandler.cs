using Domain.Interfaces;

namespace Application.UseCases.Boards.UpdateBoard
{
    public class UpdateBoardHandler(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        public record UpdateBoardCommand(Guid BoardId, string? Name, string? Description);
        private readonly IBoardRepository _boardRepository = boardRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateBoardCommand command, CancellationToken cancellationToken = default)
        {
            var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken) 
                ?? throw new InvalidOperationException($"Не удалось найти доску с Id {command.BoardId}");

            if (command.Name is not null)
                board.Rename(command.Name);

            if (command.Description is not null)
                board.ChangeDescription(command.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}