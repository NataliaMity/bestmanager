using Application.Common;
using Application.Handlers.Boards.DeleteBoard;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Boards
{
    public class DeleteBoardUnitTests
    {
        private readonly InMemoryStore _store = new();
        private DeleteBoardHandler Handler => new(_store.BoardRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_ExistingBoard_RemovesAndSaves()
        {
            var board = _store.AddBoard();

            await Handler.Handle(new DeleteBoardHandler.DeleteBoardCommand(board.Id));

            Assert.Empty(_store.Boards);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_UnknownBoard_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                () => Handler.Handle(new DeleteBoardHandler.DeleteBoardCommand(Guid.NewGuid())));

            Assert.Equal(0, _store.UnitOfWork.SaveCount);
        }
    }
}
