using Domain.Exceptions;

namespace Domain.Entities
{
    /// <summary>
    /// Задача (карточка) в колонке. Создаётся, перемещается и удаляется только через <see cref="Column"/>.
    /// Называется TaskItem, чтобы не конфликтовать с System.Threading.Tasks.Task.
    /// </summary>
    public class TaskItem
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        public Guid Id { get; private set; }
        public Guid ColumnId { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public DateTime? Deadline { get; private set; }
        public int Order { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

        // Для EF Core
        private TaskItem() { }

        internal TaskItem(Guid columnId, string name, string? description, DateTime? deadline, int order)
        {
            Id = Guid.NewGuid();
            ColumnId = columnId;
            Name = ValidateName(name);
            Description = ValidateDescription(description);
            Deadline = ValidateDeadline(deadline);
            Order = order;
            Created = DateTime.UtcNow;
            Updated = Created;
        }

        public void Rename(string newName)
        {
            newName = ValidateName(newName);
            if (Name == newName) return;

            Name = newName;
            Updated = DateTime.UtcNow;
        }

        public void ChangeDescription(string? newDescription)
        {
            newDescription = ValidateDescription(newDescription);
            if (Description == newDescription) return;

            Description = newDescription;
            Updated = DateTime.UtcNow;
        }

        /// <param name="deadline">null — снять дедлайн.</param>
        public void SetDeadline(DateTime? deadline)
        {
            if (Deadline == deadline) return;

            Deadline = ValidateDeadline(deadline);
            Updated = DateTime.UtcNow;
        }

        internal void MoveTo(Guid columnId, int order)
        {
            ColumnId = columnId;
            Order = order;
            Updated = DateTime.UtcNow;
        }

        internal void SetOrder(int order)
        {
            if (Order == order) return;

            Order = order;
            Updated = DateTime.UtcNow;
        }

        private static string ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Название задачи не может быть пустым");
            name = name.Trim();
            if (name.Length > NameMaxLength)
                throw new DomainException($"Название задачи не может быть длиннее {NameMaxLength} символов");
            return name;
        }

        private static string ValidateDescription(string? description)
        {
            description = description?.Trim() ?? string.Empty;
            if (description.Length > DescriptionMaxLength)
                throw new DomainException($"Описание задачи не может быть длиннее {DescriptionMaxLength} символов");
            return description;
        }

        private static DateTime? ValidateDeadline(DateTime? deadline)
        {
            if (deadline is null) return null;

            var utc = deadline.Value.ToUniversalTime();
            if (utc < DateTime.UtcNow)
                throw new DomainException("Дедлайн не может быть раньше текущей даты");
            return utc;
        }
    }
}
