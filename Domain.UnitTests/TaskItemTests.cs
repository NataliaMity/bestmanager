using Domain.Entities;
using Domain.Exceptions;

namespace Domain.UnitTests
{
    public class TaskItemTests
    {
        private static TaskItem NewTask(DateTime? deadline = null) =>
            new Column(Guid.NewGuid(), "Колонка", 0).AddTask("Задача", "Описание", deadline);

        [Fact]
        public void Create_TrimsNameAndDescription()
        {
            var task = new Column(Guid.NewGuid(), "Колонка", 0).AddTask("  Задача  ", "  Описание ");

            Assert.Equal("Задача", task.Name);
            Assert.Equal("Описание", task.Description);
        }

        [Fact]
        public void Create_NullDescription_BecomesEmpty()
        {
            var task = new Column(Guid.NewGuid(), "Колонка", 0).AddTask("Задача", null);

            Assert.Equal(string.Empty, task.Description);
        }

        [Fact]
        public void Create_DeadlineInPast_Throws()
        {
            Assert.Throws<DomainException>(() => NewTask(DateTime.UtcNow.AddDays(-1)));
        }

        [Fact]
        public void Rename_TooLongName_Throws()
        {
            var task = NewTask();

            Assert.Throws<DomainException>(() => task.Rename(new string('x', TaskItem.NameMaxLength + 1)));
        }

        [Fact]
        public void Rename_SameName_DoesNotTouchUpdated()
        {
            var task = NewTask();
            var updated = task.Updated;

            task.Rename("Задача");

            Assert.Equal(updated, task.Updated);
        }

        [Fact]
        public void SetDeadline_Null_ClearsDeadline()
        {
            var task = NewTask(DateTime.UtcNow.AddDays(1));

            task.SetDeadline(null);

            Assert.Null(task.Deadline);
        }

        [Fact]
        public void SetDeadline_StoresUtc()
        {
            var task = NewTask();
            var local = DateTime.SpecifyKind(DateTime.Now.AddDays(1), DateTimeKind.Local);

            task.SetDeadline(local);

            Assert.Equal(DateTimeKind.Utc, task.Deadline!.Value.Kind);
            Assert.Equal(local.ToUniversalTime(), task.Deadline);
        }
    }
}
