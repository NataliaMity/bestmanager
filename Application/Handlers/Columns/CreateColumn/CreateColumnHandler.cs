using Application.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Handlers.Columns.CreateColumn
{
    public class CreateColumnHandler(IBoardRepository boardRepository,
                                     IColumnRepository columnRepository,
                                     IUnitOfWork unitOfWork)
    {
        public record CreateColumnCommand(Guid BoardId, string Name);

        public async Task<Guid> Handle(CreateColumnCommand command, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIdAsync(command.BoardId, cancellationToken)
                ?? throw new NotFoundException("Доска", command.BoardId);

            // Новая колонка встаёт в конец доски
            var existing = await columnRepository.GetByBoardAsync(board.Id, cancellationToken: cancellationToken);
            var column = new Column(board.Id, command.Name, existing.Count);

            columnRepository.Add(column);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return column.Id;
        }
    }
}
