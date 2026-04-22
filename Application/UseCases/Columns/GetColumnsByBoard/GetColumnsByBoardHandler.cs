using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Columns.GetColumnsByBoard
{
    public class GetColumnsByBoardHandler(IColumnRepository columnRepository)
    {
        public record GetColumnsByBoardCommand(Guid BoardId);
        private readonly IColumnRepository columnRepository = columnRepository;

        public async Task<List<Column>> Handle(GetColumnsByBoardCommand command, CancellationToken cancellationToken = default)
        {
            var columns = await columnRepository.GetByBoardAsync(command.BoardId, cancellationToken);
            
            return columns ?? throw new InvalidOperationException($"Доска с id {command.BoardId} не найдена");
        }
    }
}
