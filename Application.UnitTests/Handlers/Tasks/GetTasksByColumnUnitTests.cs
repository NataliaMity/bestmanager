using Application.Common;
using Application.Handlers.Tasks.GetTaskById;
using Application.Handlers.Tasks.GetTasksByColumn;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Tasks
{
    public class GetTasksByColumnUnitTests
    {
        private readonly InMemoryStore _store = new();
        private GetTasksByColumnHandler Handler => new(_store.ColumnRepository, _store.TaskRepository);

        [Fact]
        public async Task Handle_ReturnsTasksInOrder()
        {
            var column = _store.AddColumn(_store.AddBoard());
            column.AddTask("A", null);
            var b = column.AddTask("B", null);
            column.ReorderTask(b.Id, 0);

            var tasks = await Handler.Handle(new GetTasksByColumnHandler.GetTasksByColumnQuery(column.Id));

            Assert.Equal(new[] { "B", "A" }, tasks.Select(t => t.Name));
        }

        [Fact]
        public async Task Handle_EmptyColumn_ReturnsEmptyList()
        {
            var column = _store.AddColumn(_store.AddBoard());

            var tasks = await Handler.Handle(new GetTasksByColumnHandler.GetTasksByColumnQuery(column.Id));

            Assert.Empty(tasks);
        }

        [Fact]
        public async Task Handle_UnknownColumn_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new GetTasksByColumnHandler.GetTasksByColumnQuery(Guid.NewGuid())));
        }

        [Fact]
        public async Task GetTaskById_ReturnsTask()
        {
            var column = _store.AddColumn(_store.AddBoard());
            var task = column.AddTask("Задача", "Описание");

            var dto = await new GetTaskByIdHandler(_store.TaskRepository)
                .Handle(new GetTaskByIdHandler.GetTaskByIdQuery(task.Id));

            Assert.Equal(task.Id, dto.Id);
            Assert.Equal("Описание", dto.Description);
        }
    }
}
