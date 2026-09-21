namespace Web.Endpoints
{
    public record IdResponse(Guid Id);

    /// <param name="Index">Новая позиция (с 0). Выход за границы прижимается к началу/концу.</param>
    public record ReorderRequest(int Index);
}
