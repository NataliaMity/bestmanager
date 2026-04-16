using Domain.Interfaces;

namespace Application.UseCases.Columns.UpdateColumn
{
    public class UpdateColumnUseCase(IColumnRepository columnRepository)
    {
        private readonly IColumnRepository columnRepository = columnRepository;

        public async Task ExecuteAsync(UpdateColumnRequest request, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(request.ColumnId, cancellationToken) 
                ?? throw new Exception($"Не удалось найти задачу с Id {request.ColumnId}");

            if (request.Name is not null)
                column.UpdateName(request.Name);

            await columnRepository.UpdateAsync(column.Id, cancellationToken);
        }
    }
}