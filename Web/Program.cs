using Application.Handlers.Boards.CreateBoard;
using Application.Handlers.Boards.DeleteBoard;
using Application.Handlers.Boards.GetBoard;
using Application.Handlers.Boards.GetBoardById;
using Application.Handlers.Boards.UpdateBoard;
using Application.UseCases.Columns.CreateColumn;
using Application.UseCases.Columns.DeleteColumn;
using Application.UseCases.Columns.GetColumnsByBoard;
using Application.UseCases.Columns.ReorderColumn;
using Application.UseCases.Columns.UpdateColumn;
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
builder.Services.AddScoped<CreateColumnHandler>();
builder.Services.AddScoped<UpdateColumnHandler>();
builder.Services.AddScoped<DeleteColumnHandler>();
builder.Services.AddScoped<GetColumnsByBoardHandler>();
builder.Services.AddScoped<ReorderColumnHandler>();

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