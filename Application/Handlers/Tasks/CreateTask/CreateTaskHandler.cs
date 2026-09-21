using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Tasks.CreateTask
{
    public class CreateTaskHandler(IColumnRepository columnRepository, IUnitOfWork unitOfWork)
    {
        public record CreateTaskCommand(Guid ColumnId, string Name, string? Description, DateTime? Deadline);

        public async Task<Guid> Handle(CreateTaskCommand command, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
                ?? throw new NotFoundException("Колонка", command.ColumnId);

            var task = column.AddTask(command.Name, command.Description, command.Deadline);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
