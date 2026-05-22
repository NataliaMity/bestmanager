using Application.UseCases.Boards.DeleteBoard;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Application.UnitTests.UseCases.Boards
{
    public class DeleteBoardUnitTests
    {
        [Fact]
        public async Task Handler_WhenBoardNotFound_ShouldThrowExceptionAndNotCallDelete()
        {
            var boardId = Guid.NewGuid();
            var request = new DeleteBoardRequest(boardId);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIdAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Board?)null);

            var useCase = new DeleteBoardHandler(mockBoardRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));

            mockBoardRepo.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handler_WhenBoardExists_ShouldCallDeleteOnce()
        {
            var board = new Board("Board", "desc");
            var request = new DeleteBoardRequest(board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIdAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new DeleteBoardHandler(mockBoardRepo.Object);

            await useCase.Handler(request);

            mockBoardRepo.Verify(r => r.RemoveAsync(board.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
