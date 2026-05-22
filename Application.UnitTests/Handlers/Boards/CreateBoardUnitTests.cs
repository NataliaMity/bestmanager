using Application.Handlers.Boards.CreateBoard;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.UseCases.Boards
{
    public class CreateBoardUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task Handler_WhenCalled_ShouldAddBoardAndReturnResponse()
        {
            var request = new CreateBoardRequest("BoardName", "BoardDesc");

            var mockBoardRepo = new Mock<IBoardRepository>();

            Board? captured = null;
            mockBoardRepo
                .Setup(r => r.AddAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()))
                .Callback<Board, CancellationToken>((b, _) => captured = b)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            var useCase = new CreateBoardHandler(mockBoardRepo.Object);

            var response = await useCase.Handler(request);

            Assert.NotNull(captured);
            Assert.Equal(request.Name, captured!.Name);
            Assert.Equal(request.Description, captured.Description);
            Assert.Equal(captured.Id, response.BoardId);

            mockBoardRepo.Verify(r => r.AddAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
