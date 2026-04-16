using Domain.Interfaces;

namespace Application.UseCases.Columns.DeleteColumn
{
    public class DeleteColumnUseCase(IColumnRepository columnRepository)
    {
        private readonly IColumnRepository columnRepository = columnRepository;

        public async Task ExecuteAsync(DeleteColumnRequest request, CancellationToken cancellationToken = default)
        {
            var column = await columnRepository.GetByIdAsync(request.ColumnId, cancellationToken);
            if (column == null)
                throw new Exception($"Колонка с id {request.ColumnId} не найдена");

            await columnRepository.DeleteAsync(request.ColumnId, cancellationToken);
        }
    }
}