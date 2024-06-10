using DominandoEFCore18.Data;
using DominandoEFCore18.Data.Repositories;
using DominandoEFCore18.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =  ReferenceLoopHandling.Ignore); // Ignorando referencias ciclicas

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options
        .UseSqlServer("Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore18;Integrated Security=true;MultipleActiveResultSets=true;")
        .LogTo(Console.WriteLine)
        .EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();

builder.Services.AddScoped<IDepartamentoGenericRepository, DepartamentoGenericRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

InicializarBaseDeDados(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void InicializarBaseDeDados(IApplicationBuilder app)
{
    using var db = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (db.Database.EnsureCreated())
    {
        db.Departamentos.AddRange(Enumerable.Range(1, 10)
            .Select(p => new Departamento 
            { 
                Descricao = $"Departamento - {p}", 
                Colaboradores = Enumerable.Range(1, 10)
                    .Select(x => new Colaborador 
                    { 
                        Nome = $"Colaborador {x}/{p}" 
                    }).ToList() 
            }));
    }

    db.SaveChanges();
}