namespace Domain.Entities
{
    public class Task
    {
        public Guid ID { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Column Column { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }
        public DateTime Deadline { get; private set; }
        public int Order { get; private set; }

        public Task(string name, string description, Column column, int order) 
        {
            ID = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Column = column ?? throw new ArgumentNullException(nameof(column));
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
            Description = string.Empty;
            Order = order;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название задачи не может быть пустым");
            if(Name  == newName) return;

            Name = newName;
            Updated = DateTime.Now;
        }

        public void UpdateDescription(string newDescription)
        {
            if(Description  == newDescription) return;

            Description = newDescription ?? string.Empty;
            Updated = DateTime.Now;
        }

        public void MoveTo(Column newColumn)
        {
            Column = newColumn;
            Updated = DateTime.Now;
        }

        public void SetDeadline(DateTime deadline)
        {
            if (deadline < DateTime.Now)
                throw new ArgumentException("Дедлайн не может быть раньше текущей даты");
            Deadline = deadline;
            Updated = DateTime.Now;
        }

    }

}
