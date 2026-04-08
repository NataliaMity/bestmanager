namespace Domain.Entities
{
    public class Board
    {
        public Guid ID { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Created {  get; private set; }
        public DateTime Updated { get; private set; }

        public Board (string name, string description, DateTime created, DateTime updated)
        {
            ID = Guid.NewGuid();
            Name = name;
            Description = description;
            Created = created;
            Updated = updated;
        }
    }
}
