using Application.Common;
using Application.Handlers.Tasks.DeleteTask;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Tasks
{
    public class DeleteTaskUnitTests
    {
        private readonly InMemoryStore _store = new();
        private DeleteTaskHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_RemovesTaskAndRenumbersRest()
        {
            var column = _store.AddColumn(_store.AddBoard());
            var a = column.AddTask("A", null);
            var b = column.AddTask("B", null);
            var c = column.AddTask("C", null);

            await Handler.Handle(new DeleteTaskHandler.DeleteTaskCommand(b.Id));

            Assert.Equal(new[] { a, c }, column.Tasks);
            Assert.Equal((0, 1), (a.Order, c.Order));
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownTask_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new DeleteTaskHandler.DeleteTaskCommand(Guid.NewGuid())));
        }
    }
}
