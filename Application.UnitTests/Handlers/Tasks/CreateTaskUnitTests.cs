using Application.Common;
using Application.Handlers.Tasks.CreateTask;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Tasks
{
    public class CreateTaskUnitTests
    {
        private readonly InMemoryStore _store = new();
        private CreateTaskHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_AddsTaskToEndOfColumn()
        {
            var column = _store.AddColumn(_store.AddBoard());
            column.AddTask("Первая", null);
            var deadline = DateTime.UtcNow.AddDays(1);

            var id = await Handler.Handle(new CreateTaskHandler.CreateTaskCommand(column.Id, "Вторая", "Описание", deadline));

            var task = column.GetTask(id);
            Assert.Equal("Вторая", task.Name);
            Assert.Equal("Описание", task.Description);
            Assert.Equal(deadline, task.Deadline);
            Assert.Equal(column.Id, task.ColumnId);
            Assert.Equal(1, task.Order);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownColumn_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new CreateTaskHandler.CreateTaskCommand(Guid.NewGuid(), "Задача", null, null)));
        }
    }
}
