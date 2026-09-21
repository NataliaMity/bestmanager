using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.GetTasksByColumn
{
    public class GetTasksByColumnHandler(IColumnRepository columnRepository, ITaskRepository taskRepository)
    {
        public record GetTasksByColumnQuery(Guid ColumnId);

        public async Task<List<TaskDto>> Handle(GetTasksByColumnQuery query, CancellationToken cancellationToken = default)
        {
            _ = await columnRepository.GetByIdAsync(query.ColumnId, cancellationToken)
                ?? throw new NotFoundException("Колонка", query.ColumnId);

            var tasks = await taskRepository.GetByColumnAsync(query.ColumnId, cancellationToken);
            return tasks.Select(TaskDto.From).ToList();
        }
    }
}
