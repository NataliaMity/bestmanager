using Application.UseCases.Boards.UpdateBoard;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Application.UnitTests.UseCases.Boards
{
    public class UpdateBoardUnitTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenBoardNotFound_ShouldThrowExceptionAndNotCallUpdate()
        {
            var boardId = Guid.NewGuid();
            var request = new UpdateBoardRequest("NewName", boardId);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Board?)null);

            var useCase = new UpdateBoardUseCase(mockBoardRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));

            mockBoardRepo.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenNameProvided_ShouldUpdateNameAndCallUpdateAsync()
        {
            var board = new Board("OldName", "desc");
            var request = new UpdateBoardRequest("NewName", board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new UpdateBoardUseCase(mockBoardRepo.Object);

            await useCase.ExecuteAsync(request);

            Assert.Equal("NewName", board.Name);
            mockBoardRepo.Verify(r => r.UpdateAsync(board.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenNameNull_ShouldCallUpdateAsyncWithoutChangingName()
        {
            var board = new Board("OldName", "desc");
            var request = new UpdateBoardRequest(null, board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new UpdateBoardUseCase(mockBoardRepo.Object);

            await useCase.ExecuteAsync(request);

            Assert.Equal("OldName", board.Name);
            mockBoardRepo.Verify(r => r.UpdateAsync(board.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenNameEmptyString_ShouldThrowExceptionAndNotCallUpdate()
        {
            var board = new Board("OldName", "desc");
            var request = new UpdateBoardRequest("", board.Id);

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var useCase = new UpdateBoardUseCase(mockBoardRepo.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await useCase.ExecuteAsync(request));

            mockBoardRepo.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
