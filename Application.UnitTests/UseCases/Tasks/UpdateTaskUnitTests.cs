using Application.UseCases.Tasks.UpdateTask;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class UpdateTaskUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenTaskNotFound_ShouldThrowException()
        {
            var taskId = Guid.NewGuid();
            var request = new UpdateTaskRequest("NewName", "NewDesc", taskId);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Task?)null);

            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object);

            await Assert.ThrowsAsync<Exception>(async () => await useCase.Handler(request));

            mockTaskRepo.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenNameAndDescriptionProvided_ShouldUpdateAndCallUpdateAsync()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);
            var task = new Domain.Entities.Task("OldName", "OldDesc", column, 0);

            var newName = "NewName";
            var newDesc = "NewDesc";
            var request = new UpdateTaskRequest(newName, newDesc, task.Id);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object);

            await useCase.Handler(request);

            Assert.Equal(newName, task.Name);
            Assert.Equal(newDesc, task.Description);

            mockTaskRepo.Verify(r => r.UpdateAsync(It.Is<Domain.Entities.Task>(t => t.Id == task.Id && t.Name == newName 
                && t.Description == newDesc), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenOnlyDescriptionProvided_ShouldUpdateAndCallUpdateAsync()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);
            var task = new Domain.Entities.Task("OldName", "OldDesc", column, 0);

            var newDesc = "NewDesc";
            var request = new UpdateTaskRequest(null, newDesc, task.Id);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object);

            await useCase.Handler(request);

            Assert.Equal(newDesc, task.Description);

            mockTaskRepo.Verify(r => r.UpdateAsync(It.Is<Domain.Entities.Task>(t => t.Id == task.Id && t.Description == newDesc), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenOnlyNameProvided_ShouldUpdateAndCallUpdateAsync()
        {
            var board = new Board("Board", "owner");
            var column = new Column("Col", board);
            var task = new Domain.Entities.Task("OldName", "OldDesc", column, 0);

            var newName = "NewName";
            var request = new UpdateTaskRequest(newName, null, task.Id);

            var mockTaskRepo = new Mock<ITaskRepository>();
            mockTaskRepo
                .Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object);

            await useCase.Handler(request);

            Assert.Equal(newName, task.Name);

            mockTaskRepo.Verify(r => r.UpdateAsync(It.Is<Domain.Entities.Task>(t => t.Id == task.Id && t.Name == newName), 
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
