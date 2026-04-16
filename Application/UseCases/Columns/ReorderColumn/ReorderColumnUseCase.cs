using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Columns.ReorderColumn
{
    public class ReorderColumnUseCase(IColumnRepository columnRepository)
    {
        private readonly IColumnRepository columnRepository = columnRepository;

        public async System.Threading.Tasks.Task ExecuteAsync(ReorderColumnRequest request, CancellationToken cancellationToken = default)
        {
            List<Column>? columns = await columnRepository.GetByBoardAsync(request.BoardId, cancellationToken);
            if (columns == null || columns.Count == 0)
                throw new Exception($"Не удалось найти доску с Id {request.BoardId}");

            var column = columns.Find(column => column.Id == request.ColumnId);
            if (column == null)
                throw new Exception($"Не удалось найти колонку с Id {request.ColumnId}");

            columns.Remove(column);
            columns.Insert(request.Index, column);
        }
    }
}