using Application;
using Infrastructure;
using Web.Endpoints;
using Web.ErrorHandling;

var builder = WebApplication.CreateBuilder(args);

// Все *Handler из Application регистрируются автоматически — руками добавлять не нужно
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapBoardEndpoints();
app.MapColumnEndpoints();
app.MapTaskEndpoints();

app.Run();
