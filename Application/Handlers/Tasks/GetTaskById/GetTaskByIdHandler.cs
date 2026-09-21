using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.GetTaskById
{
    public class GetTaskByIdHandler(ITaskRepository taskRepository)
    {
        public record GetTaskByIdQuery(Guid TaskId);

        public async Task<TaskDto> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken = default)
        {
            var task = await taskRepository.GetByIdAsync(query.TaskId, cancellationToken)
                ?? throw new NotFoundException("Задача", query.TaskId);

            return TaskDto.From(task);
        }
    }
}
