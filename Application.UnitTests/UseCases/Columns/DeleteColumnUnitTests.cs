using Application.UseCases.Columns.DeleteColumn;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Columns
{
    public class DeleteColumnUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenColumnNotFound_ShouldThrowExceptionAndNotCallDelete()
        {
            var columnId = Guid.NewGuid();
            var request = new DeleteColumnRequest(columnId);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(columnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Column?)null);

            var useCase = new DeleteColumnUseCase(mockColumnRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));

            mockColumnRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenColumnExists_ShouldCallDeleteOnce()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);
            var request = new DeleteColumnRequest(column.Id);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(column);

            var useCase = new DeleteColumnUseCase(mockColumnRepo.Object);

            await useCase.ExecuteAsync(request);

            mockColumnRepo.Verify(r => r.DeleteAsync(column.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
