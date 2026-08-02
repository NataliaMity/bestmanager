using Application.UseCases.Tasks.CreateTask;
using Application.UseCases.Tasks.DeleteTask;
using Application.UseCases.Tasks.GetTasksByColumn;
using Application.UseCases.Tasks.MoveTask;
using Application.UseCases.Tasks.ReorderTask;
using Application.UseCases.Tasks.UpdateTask;

namespace Web.Endpoints
{
    public static class TaskEndpoints
    {
        public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/tasks");

            group.MapGet("/{TaskId}", GetTasksByColumnId);
            group.MapPost("/", CreateTask);
            group.MapPut("/update/{id}", UpdateTask);
            group.MapDelete("/{id}", DeleteTask);
            group.MapPut("/reorder/{id}", ReorderTask);
            group.MapPut("/move/{id}", MoveTask);
        }

        private static async Task<IResult> ReorderTask(
            ReorderTaskHandler.ReorderTaskCommand command,
            ReorderTaskHandler handler,
            CancellationToken cancellationToken
            )
        {
            await handler.Handle(command, cancellationToken);
            return Results.Ok();
        }

        private static async Task<IResult> DeleteTask(
            Guid id,
            DeleteTaskHandler handler,
            CancellationToken cancellationToken
            )
        {
            await handler.Handle(new DeleteTaskHandler.DeleteTaskCommand(id), cancellationToken);
            return Results.NoContent();
        }

        private static async Task<IResult> UpdateTask(
            Guid id,
            UpdateTaskHandler.UpdateTaskCommand command,
            UpdateTaskHandler handler,
            CancellationToken cancellationToken
            )
        {
            await handler.Handle(command with { TaskId = id}, cancellationToken);
            return Results.Ok();
        }

        private static async Task<IResult> CreateTask(
            CreateTaskHandler.CreateTaskCommand command,
            CreateTaskHandler handler,
            CancellationToken cancellationToken
            )
        {
            var TaskId = await handler.Handle(command, cancellationToken);
            return Results.Created($"/api/Tasks/{TaskId}", new { id = TaskId });
        }

        private static async Task<IResult> GetTasksByColumnId(
            Guid column,
            GetTaskByColumnHandler handler,
            CancellationToken cancellationToken)
        {
            var Tasks = handler.Handle(new GetTaskByColumnHandler.GetTaskByColumnCommand(column), cancellationToken);
            return Results.Ok(Tasks);
        }

        private static async Task<IResult> MoveTask(
            Guid columnId,
            Guid taskId,
            MoveTaskHandler handler,
            CancellationToken cancellationToken)
        {
            var Tasks = handler.Handle(new MoveTaskHandler.MoveTaskCommand(taskId, columnId), cancellationToken);
            return Results.Ok(Tasks);
        }
    }
}