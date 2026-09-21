using Application.Common;
using Application.Handlers.Columns.ReorderColumn;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Columns
{
    public class ReorderColumnUnitTests
    {
        private readonly InMemoryStore _store = new();
        private ReorderColumnHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_MovesColumnAndSaves()
        {
            var board = _store.AddBoard();
            var a = _store.AddColumn(board, "A");
            var b = _store.AddColumn(board, "B");
            var c = _store.AddColumn(board, "C");

            await Handler.Handle(new ReorderColumnHandler.ReorderColumnCommand(c.Id, 0));

            Assert.Equal((0, 1, 2), (c.Order, a.Order, b.Order));
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownColumn_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new ReorderColumnHandler.ReorderColumnCommand(Guid.NewGuid(), 0)));
        }
    }
}
