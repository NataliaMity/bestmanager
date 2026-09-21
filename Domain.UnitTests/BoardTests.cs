using Domain.Entities;
using Domain.Exceptions;

namespace Domain.UnitTests
{
    public class BoardTests
    {
        [Fact]
        public void Create_SetsFields()
        {
            var board = new Board(" Доска ", null);

            Assert.NotEqual(Guid.Empty, board.Id);
            Assert.Equal("Доска", board.Name);
            Assert.Equal(string.Empty, board.Description);
            Assert.Equal(board.Created, board.Updated);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Create_EmptyName_Throws(string name)
        {
            Assert.Throws<DomainException>(() => new Board(name, null));
        }

        [Fact]
        public void ChangeDescription_TooLong_Throws()
        {
            var board = new Board("Доска", null);

            Assert.Throws<DomainException>(
                () => board.ChangeDescription(new string('x', Board.DescriptionMaxLength + 1)));
        }
    }
}
