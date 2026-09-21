using Application.Common;
using Application.Handlers.Tasks.ReorderTask;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Tasks
{
    public class ReorderTaskUnitTests
    {
        private readonly InMemoryStore _store = new();
        private ReorderTaskHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_ReordersTaskInColumn()
        {
            var column = _store.AddColumn(_store.AddBoard());
            var a = column.AddTask("A", null);
            var b = column.AddTask("B", null);
            var c = column.AddTask("C", null);

            await Handler.Handle(new ReorderTaskHandler.ReorderTaskCommand(a.Id, 2));

            Assert.Equal(new[] { b, c, a }, column.Tasks);
            Assert.Equal((0, 1, 2), (b.Order, c.Order, a.Order));
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownTask_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new ReorderTaskHandler.ReorderTaskCommand(Guid.NewGuid(), 0)));
        }
    }
}
