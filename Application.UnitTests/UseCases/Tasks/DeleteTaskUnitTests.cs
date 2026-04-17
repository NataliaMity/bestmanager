using Application.UseCases.Tasks.DeleteTask;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class DeleteTaskUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenTaskExists_ShouldDeleteTask()
        {
            var board = new Board("My Board", "user123");
            var column = new Column("Backlog", board);
            var taskId = Guid.NewGuid();
            var task = new Domain.Entities.Task("Test", "Desc", column, 0);
            var request = new DeleteTaskRequest(taskId);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(repo => repo.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);
            var useCase = new DeleteTaskUseCase(mockTaskRepo.Object);

            await useCase.ExecuteAsync(request);

            mockTaskRepo.Verify(
                repo => repo.DeleteAsync(taskId, It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenTaskDoNotExists_ShouldThrowException()
        {
            var taskId = Guid.NewGuid();
            var request = new DeleteTaskRequest(taskId);

            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new DeleteTaskUseCase(mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(
                async () => await useCase.ExecuteAsync(request));

            mockTaskRepo.Verify(
                repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
