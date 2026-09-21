using Application.Common;
using Application.Handlers.Columns.CreateColumn;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Columns
{
    public class CreateColumnUnitTests
    {
        private readonly InMemoryStore _store = new();
        private CreateColumnHandler Handler => new(_store.BoardRepository, _store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_AddsColumnToEndOfBoard()
        {
            var board = _store.AddBoard();
            _store.AddColumn(board, "To do");

            var id = await Handler.Handle(new CreateColumnHandler.CreateColumnCommand(board.Id, "Done"));

            var column = _store.Columns.Single(c => c.Id == id);
            Assert.Equal(board.Id, column.BoardId);
            Assert.Equal("Done", column.Name);
            Assert.Equal(1, column.Order);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownBoard_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new CreateColumnHandler.CreateColumnCommand(Guid.NewGuid(), "Колонка")));

            Assert.Empty(_store.Columns);
        }
    }
}
