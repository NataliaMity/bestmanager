using Application.UseCases.Tasks.MoveTask;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class MoveTaskUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenTaskNotFound_ShouldThrowException()
        {
            var taskId = Guid.NewGuid();
            var request = new MoveTaskRequest(taskId, Guid.NewGuid());

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Task?)null);

            var mockColumnRepo = new Mock<IColumnRepository>();

            var useCase = new MoveTaskUseCase(mockTaskRepo.Object, mockColumnRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));

            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenColumnNotFound_ShouldThrowException()
        {
            var board = new Board("Board", "owner");
            var originalColumn = new Column("ColA", board);
            var task = new Domain.Entities.Task("t1", null, originalColumn, 0);

            var request = new MoveTaskRequest(task.Id, Guid.NewGuid()); // target column id that does not exist

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(request.ColumnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Column?)null);

            var useCase = new MoveTaskUseCase(mockTaskRepo.Object, mockColumnRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.ExecuteAsync(request));

            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenValidRequest_ShouldMoveTaskAndCallUpdate()
        {
            var board = new Board("Board", "owner");
            var originalColumn = new Column("ColA", board);
            var targetColumn = new Column("ColB", board);
            var task = new Domain.Entities.Task("t1", null, originalColumn, 0);

            var request = new MoveTaskRequest(task.Id, targetColumn.Id);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(r => r.GetByIdAsync(targetColumn.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(targetColumn);

            var useCase = new MoveTaskUseCase(mockTaskRepo.Object, mockColumnRepo.Object);

            await useCase.ExecuteAsync(request);

            Assert.Equal(targetColumn.Id, task.Column.Id);

            mockTaskRepo.Verify(r => r.UpdateAsync(It.Is<Domain.Entities.Task>(t => t.Id == task.Id && t.Column.Id == targetColumn.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}