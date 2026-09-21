using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует все классы *Handler из сборки Application — новый хендлер не нужно добавлять руками.
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var handlers = typeof(DependencyInjection).Assembly
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true }
                            && t.Name.EndsWith("Handler", StringComparison.Ordinal));

            foreach (var handler in handlers)
                services.AddScoped(handler);

            return services;
        }
    }
}
