using Application.Common;
using Domain.Interfaces;

namespace Application.Handlers.Boards.GetBoardById
{
    public class GetBoardByIdHandler(IBoardRepository boardRepository, IColumnRepository columnRepository)
    {
        public record GetBoardByIdQuery(Guid BoardId);

        public async Task<BoardDetailsDto> Handle(GetBoardByIdQuery query, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIdAsync(query.BoardId, cancellationToken)
                ?? throw new NotFoundException("Доска", query.BoardId);

            var columns = await columnRepository.GetByBoardAsync(board.Id, includeTasks: true, cancellationToken);

            return BoardDetailsDto.From(board, columns);
        }
    }
}
