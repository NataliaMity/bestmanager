using Domain.Exceptions;

namespace Domain.Entities
{
    public class Board
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

        // Для EF Core
        private Board() { }

        public Board(string name, string? description)
        {
            Id = Guid.NewGuid();
            Name = ValidateName(name);
            Description = ValidateDescription(description);
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

        public void ChangeDescription(string? newDescription)
        {
            newDescription = ValidateDescription(newDescription);
            if (Description == newDescription) return;

            Description = newDescription;
            Updated = DateTime.UtcNow;
        }

        private static string ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Название доски не может быть пустым");
            name = name.Trim();
            if (name.Length > NameMaxLength)
                throw new DomainException($"Название доски не может быть длиннее {NameMaxLength} символов");
            return name;
        }

        private static string ValidateDescription(string? description)
        {
            description = description?.Trim() ?? string.Empty;
            if (description.Length > DescriptionMaxLength)
                throw new DomainException($"Описание доски не может быть длиннее {DescriptionMaxLength} символов");
            return description;
        }
    }
}
