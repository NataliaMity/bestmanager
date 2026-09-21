using Application.Common;
using Application.Handlers.Columns.UpdateColumn;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Columns
{
    public class UpdateColumnUnitTests
    {
        private readonly InMemoryStore _store = new();
        private UpdateColumnHandler Handler => new(_store.ColumnRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_RenamesColumn()
        {
            var column = _store.AddColumn(_store.AddBoard(), "Старое");

            await Handler.Handle(new UpdateColumnHandler.UpdateColumnCommand(column.Id, "Новое"));

            Assert.Equal("Новое", column.Name);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownColumn_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new UpdateColumnHandler.UpdateColumnCommand(Guid.NewGuid(), "Имя")));
        }
    }
}
