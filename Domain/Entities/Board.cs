namespace Domain.Entities
{
    public class Board
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Created {  get; private set; }
        public DateTime Updated { get; private set; }

        public Board (string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название доски не может быть пустым");
            Name = newName;
            Updated = DateTime.UtcNow;
        }

        public void ChangeDescription(string newDescription)
        {
            Description = newDescription ?? string.Empty;
            Updated = DateTime.UtcNow;
        }
    }
}
