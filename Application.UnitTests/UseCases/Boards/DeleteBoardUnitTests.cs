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
        public async Task ExecuteAsync_WhenBoardNotFound_ShouldThrowExceptionAndNotCallDelete()
        {
            var boardId = Guid.NewGuid();
            var request = new DeleteBoardRequest(boardId);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Board?)null);

            var useCase = new DeleteBoardUseCase(mockBoardRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));

            mockBoardRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenBoardExists_ShouldCallDeleteOnce()
        {
            var board = new Board("Board", "desc");
            var request = new DeleteBoardRequest(board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new DeleteBoardUseCase(mockBoardRepo.Object);

            await useCase.ExecuteAsync(request);

            mockBoardRepo.Verify(r => r.DeleteAsync(board.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
