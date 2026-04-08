namespace Domain.Entities
{
    public class Column
    {
        public Guid ID { get; }
        public string Name { get; private set; }
        public Board Board { get; private set; }

        public Column(string name, Board board)
        {
            ID = Guid.NewGuid();
            Name = name;
            Board = board;
        }
    }
}
