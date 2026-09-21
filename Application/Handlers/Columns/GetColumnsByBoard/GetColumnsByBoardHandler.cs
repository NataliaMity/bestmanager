using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Columns.GetColumnsByBoard
{
    public class GetColumnsByBoardHandler(IBoardRepository boardRepository, IColumnRepository columnRepository)
    {
        public record GetColumnsByBoardQuery(Guid BoardId);

        public async Task<List<ColumnDto>> Handle(GetColumnsByBoardQuery query, CancellationToken cancellationToken = default)
        {
            _ = await boardRepository.GetByIdAsync(query.BoardId, cancellationToken)
                ?? throw new NotFoundException("Доска", query.BoardId);

            var columns = await columnRepository.GetByBoardAsync(query.BoardId, cancellationToken: cancellationToken);
            return columns.Select(ColumnDto.From).ToList();
        }
    }
}
