using Domain.Entities;

namespace Application.Handlers.Columns
{
    public record ColumnDto(Guid Id, Guid BoardId, string Name, int Order, DateTime Created, DateTime Updated)
    {
        public static ColumnDto From(Column column) =>
            new(column.Id, column.BoardId, column.Name, column.Order, column.Created, column.Updated);
    }
}
