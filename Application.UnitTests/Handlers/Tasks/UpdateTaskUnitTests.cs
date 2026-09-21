using Application.Common;
using Application.Handlers.Tasks.UpdateTask;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Tasks
{
    public class UpdateTaskUnitTests
    {
        private readonly InMemoryStore _store = new();
        private UpdateTaskHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_UpdatesOnlyPassedFields()
        {
            var column = _store.AddColumn(_store.AddBoard());
            var task = column.AddTask("Старое", "Описание");

            await Handler.Handle(new UpdateTaskHandler.UpdateTaskCommand(task.Id, "Новое", null, null));

            Assert.Equal("Новое", task.Name);
            Assert.Equal("Описание", task.Description);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_RemoveDeadline_ClearsDeadline()
        {
            var column = _store.AddColumn(_store.AddBoard());
            var task = column.AddTask("Задача", null, DateTime.UtcNow.AddDays(1));

            await Handler.Handle(new UpdateTaskHandler.UpdateTaskCommand(task.Id, null, null, null, RemoveDeadline: true));

            Assert.Null(task.Deadline);
        }

        [Fact]
        public async Task Handle_UnknownTask_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new UpdateTaskHandler.UpdateTaskCommand(Guid.NewGuid(), "Имя", null, null)));
        }
    }
}
