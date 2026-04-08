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

        public Task(string name, string description, Column column, DateTime created, DateTime updated, DateTime deadline) 
        {
            Name = name;
            Description = description;
            Column = column;
            Created = created;
            Updated = updated;
            Deadline = deadline;
        }

    }
}
