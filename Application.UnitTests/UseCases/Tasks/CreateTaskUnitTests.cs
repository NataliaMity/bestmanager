using Application.UseCases.Tasks.CreateTask;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Tasks
{
    public class CreateTaskUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ExecuteAsync_WhenColumnExists_ShouldCreateTaskAndReturnResponse()
        {
            // Arrange
            var board = new Board("My Board", "user123");           // ← создаём Board сами
            var column = new Column("Backlog", board);              // ← передаём Board в Column

            var request = new CreateTaskRequest("Test Task", "Description", column.Id, 0);

            // 1. Создаём мок репозитория колонок
            var mockColumnRepo = new Mock<IColumnRepository>();
            mockColumnRepo
                .Setup(repo => repo.GetByIdAsync(column.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(column);

            // 2. Создаём мок репозитория задач
            var mockTaskRepo = new Mock<ITaskRepository>();

            // 3. Создаём Use Case с моками
            var useCase = new CreateTaskUseCase(mockColumnRepo.Object, mockTaskRepo.Object);

            // Act
            var response = await useCase.ExecuteAsync(request);

            // Assert
            // Проверяем, что AddAsync был вызван 1 раз
            mockTaskRepo.Verify(
                repo => repo.AddAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()),
                Times.Once);

            // Проверяем, что ответ содержит правильное имя
            Assert.Equal("Test Task", response.TaskName);
        }
    }
}
