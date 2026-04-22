using Application.UseCases.Tasks.GetTasksByColumn;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class GetTaskByColumnUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenColumnNotFound_ShouldThrowException()
        {
            var columnId = Guid.NewGuid();
            var request = new GetTaskByColumnRequest(columnId);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByColumnIdAsync(columnId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Domain.Entities.Task>?)null);

            var useCase = new GetTaskByColumnHandler(mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenTasksExist_ShouldReturnResponse()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);

            var t1 = new Domain.Entities.Task("t1", null, column, 0);
            var t2 = new Domain.Entities.Task("t2", null, column, 1);
            var tasks = new List<Domain.Entities.Task> { t1, t2 };

            var request = new GetTaskByColumnRequest(column.Id);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByColumnIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var useCase = new GetTaskByColumnHandler(mockTaskRepo.Object);

            var response = await useCase.Handler(request);

            Assert.NotNull(response);
            Assert.Equal(tasks, response.tasks);
        }
    }
}
