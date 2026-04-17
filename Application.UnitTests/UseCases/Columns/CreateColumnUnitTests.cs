using Application.UseCases.Columns.CreateColumn;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Columns
{
    public class CreateColumnUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenBoardNotFound_ShouldThrowExceptionAndNotAddColumn()
        {
            var boardId = Guid.NewGuid();
            var request = new CreateColumnRequest(boardId, "New Column");

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Board?)null);
            
            var mockColumnRepo = new Mock<IColumnRepository>();

            var useCase = new CreateColumnUseCase(mockColumnRepo.Object, mockBoardRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));

            mockColumnRepo.Verify(r => r.AddAsync(It.IsAny<Column>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenBoardExists_ShouldAddColumnAndReturnResponse()
        {
            var board = new Board("Board", "owner");
            var request = new CreateColumnRequest(board.Id, "New Column");

            var mockBoardRepo = new Mock<IBoardRepository>();
            mockBoardRepo
                .Setup(r => r.GetByIDAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(board);

            var mockColumnRepo = new Mock<IColumnRepository>();

            Column? captured = null;
            mockColumnRepo
                .Setup(r => r.AddAsync(It.IsAny<Column>(), It.IsAny<CancellationToken>()))
                .Callback<Column, CancellationToken>((c, _) => captured = c)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var useCase = new CreateColumnUseCase(mockColumnRepo.Object, mockBoardRepo.Object);

            var response = await useCase.ExecuteAsync(request);

            Assert.NotNull(captured);
            Assert.Equal(request.Name, captured!.Name);
            Assert.Equal(request.BoardId, captured.Board.Id);

            Assert.Equal(captured.Id, response.ColumnId);

            mockColumnRepo.Verify(r => r.AddAsync(It.IsAny<Column>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}