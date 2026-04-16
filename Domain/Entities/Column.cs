namespace Domain.Entities
{
    public class Column
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public Board Board { get; private set; }
        public int Order {  get; private set; }

        public Column(string name, Board board)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Board = board ?? throw new ArgumentNullException(nameof(board));
            Order = 0;
        }
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название колонки не может быть пустым");
            Name = newName;
        }
    }
}
