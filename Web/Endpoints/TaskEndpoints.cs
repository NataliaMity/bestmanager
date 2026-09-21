using Application.Handlers.Tasks;
using Application.Handlers.Tasks.CreateTask;
using Application.Handlers.Tasks.DeleteTask;
using Application.Handlers.Tasks.GetTaskById;
using Application.Handlers.Tasks.GetTasksByColumn;
using Application.Handlers.Tasks.MoveTask;
using Application.Handlers.Tasks.ReorderTask;
using Application.Handlers.Tasks.UpdateTask;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Endpoints
{
    public static class TaskEndpoints
    {
        public record UpdateTaskRequest(string? Name, string? Description, DateTime? Deadline, bool RemoveDeadline = false);

        /// <param name="Index">Позиция в целевой колонке (с 0); null — в конец.</param>
        public record MoveTaskRequest(Guid ColumnId, int? Index);

        public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/tasks").WithTags("Tasks");

            group.MapGet("/{columnId:guid}", GetTasksByColumnId);
            group.MapGet("/get/{id:guid}", GetTask);
            group.MapPost("/", CreateTask);
            group.MapPut("/update/{id:guid}", UpdateTask);
            group.MapDelete("/delete/{id:guid}", DeleteTask);
            group.MapPut("/reorder/{id:guid}", ReorderTask);
            group.MapPut("/move/{id:guid}", MoveTask);

            return app;
        }

        private static async Task<Ok<List<TaskDto>>> GetTasksByColumnId(
            Guid columnId,
            GetTasksByColumnHandler handler,
            CancellationToken cancellationToken)
        {
            var tasks = await handler.Handle(new GetTasksByColumnHandler.GetTasksByColumnQuery(columnId), cancellationToken);
            return TypedResults.Ok(tasks);
        }

        private static async Task<Ok<TaskDto>> GetTask(
            Guid id,
            GetTaskByIdHandler handler,
            CancellationToken cancellationToken)
        {
            var task = await handler.Handle(new GetTaskByIdHandler.GetTaskByIdQuery(id), cancellationToken);
            return TypedResults.Ok(task);
        }

        /// <summary>Задача добавляется в конец колонки.</summary>
        private static async Task<Created<IdResponse>> CreateTask(
            CreateTaskHandler.CreateTaskCommand command,
            CreateTaskHandler handler,
            CancellationToken cancellationToken)
        {
            var taskId = await handler.Handle(command, cancellationToken);
            return TypedResults.Created($"/api/tasks/get/{taskId}", new IdResponse(taskId));
        }

        private static async Task<NoContent> UpdateTask(
            Guid id,
            UpdateTaskRequest request,
            UpdateTaskHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTaskHandler.UpdateTaskCommand(
                id, request.Name, request.Description, request.Deadline, request.RemoveDeadline);
            await handler.Handle(command, cancellationToken);
            return TypedResults.NoContent();
        }

        private static async Task<NoContent> DeleteTask(
            Guid id,
            DeleteTaskHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new DeleteTaskHandler.DeleteTaskCommand(id), cancellationToken);
            return TypedResults.NoContent();
        }

        private static async Task<NoContent> ReorderTask(
            Guid id,
            ReorderRequest request,
            ReorderTaskHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new ReorderTaskHandler.ReorderTaskCommand(id, request.Index), cancellationToken);
            return TypedResults.NoContent();
        }

        private static async Task<NoContent> MoveTask(
            Guid id,
            MoveTaskRequest request,
            MoveTaskHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.Handle(new MoveTaskHandler.MoveTaskCommand(id, request.ColumnId, request.Index), cancellationToken);
            return TypedResults.NoContent();
        }
    }
}
