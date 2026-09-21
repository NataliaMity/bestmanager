using Application.Handlers.Boards;
using Application.Handlers.Boards.CreateBoard;
using Application.Handlers.Boards.DeleteBoard;
using Application.Handlers.Boards.GetBoardById;
using Application.Handlers.Boards.GetBoards;
using Application.Handlers.Boards.UpdateBoard;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Endpoints
{
    public static class BoardEndpoints
    {
        public record UpdateBoardRequest(string? Name, string? Description);

        public static IEndpointRouteBuilder MapBoardEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/boards").WithTags("Boards");

            group.MapGet("/", GetBoards);
            group.MapGet("/get/{id:guid}", GetBoard);
            group.MapPost("/", CreateBoard);
            group.MapPut("/update/{id:guid}", UpdateBoard);
            group.MapDelete("/delete/{id:guid}", DeleteBoard);

            return app;
        }

        private static async Task<Ok<List<BoardDto>>> GetBoards(
            GetBoardsHandler handler,
            CancellationToken cancellationToken)
        {
            return TypedResults.Ok(await handler.Handle(cancellationToken));
        }

        /// <summary>Доска целиком: колонки по порядку, в каждой — задачи по порядку.</summary>
        private static async Task<Ok<BoardDetailsDto>> GetBoard(
            Guid id,
            GetBoardByIdHandler handler,
            CancellationToken cancellationToken)
        {
            var board = await handler.Handle(new GetBoardByIdHandler.GetBoardByIdQuery(id), cancellationToken);
            return TypedResults.Ok(board);
        }

        private static async Task<Created<IdResponse>> CreateBoard(
            CreateBoardHandler.CreateBoardCommand command,
            CreateBoardHandler handler,
            CancellationToken cancellationToken)
        {
            var boardId = await handler.Handle(command, cancellationToken);
            return TypedResults.Created($"/api/boards/get/{boardId}", new IdResponse(boardId));
        }

        private static async Task<NoContent> UpdateBoard(
            Guid id,
            UpdateBoardRequest request,
            UpdateBoardHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new UpdateBoardHandler.UpdateBoardCommand(id, request.Name, request.Description), cancellationToken);
            return TypedResults.NoContent();
        }

        private static async Task<NoContent> DeleteBoard(
            Guid id,
            DeleteBoardHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new DeleteBoardHandler.DeleteBoardCommand(id), cancellationToken);
            return TypedResults.NoContent();
        }
    }
}
