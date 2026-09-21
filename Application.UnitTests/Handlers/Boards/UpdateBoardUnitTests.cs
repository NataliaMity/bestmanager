using Application.Common;
using Application.Handlers.Boards.UpdateBoard;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Boards
{
    public class UpdateBoardUnitTests
    {
        private readonly InMemoryStore _store = new();
        private UpdateBoardHandler Handler => new(_store.BoardRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_OnlyName_ChangesNameKeepsDescription()
        {
            var board = _store.AddBoard("Старое");
            board.ChangeDescription("Описание");

            await Handler.Handle(new UpdateBoardHandler.UpdateBoardCommand(board.Id, "Новое", null));

            Assert.Equal("Новое", board.Name);
            Assert.Equal("Описание", board.Description);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownBoard_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new UpdateBoardHandler.UpdateBoardCommand(Guid.NewGuid(), "Имя", null)));
        }
    }
}
