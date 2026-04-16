using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Boards.CreateBoard
{
    public class CreateBoardUseCase(IBoardRepository boardRepository)
    {
        private readonly IBoardRepository boardRepository = boardRepository;

        public async Task<CreateBoardResponse> ExecuteAsync(CreateBoardRequest request, CancellationToken cancellationToken = default)
        {
            var board = new Board(request.Name, request.Description);
            await boardRepository.AddAsync(board);

            return new CreateBoardResponse(board.Id); 
        }
    }
}