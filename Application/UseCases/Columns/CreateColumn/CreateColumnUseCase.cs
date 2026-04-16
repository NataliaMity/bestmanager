using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Columns.CreateColumn
{
    public class CreateColumnUseCase(IColumnRepository columnRepository,
                                     IBoardRepository boardRepository)
    {
        private readonly IColumnRepository columnRepository = columnRepository;
        private readonly IBoardRepository boardRepository = boardRepository;

        public async Task<CreateColumnResponse> ExecuteAsync(CreateColumnRequest request, CancellationToken cancellationToken = default)
        {
            var board = await boardRepository.GetByIDAsync(request.BoardId);
            if (board == null)
                throw new Exception($"Доска с id {request.BoardId} не найдена");

            var column = new Column(request.Name,  board);
            await columnRepository.AddAsync(column);

            return new CreateColumnResponse(column.Id); 
        }
    }
}