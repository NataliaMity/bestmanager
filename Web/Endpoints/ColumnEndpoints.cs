using Application.Handlers.Columns;
using Application.Handlers.Columns.CreateColumn;
using Application.Handlers.Columns.DeleteColumn;
using Application.Handlers.Columns.GetColumnsByBoard;
using Application.Handlers.Columns.ReorderColumn;
using Application.Handlers.Columns.UpdateColumn;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Endpoints
{
    public static class ColumnEndpoints
    {
        public record UpdateColumnRequest(string? Name);

        public static IEndpointRouteBuilder MapColumnEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/columns").WithTags("Columns");

            group.MapGet("/{boardId:guid}", GetColumnsByBoardId);
            group.MapPost("/", CreateColumn);
            group.MapPut("/update/{id:guid}", UpdateColumn);
            group.MapDelete("/delete/{id:guid}", DeleteColumn);
            group.MapPut("/reorder/{id:guid}", ReorderColumn);

            return app;
        }

        private static async Task<Ok<List<ColumnDto>>> GetColumnsByBoardId(
            Guid boardId,
            GetColumnsByBoardHandler handler,
            CancellationToken cancellationToken)
        {
            var columns = await handler.Handle(new GetColumnsByBoardHandler.GetColumnsByBoardQuery(boardId), cancellationToken);
            return TypedResults.Ok(columns);
        }

        /// <summary>Колонка добавляется в конец доски.</summary>
        private static async Task<Created<IdResponse>> CreateColumn(
            CreateColumnHandler.CreateColumnCommand command,
            CreateColumnHandler handler,
            CancellationToken cancellationToken)
        {
            var columnId = await handler.Handle(command, cancellationToken);
            return TypedResults.Created($"/api/columns/{command.BoardId}", new IdResponse(columnId));
        }

        private static async Task<NoContent> UpdateColumn(
            Guid id,
            UpdateColumnRequest request,
            UpdateColumnHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new UpdateColumnHandler.UpdateColumnCommand(id, request.Name), cancellationToken);
            return TypedResults.NoContent();
        }

        private static async Task<NoContent> DeleteColumn(
            Guid id,
            DeleteColumnHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new DeleteColumnHandler.DeleteColumnCommand(id), cancellationToken);
            return TypedResults.NoContent();
        }

        private static async Task<NoContent> ReorderColumn(
            Guid id,
            ReorderRequest request,
            ReorderColumnHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new ReorderColumnHandler.ReorderColumnCommand(id, request.Index), cancellationToken);
            return TypedResults.NoContent();
        }
    }
}
