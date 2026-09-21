namespace Domain.Exceptions
{
    /// <summary>
    /// Нарушение бизнес-правила. На уровне API превращается в 400 Bad Request.
    /// </summary>
    public class DomainException(string message) : Exception(message);
}
