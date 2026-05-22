using Application.UseCases.Tasks.CreateTask;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class CreateTaskUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnExists_ShouldCreateTaskAndReturnResponse()
        {
            var board = new Board("My Board", "user123");
            var column = new Column("Backlog", board);

            var request = new CreateTaskRequest("Test Task", "Description", column.Id, 0);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(repo => repo.GetByIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(column);

            var mockTaskRepo = new Mock<ITaskRepository>();

            var useCase = new CreateTaskUseCase(mockColumnRepo.Object, mockTaskRepo.Object);

            var response = await useCase.Handler(request);

            mockTaskRepo.Verify(
                repo => repo.AddAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.Equal("Test Task", response.TaskName);
        }
        
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnDoNotExists_ShouldThrowException()
        {
            var columnId = Guid.NewGuid();
            var request = new CreateTaskRequest("Test Task", "Description", columnId, 0);

            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(repo => repo.GetByIdAsync(columnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Column?)null);

            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new CreateTaskUseCase(mockColumnRepo.Object, mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(
                async () => await useCase.Handler(request));

            mockTaskRepo.Verify(
                repo => repo.AddAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
