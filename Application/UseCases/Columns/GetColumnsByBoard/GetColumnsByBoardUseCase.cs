using Domain.Interfaces;

namespace Application.UseCases.Columns.GetColumnsByBoard
{
    public class GetColumnsByBoardUseCase(IColumnRepository columnRepository)
    {
        private readonly IColumnRepository columnRepository = columnRepository;

        public async Task<GetColumnsByBoardResponse?> ExecuteAsync(GetColumnsByBoardRequest request, CancellationToken cancellationToken = default)
        {
            var columns = await columnRepository.GetByBoardAsync(request.BoardId, cancellationToken);
            
            return columns == null
                ? throw new Exception($"Доска с id {request.BoardId} не найдена")
                : new GetColumnsByBoardResponse(columns);
        }
    }
}
