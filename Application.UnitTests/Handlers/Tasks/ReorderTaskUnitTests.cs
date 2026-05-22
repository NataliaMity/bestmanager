using Application.UseCases.Tasks.ReorderTask;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class ReorderTaskUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnIsNullFromRepository_ShouldThrowException()
        {
            var columnId = Guid.NewGuid();
            var request = new ReorderTaskRequest(Guid.NewGuid(), columnId, 0);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByColumnIdAsync(columnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Domain.Entities.Task>?)null);

            var useCase = new ReorderTaskHandler(mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));

            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnHasNoTasks_ShouldThrowException()
        {
            var columnId = Guid.NewGuid();
            var request = new ReorderTaskRequest(Guid.NewGuid(), columnId, 0);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByColumnIdAsync(columnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Entities.Task>());

            var useCase = new ReorderTaskHandler(mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));

            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenTaskNotFoundInColumn_ShouldThrowException()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);

            // create tasks that do not contain the requested task id
            var tasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task("t1", null, column, 0),
                new Domain.Entities.Task("t2", null, column, 1)
            };

            var request = new ReorderTaskRequest(Guid.NewGuid(), column.Id, 0);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByColumnIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var useCase = new ReorderTaskHandler(mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));

            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenValidRequest_ShouldMoveTaskToRequestedIndex()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);

            var t1 = new Domain.Entities.Task("t1", null, column, 0);
            var t2 = new Domain.Entities.Task("t2", null, column, 1);
            var t3 = new Domain.Entities.Task("t3", null, column, 2);

            var tasks = new List<Domain.Entities.Task> { t1, t2, t3 };

            // move t3 to index 1 (middle)
            var request = new ReorderTaskRequest(t3.Id, column.Id, 1);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByColumnIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var useCase = new ReorderTaskHandler(mockTaskRepo.Object);

            await useCase.Handler(request);

            // After reorder, tasks should contain t3 at index 1
            Assert.Equal(t3.Id, tasks[1].Id);

            // Ensure list still has same items
            Assert.Contains(tasks, x => x.Id == t1.Id);
            Assert.Contains(tasks, x => x.Id == t2.Id);
            Assert.Contains(tasks, x => x.Id == t3.Id);

            // The use case does not call repository update in current implementation
            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
