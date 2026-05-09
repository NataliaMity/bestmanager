using Application.UseCases.Boards.GetBoard;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Application.UnitTests.UseCases.Boards
{
    public class GetBoardUnitTests
    {
        [Fact]
        public async Task Handler_WhenBoardNotFound_ShouldThrowException()
        {
            var boardId = Guid.NewGuid();
            var request = new GetBoardRequest(boardId);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Board?)null);

            var useCase = new GetBoardsHandler(mockBoardRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));
        }

        [Fact]
        public async Task Handler_WhenBoardExists_ShouldReturnResponse()
        {
            var board = new Board("Board", "desc");
            var request = new GetBoardRequest(board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIdAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new GetBoardsHandler(mockBoardRepo.Object);

            var response = await useCase.Handler(request);

            Assert.NotNull(response);
            Assert.Equal(board, response.Board);
        }
    }
}
