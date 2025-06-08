using Microsoft.EntityFrameworkCore;
using UserProTasks.Infrastructure.Data;
using UserProTasks.Application.Interfaces;
using UserProTasks.Infrastructure.Repositories;
using UserProTasks.Application.UseCases.Projetos;
using UserProTasks.Application.UseCases.Tarefas;
using UserProTasks.Application.UseCases.Relatorios; // Novo

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Repositories
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

// Register Use Cases
builder.Services.AddScoped<CriarProjetoUseCase>();
builder.Services.AddScoped<ListarProjetosUsuarioUseCase>(); // Novo
builder.Services.AddScoped<RemoverProjetoUseCase>();        // Novo
builder.Services.AddScoped<CriarTarefaUseCase>();
builder.Services.AddScoped<ListarTarefasProjetoUseCase>();  // Novo
builder.Services.AddScoped<VisualizarTarefaUseCase>();      // Novo
builder.Services.AddScoped<AtualizarTarefaUseCase>();      // Novo
builder.Services.AddScoped<RemoverTarefaUseCase>();        // Novo
builder.Services.AddScoped<AdicionarComentarioTarefaUseCase>(); // Novo
builder.Services.AddScoped<GerarRelatorioDesempenhoUseCase>(); // Novo
builder.Services.AddScoped<ListarUsuariosDosProjetosUseCase>();

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization(); // Mantenha isso se planeja usar autenticação

app.MapControllers();
app.Run();