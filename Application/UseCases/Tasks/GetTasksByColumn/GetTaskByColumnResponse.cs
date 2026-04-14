namespace Application.UseCases.Tasks.GetTasksByColumn
{
    public record GetTaskByColumnResponse(
        List<Domain.Entities.Task> tasks);
}