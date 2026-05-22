using Application.Handlers.Tasks;
using Application.Handlers.Tasks.CreateTask;
using Application.Handlers.Tasks.DeleteTask;
using Application.Handlers.Tasks.MoveTask;
using Application.Handlers.Tasks.UpdateTask;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateTaskUseCase>();
            services.AddScoped<UpdateTaskHandler>();
            services.AddScoped<DeleteTaskHandler>();
            services.AddScoped<MoveTaskHandler>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                                    IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}