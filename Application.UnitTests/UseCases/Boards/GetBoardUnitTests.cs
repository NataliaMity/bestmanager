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
        public async Task ExecuteAsync_WhenBoardNotFound_ShouldThrowException()
        {
            var boardId = Guid.NewGuid();
            var request = new GetBoardRequest(boardId);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Board?)null);

            var useCase = new GetBoardUseCase(mockBoardRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_WhenBoardExists_ShouldReturnResponse()
        {
            var board = new Board("Board", "desc");
            var request = new GetBoardRequest(board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new GetBoardUseCase(mockBoardRepo.Object);

            var response = await useCase.ExecuteAsync(request);

            Assert.NotNull(response);
            Assert.Equal(board, response.Board);
        }
    }
}
