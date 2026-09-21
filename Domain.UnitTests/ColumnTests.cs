using Domain.Entities;
using Domain.Exceptions;

namespace Domain.UnitTests
{
    public class ColumnTests
    {
        private static readonly Guid BoardId = Guid.NewGuid();

        private static Column NewColumn(string name = "Колонка", int order = 0) => new(BoardId, name, order);

        [Fact]
        public void AddTask_AppendsWithSequentialOrder()
        {
            var column = NewColumn();

            var a = column.AddTask("A", null);
            var b = column.AddTask("B", null);

            Assert.Equal(new[] { a, b }, column.Tasks);
            Assert.Equal((0, 1), (a.Order, b.Order));
            Assert.All(column.Tasks, t => Assert.Equal(column.Id, t.ColumnId));
        }

        [Theory]
        [InlineData(0, new[] { "C", "A", "B" })]
        [InlineData(1, new[] { "A", "C", "B" })]
        [InlineData(-5, new[] { "C", "A", "B" })]
        [InlineData(100, new[] { "A", "B", "C" })]
        public void ReorderTask_PlacesTaskAtIndex(int index, string[] expected)
        {
            var column = NewColumn();
            column.AddTask("A", null);
            column.AddTask("B", null);
            var c = column.AddTask("C", null);

            column.ReorderTask(c.Id, index);

            Assert.Equal(expected, column.Tasks.Select(t => t.Name));
            Assert.Equal(new[] { 0, 1, 2 }, column.Tasks.Select(t => t.Order));
        }

        [Fact]
        public void RemoveTask_ClosesGapInOrder()
        {
            var column = NewColumn();
            var a = column.AddTask("A", null);
            var b = column.AddTask("B", null);
            var c = column.AddTask("C", null);

            column.RemoveTask(a.Id);

            Assert.Equal(new[] { b, c }, column.Tasks);
            Assert.Equal((0, 1), (b.Order, c.Order));
        }

        [Fact]
        public void RemoveTask_UnknownTask_Throws()
        {
            Assert.Throws<DomainException>(() => NewColumn().RemoveTask(Guid.NewGuid()));
        }

        [Fact]
        public void MoveTaskTo_OtherColumn_UpdatesBothColumns()
        {
            var source = NewColumn("To do");
            var target = NewColumn("Done", 1);
            var a = source.AddTask("A", null);
            var b = source.AddTask("B", null);
            var x = target.AddTask("X", null);
            var y = target.AddTask("Y", null);

            source.MoveTaskTo(a.Id, target, 1);

            Assert.Equal(new[] { b }, source.Tasks);
            Assert.Equal(0, b.Order);
            Assert.Equal(new[] { x, a, y }, target.Tasks);
            Assert.Equal(new[] { 0, 1, 2 }, target.Tasks.Select(t => t.Order));
            Assert.Equal(target.Id, a.ColumnId);
        }

        [Fact]
        public void MoveTaskTo_WithoutIndex_AppendsToEnd()
        {
            var source = NewColumn();
            var target = NewColumn(order: 1);
            var a = source.AddTask("A", null);
            target.AddTask("X", null);

            source.MoveTaskTo(a.Id, target);

            Assert.Same(a, target.Tasks[^1]);
            Assert.Equal(1, a.Order);
        }

        [Fact]
        public void MoveTaskTo_SameColumn_Reorders()
        {
            var column = NewColumn();
            var a = column.AddTask("A", null);
            var b = column.AddTask("B", null);

            column.MoveTaskTo(a.Id, column, 1);

            Assert.Equal(new[] { b, a }, column.Tasks);
        }

        [Fact]
        public void MoveTaskTo_ColumnOfOtherBoard_Throws()
        {
            var source = NewColumn();
            var foreign = new Column(Guid.NewGuid(), "Чужая", 0);
            var a = source.AddTask("A", null);

            Assert.Throws<DomainException>(() => source.MoveTaskTo(a.Id, foreign));
            Assert.Equal(new[] { a }, source.Tasks);
        }

        [Fact]
        public void Reorder_MovesColumnAndRenumbers()
        {
            var a = NewColumn("A", 0);
            var b = NewColumn("B", 1);
            var c = NewColumn("C", 2);

            Column.Reorder(new[] { a, b, c }, a.Id, 2);

            Assert.Equal((0, 1, 2), (b.Order, c.Order, a.Order));
        }

        [Fact]
        public void Reorder_ColumnNotInList_Throws()
        {
            var a = NewColumn("A", 0);

            Assert.Throws<DomainException>(() => Column.Reorder(new[] { a }, Guid.NewGuid(), 0));
        }

        [Fact]
        public void RenumberColumns_RemovesGaps()
        {
            var a = NewColumn("A", 0);
            var c = NewColumn("C", 2);
            var d = NewColumn("D", 5);

            Column.RenumberColumns(new[] { d, a, c });

            Assert.Equal((0, 1, 2), (a.Order, c.Order, d.Order));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_EmptyName_Throws(string name)
        {
            Assert.Throws<DomainException>(() => new Column(BoardId, name, 0));
        }

        [Fact]
        public void Create_WithoutBoard_Throws()
        {
            Assert.Throws<DomainException>(() => new Column(Guid.Empty, "Колонка", 0));
        }
    }
}
