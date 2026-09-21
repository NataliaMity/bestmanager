using Domain.Exceptions;

namespace Domain.Entities
{
    /// <summary>
    /// Корень агрегата «колонка + её задачи». Все изменения задач (создание, порядок,
    /// перенос, удаление) проходят через колонку, чтобы порядок всегда оставался 0..N-1.
    /// </summary>
    public class Column
    {
        public const int NameMaxLength = 100;

        public Guid Id { get; private set; }
        public Guid BoardId { get; private set; }
        public string Name { get; private set; } = null!;
        public int Order { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

        private readonly List<TaskItem> _tasks = new();
        public IReadOnlyList<TaskItem> Tasks => _tasks.AsReadOnly();

        // Для EF Core
        private Column() { }

        public Column(Guid boardId, string name, int order)
        {
            if (boardId == Guid.Empty)
                throw new DomainException("Не указана доска");
            if (order < 0)
                throw new DomainException("Порядок колонки не может быть отрицательным");

            Id = Guid.NewGuid();
            BoardId = boardId;
            Name = ValidateName(name);
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

        public TaskItem GetTask(Guid taskId) =>
            _tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new DomainException($"Задача {taskId} не находится в колонке {Id}");

        public TaskItem AddTask(string name, string? description, DateTime? deadline = null)
        {
            SortTasks();
            var task = new TaskItem(Id, name, description, deadline, _tasks.Count);
            _tasks.Add(task);
            Touch();
            return task;
        }

        public void RemoveTask(Guid taskId)
        {
            SortTasks();
            _tasks.Remove(GetTask(taskId));
            RecalculateTaskOrder();
            Touch();
        }

        /// <param name="newIndex">Позиция в колонке. Выход за границы прижимается к началу/концу.</param>
        public void ReorderTask(Guid taskId, int newIndex)
        {
            SortTasks();
            var task = GetTask(taskId);

            _tasks.Remove(task);
            _tasks.Insert(Math.Clamp(newIndex, 0, _tasks.Count), task);

            RecalculateTaskOrder();
            Touch();
        }

        /// <param name="newIndex">Позиция в целевой колонке; null — в конец.</param>
        public void MoveTaskTo(Guid taskId, Column target, int? newIndex = null)
        {
            ArgumentNullException.ThrowIfNull(target);

            if (target == this)
            {
                ReorderTask(taskId, newIndex ?? int.MaxValue);
                return;
            }
            if (target.BoardId != BoardId)
                throw new DomainException("Нельзя перенести задачу в колонку другой доски");

            SortTasks();
            target.SortTasks();

            var task = GetTask(taskId);
            _tasks.Remove(task);
            RecalculateTaskOrder();
            Touch();

            var index = Math.Clamp(newIndex ?? target._tasks.Count, 0, target._tasks.Count);
            task.MoveTo(target.Id, index);
            target._tasks.Insert(index, task);
            target.RecalculateTaskOrder();
            target.Touch();
        }

        /// <summary>
        /// Переставляет колонку внутри доски и пересчитывает порядок у всех колонок.
        /// </summary>
        /// <param name="boardColumns">Все колонки одной доски.</param>
        public static void Reorder(IEnumerable<Column> boardColumns, Guid columnId, int newIndex)
        {
            var columns = boardColumns.OrderBy(c => c.Order).ToList();
            var column = columns.FirstOrDefault(c => c.Id == columnId)
                ?? throw new DomainException($"Колонка {columnId} не принадлежит доске");
            if (columns.Any(c => c.BoardId != column.BoardId))
                throw new DomainException("Колонки должны принадлежать одной доске");

            columns.Remove(column);
            columns.Insert(Math.Clamp(newIndex, 0, columns.Count), column);

            AssignOrder(columns);
        }

        /// <summary>
        /// Убирает «дыры» в порядке колонок (например, после удаления одной из них).
        /// </summary>
        public static void RenumberColumns(IEnumerable<Column> boardColumns) =>
            AssignOrder(boardColumns.OrderBy(c => c.Order).ToList());

        private static void AssignOrder(List<Column> orderedColumns)
        {
            for (var i = 0; i < orderedColumns.Count; i++)
                orderedColumns[i].SetOrder(i);
        }

        private void SetOrder(int order)
        {
            if (Order == order) return;

            Order = order;
            Touch();
        }

        // EF не гарантирует порядок элементов коллекции при загрузке
        private void SortTasks() => _tasks.Sort((a, b) => a.Order.CompareTo(b.Order));

        private void RecalculateTaskOrder()
        {
            for (var i = 0; i < _tasks.Count; i++)
                _tasks[i].SetOrder(i);
        }

        private void Touch() => Updated = DateTime.UtcNow;

        private static string ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Название колонки не может быть пустым");
            name = name.Trim();
            if (name.Length > NameMaxLength)
                throw new DomainException($"Название колонки не может быть длиннее {NameMaxLength} символов");
            return name;
        }
    }
}
