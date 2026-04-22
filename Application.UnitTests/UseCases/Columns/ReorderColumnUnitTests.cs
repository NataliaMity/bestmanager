using Application.UseCases.Columns.ReorderColumn;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Columns
{
    public class ReorderColumnUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenBoardColumnsNull_ShouldThrowException()
        {
            var boardId = Guid.NewGuid();
            var request = new ReorderColumnRequest(Guid.NewGuid(), boardId, 0);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByBoardAsync(boardId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Column>?)null);

            var useCase = new ReorderColumnHandler(mockColumnRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnNotFound_ShouldThrowException()
        {
            var board = new Board("Board", "owner");
            var columns = new List<Column>
            {
                new Column("c1", board),
                new Column("c2", board)
            };

            var request = new ReorderColumnRequest(Guid.NewGuid(), board.Id, 0);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByBoardAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(columns);

            var useCase = new ReorderColumnHandler(mockColumnRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenValidRequest_ShouldReorderColumnsInList()
        {
            var board = new Board("Board", "owner");
            var c1 = new Column("c1", board);
            var c2 = new Column("c2", board);
            var c3 = new Column("c3", board);
            var columns = new List<Column> { c1, c2, c3 };

            // move c3 to index 1
            var request = new ReorderColumnRequest(board.Id, c3.Id, 1);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByBoardAsync(board.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(columns);

            var useCase = new ReorderColumnHandler(mockColumnRepo.Object);

            await useCase.Handler(request);

            Assert.Equal(c3.Id, columns[1].Id);
            Assert.Contains(columns, x => x.Id == c1.Id);
            Assert.Contains(columns, x => x.Id == c2.Id);
            Assert.Contains(columns, x => x.Id == c3.Id);
        }
    }
}
