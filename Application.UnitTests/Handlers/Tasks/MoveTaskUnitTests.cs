using Application.Common;
using Application.Handlers.Tasks.MoveTask;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Tasks
{
    public class MoveTaskUnitTests
    {
        private readonly InMemoryStore _store = new();
        private MoveTaskHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_MovesTaskToOtherColumn()
        {
            var board = _store.AddBoard();
            var todo = _store.AddColumn(board, "To do");
            var done = _store.AddColumn(board, "Done");
            var task = todo.AddTask("Задача", null);
            var other = todo.AddTask("Другая", null);
            var existing = done.AddTask("Уже готово", null);

            await Handler.Handle(new MoveTaskHandler.MoveTaskCommand(task.Id, done.Id, 0));

            Assert.Equal(new[] { other }, todo.Tasks);
            Assert.Equal(0, other.Order);
            Assert.Equal(new[] { task, existing }, done.Tasks);
            Assert.Equal(done.Id, task.ColumnId);
            Assert.Equal((0, 1), (task.Order, existing.Order));
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownTask_ThrowsNotFound()
        {
            var column = _store.AddColumn(_store.AddBoard());

            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new MoveTaskHandler.MoveTaskCommand(Guid.NewGuid(), column.Id, null)));
        }

        [Fact]
        public async Task Handle_UnknownTargetColumn_ThrowsNotFound()
        {
            var column = _store.AddColumn(_store.AddBoard());
            var task = column.AddTask("Задача", null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new MoveTaskHandler.MoveTaskCommand(task.Id, Guid.NewGuid(), null)));

            Assert.Equal(column.Id, task.ColumnId);
            Assert.Equal(0, _store.UnitOfWork.SaveCount);
        }
    }
}
