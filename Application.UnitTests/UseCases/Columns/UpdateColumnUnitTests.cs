using Application.UseCases.Columns.UpdateColumn;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Columns
{
    public class UpdateColumnUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnNotFound_ShouldThrowAndNotCallUpdate()
        {
            var columnId = Guid.NewGuid();
            var request = new UpdateColumnRequest("NewName", columnId);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(columnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Column?)null);

            var useCase = new UpdateColumnHandler(mockColumnRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));

            mockColumnRepo.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenNameProvided_ShouldUpdateNameAndCallUpdateAsync()
        {
            var board = new Board("Board", "owner");
            var column = new Column("OldName", board);
            var request = new UpdateColumnRequest("NewName", column.Id);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(column);

            var useCase = new UpdateColumnHandler(mockColumnRepo.Object);

            await useCase.Handler(request);

            Assert.Equal("NewName", column.Name);
            mockColumnRepo.Verify(r => r.UpdateAsync(column.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenNameNull_ShouldCallUpdateAsyncWithoutChangingName()
        {
            var board = new Board("Board", "owner");
            var column = new Column("OldName", board);
            var request = new UpdateColumnRequest(null, column.Id);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(column);

            var useCase = new UpdateColumnHandler(mockColumnRepo.Object);

            await useCase.Handler(request);

            Assert.Equal("OldName", column.Name);
            mockColumnRepo.Verify(r => r.UpdateAsync(column.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
