using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Columns.CreateColumn
{
    public class CreateColumnHandler(IColumnRepository columnRepository,
                                     IBoardRepository boardRepository)
    {
        public record CreateColumnCommand(Guid BoardId, string Name);

        private readonly IColumnRepository _columnRepository = columnRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task<Guid> Handle(CreateColumnCommand command, CancellationToken cancellationToken = default)
        {
            var board = await _boardRepository.GetByIdAsync(command.BoardId, cancellationToken);
            if (board == null)
                throw new InvalidOperationException($"Доска с id {command.BoardId} не найдена");

            var column = new Column(command.Name, board);
            await _columnRepository.AddAsync(column, cancellationToken);

            return column.Id; 
        }
    }
}