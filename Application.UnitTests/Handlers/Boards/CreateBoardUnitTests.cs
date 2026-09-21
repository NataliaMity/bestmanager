using Application.Handlers.Boards.CreateBoard;
using Application.UnitTests.Fakes;
using Domain.Exceptions;

namespace Application.UnitTests.Handlers.Boards
{
    public class CreateBoardUnitTests
    {
        private readonly InMemoryStore _store = new();
        private CreateBoardHandler Handler => new(_store.BoardRepository, _store.UnitOfWork);

        [Fact]
        public async Task Handle_ValidCommand_AddsBoardAndSaves()
        {
            var id = await Handler.Handle(new CreateBoardHandler.CreateBoardCommand("Доска", "Описание"));

            var board = Assert.Single(_store.Boards);
            Assert.Equal(id, board.Id);
            Assert.Equal("Доска", board.Name);
            Assert.Equal("Описание", board.Description);
            Assert.Equal(1, _store.UnitOfWork.SaveCount);
        }

        [Fact]
        public async Task Handle_EmptyName_ThrowsDomainException()
        {
            await Assert.ThrowsAsync<DomainException>(
                () => Handler.Handle(new CreateBoardHandler.CreateBoardCommand("  ", null)));

            Assert.Empty(_store.Boards);
            Assert.Equal(0, _store.UnitOfWork.SaveCount);
        }
    }
}
