using Application.Handlers.Tasks;
using Domain.Entities;

namespace Application.Handlers.Boards
{
    public record BoardDto(Guid Id, string Name, string Description, DateTime Created, DateTime Updated)
    {
        public static BoardDto From(Board board) =>
            new(board.Id, board.Name, board.Description, board.Created, board.Updated);
    }

    /// <summary>Доска целиком: колонки по порядку, в каждой задачи по порядку.</summary>
    public record BoardDetailsDto(
        Guid Id,
        string Name,
        string Description,
        DateTime Created,
        DateTime Updated,
        IReadOnlyList<BoardColumnDto> Columns)
    {
        public static BoardDetailsDto From(Board board, IEnumerable<Column> columns) =>
            new(board.Id, board.Name, board.Description, board.Created, board.Updated,
                columns.OrderBy(c => c.Order).Select(BoardColumnDto.From).ToList());
    }

    public record BoardColumnDto(Guid Id, string Name, int Order, IReadOnlyList<TaskDto> Tasks)
    {
        public static BoardColumnDto From(Column column) =>
            new(column.Id, column.Name, column.Order,
                column.Tasks.OrderBy(t => t.Order).Select(TaskDto.From).ToList());
    }
}
