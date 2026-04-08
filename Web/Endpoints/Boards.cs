
namespace Web.Endpoints
{
    public static class Boards
    {
        public static void MapBoardsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/boards"); 

            group.MapGet("/", GetUserBoards);
            group.MapPost("/", CreateBoard);
            group.MapPut("/{id}", UpdateBoard);
            group.MapDelete("/{id}", DeleteBoard);
        }

        private static async Task DeleteBoard(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateBoard(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task<IResult> GetUserBoards(
            IBoardRepository repository,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;
            var boards = await repository.GetByUserIdAsync(userId, cancellationToken);
            return Results.Ok(boards);
        }

        private static async Task<IResult> CreateBoard(
            CreateBoardRequest request,
            IMediator mediator,
            CancellationToken cancellationToken)
        { ... }
    }
}
