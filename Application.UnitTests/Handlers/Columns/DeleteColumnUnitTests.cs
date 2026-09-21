using Application.Common;
using Application.Handlers.Columns.DeleteColumn;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Columns
{
    public class DeleteColumnUnitTests
    {
        private readonly InMemoryStore _store = new();
        private DeleteColumnHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_RemovesColumnAndRenumbersRest()
        {
            var board = _store.AddBoard();
            var a = _store.AddColumn(board, "A");
            var b = _store.AddColumn(board, "B");
            var c = _store.AddColumn(board, "C");

            await Handler.Handle(new DeleteColumnHandler.DeleteColumnCommand(b.Id));

            Assert.DoesNotContain(b, _store.Columns);
            Assert.Equal(0, a.Order);
            Assert.Equal(1, c.Order);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownColumn_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new DeleteColumnHandler.DeleteColumnCommand(Guid.NewGuid())));
        }
    }
}
