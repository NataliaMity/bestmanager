using Application.UseCases.Columns.CreateColumn;
using Application.UseCases.Columns.DeleteColumn;
using Application.UseCases.Columns.GetColumnsByBoard;
using Application.UseCases.Columns.ReorderColumn;
using Application.UseCases.Columns.UpdateColumn;

namespace Web.Endpoints
{
    public static class ColumnEndpoints
    {
        public static void MapColumnsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/columns");

            group.MapGet("/{boardId}", GetColumnsByBoardId);
            group.MapPost("/", CreateColumn);
            group.MapPut("/update/{id}", UpdateColumn);
            group.MapDelete("/delete/{id}", DeleteColumn);
            group.MapPut("/reorder/{id}", ReorderColumn);
        }

        private static async Task<IResult> ReorderColumn(
            ReorderColumnHandler.ReorderColumnCommand command,
            ReorderColumnHandler handler,
            CancellationToken cancellationToken
            )
        {
            await handler.Handle(command, cancellationToken);
            return Results.Ok();
        }

        private static async Task<IResult> DeleteColumn(
            Guid id,
            DeleteColumnHandler handler,
            CancellationToken cancellationToken
            )
        {
            await handler.Handle(new DeleteColumnHandler.DeleteColumnCommand(id), cancellationToken);
            return Results.NoContent();
        }

        private static async Task<IResult> UpdateColumn(
            Guid id,
            UpdateColumnHandler.UpdateColumnCommand command,
            UpdateColumnHandler handler,
            CancellationToken cancellationToken
            )
        {
            await handler.Handle(command with { ColumnId = id}, cancellationToken);
            return Results.Ok();
        }

        private static async Task<IResult> CreateColumn(
            CreateColumnHandler.CreateColumnCommand command,
            CreateColumnHandler handler,
            CancellationToken cancellationToken
            )
        {
            var columnId = await handler.Handle(command, cancellationToken);
            return Results.Created($"/api/columns/{columnId}", new { id = columnId });
        }

        private static async Task<IResult> GetColumnsByBoardId(
            Guid boardId,
            GetColumnsByBoardHandler handler,
            CancellationToken cancellationToken)
        {
            var columns = handler.Handle(new GetColumnsByBoardHandler.GetColumnsByBoardCommand(boardId), cancellationToken);
            return Results.Ok(columns);
        }
    }
}