using Application.UseCases.Boards.CreateBoard;
using Application.UseCases.Boards.DeleteBoard;
using Application.UseCases.Boards.GetBoard;
using Application.UseCases.Boards.GetBoardById;
using Application.UseCases.Boards.UpdateBoard;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<CreateBoardHandler>();
builder.Services.AddScoped<GetBoardsHandler>();
builder.Services.AddScoped<GetBoardByIdHandler>();
builder.Services.AddScoped<DeleteBoardHandler>();
builder.Services.AddScoped<UpdateBoardHandler>();

builder.Services.AddOpenApi();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();