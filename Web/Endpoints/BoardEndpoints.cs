using Application.UseCases.Boards.CreateBoard;
using Application.UseCases.Boards.DeleteBoard;
using Application.UseCases.Boards.GetBoard;
using Application.UseCases.Boards.GetBoardById;
using Application.UseCases.Boards.UpdateBoard;

namespace Web.Endpoints
{
    public static class BoardEndpoints
    {
        public static void MapBoardsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/boards");

            group.MapGet("/", GetBoards);
            group.MapGet("/{id}", GetBoard);
            group.MapPost("/", CreateBoard);
            group.MapPut("/{id}", UpdateBoard);
            group.MapDelete("/{id}", DeleteBoard);
        }

        private static async Task<IResult> DeleteBoard(
            Guid id,
            DeleteBoardHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new DeleteBoardHandler.DeleteBoardCommand(id), cancellationToken);
            return Results.NoContent();
        }

        private static async Task<IResult> UpdateBoard(
            Guid id,
            UpdateBoardHandler.UpdateBoardCommand command,
            UpdateBoardHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(command with { BoardId = id }, cancellationToken);
            return Results.Ok();
        }

        private static async Task<IResult> GetBoards(
            GetBoardsHandler handler,
            CancellationToken cancellationToken)
        {
            var boards = await handler.Handle(cancellationToken);
            return Results.Ok(boards);
        }

        private static async Task<IResult> GetBoard(
            Guid id,
            GetBoardByIdHandler handler,
            CancellationToken cancellationToken)
        {
            var board = await handler.Handle(new GetBoardByIdHandler.GetBoardByIdCommand(id), cancellationToken);
            return Results.Ok(board);
        }

        private static async Task<IResult> CreateBoard(
            CreateBoardHandler.CreateBoardCommand command,
            CreateBoardHandler handler,
            CancellationToken cancellationToken)
        {
            var boardId = await handler.Handle(command, cancellationToken);
            return Results.Created($"/api/boards/{boardId}", new { id = boardId });
        }
    }
}