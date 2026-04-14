namespace Domain.Entities
{
    public class Board
    {
        public Guid ID { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Created {  get; private set; }
        public DateTime Updated { get; private set; }

        public Board (string name, string description)
        {
            ID = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название задачи не может быть пустым");
            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? string.Empty;
        }
    }
}
