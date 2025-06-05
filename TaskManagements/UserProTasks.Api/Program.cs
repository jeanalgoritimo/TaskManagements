using UserProTasks.Infrastructure.Data;
using UserProTasks.Infrastructure.Repositories;
using TaskManager.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.UseCases.Projetos;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inje��o dos reposit�rios
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
//builder.Services.AddScoped<ICriarProjetoUseCase, CriarProjetoUseCase>();
//builder.Services.AddScoped<IListarProjetosUseCase, ListarProjetosUseCase>();
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();

// Inje��o dos use cases
builder.Services.AddScoped<CriarProjetoUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
