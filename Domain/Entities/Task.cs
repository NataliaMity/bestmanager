namespace Domain.Entities
{
    public class Task
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Column Column { get; private set; }
        public Guid ColumnId { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }
        public DateTime Deadline { get; private set; }
        public int Order { get; private set; }

        //to-do: сделать метод .create, который будет создавать задачу и добавлять ее в колонку, а конструктор сделать приватным
        public Task(string name, string? description, Column column, int order) 
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            Column = column;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
            Order = order;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название задачи не может быть пустым");
            if(Name  == newName) return;

            Name = newName;
            Updated = DateTime.UtcNow;
        }

        public void ChangeDescription(string newDescription)
        {
            if(Description  == newDescription) return;

            Description = newDescription;
            Updated = DateTime.UtcNow;
        }
        //to-do: рефактор Column управляет задачами
        public void SetColumn(Column newColumn, int order)
        {
            Column = newColumn;
            ColumnId = newColumn.Id;
            Order = order;
            Updated = DateTime.UtcNow;
        }

        public void SetDeadline(DateTime deadline)
        {
            if (deadline < DateTime.UtcNow)
                throw new ArgumentException("Дедлайн не может быть раньше текущей даты");
            Deadline = deadline;
            Updated = DateTime.UtcNow;
        }

        public void SetOrder(int i)
        {
            Order = i;
            Updated = DateTime.UtcNow;
        }
    }
}