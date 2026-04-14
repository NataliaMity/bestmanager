namespace Application.UseCases.Tasks.UpdateTask
{
    public record UpdateTaskRequest
    (
        string? Name,
        string? Description,
        Guid TaskId
        );
}