namespace Application.Common
{
    /// <summary>
    /// Запрошенная сущность не найдена. На уровне API превращается в 404 Not Found.
    /// </summary>
    public class NotFoundException(string entityName, Guid id)
        : Exception($"{entityName} с Id {id} не найдена");
}
