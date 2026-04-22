namespace Domain.Entities
{
    public class Column
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public Board Board { get; private set; }
        public Guid BoardId { get; private set; }
        public int Order {  get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }
        private readonly List<Task> _tasks = new();
        public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();

        public Column(string name, Board board)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Board = board ?? throw new ArgumentNullException(nameof(board));
            Order = 0;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название колонки не может быть пустым");
            Name = newName;
            Updated = DateTime.UtcNow;
        }
        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("Не указана задача");

            _tasks.Add(task);

            task.SetColumn(this, _tasks.Count - 1);
            Updated = DateTime.UtcNow;
        }
        public void RemoveTask(Task task)
        {
            if (!_tasks.Remove(task))
                return;

            RecalculateOrder();
            Updated = DateTime.UtcNow;
        }
        public void ReorderTask(Guid taskId, int newIndex)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new InvalidOperationException("Task not found in column");

            _tasks.Remove(task);

            if (newIndex < 0 || newIndex > _tasks.Count)
                newIndex = _tasks.Count;

            _tasks.Insert(newIndex, task);

            RecalculateOrder();
            Updated = DateTime.UtcNow;
        }
        private void RecalculateOrder()
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                _tasks[i].SetOrder(i);
            }
        }
    }
}
