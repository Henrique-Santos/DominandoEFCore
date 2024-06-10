using DominandoEFCore17.Data;
using DominandoEFCore17.Domain;
using DominandoEFCore17.Extensions;
using DominandoEFCore17.Interceptors;
using DominandoEFCore17.Middlewares;
using DominandoEFCore17.ModelFactory;
using DominandoEFCore17.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TenantData>();

builder.Services.AddScoped<StrategySchemaInterceptor>();

// ESTRATEGIA COM: IDENTIFICADOR NA TABELA
//builder.Services
//    .AddDbContext<ApplicationDbContext>(options =>
//    {
//        options
//            .UseSqlServer("Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore17;Integrated Security=true;MultipleActiveResultSets=true;")
//            .LogTo(Console.WriteLine)
//            .EnableSensitiveDataLogging();
//    });

// ESTRATEGIA COM: SCHEMA
//builder.Services
//    .AddDbContext<ApplicationDbContext>((provider, options) =>
//    {
//        options
//            .UseSqlServer("Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore17;Integrated Security=true;MultipleActiveResultSets=true;")
//            .LogTo(Console.WriteLine)
//            .EnableSensitiveDataLogging();

//        var interceptor = provider.GetRequiredService<StrategySchemaInterceptor>();

//        options.AddInterceptors(interceptor);
//    });

//builder.Services
//    .AddDbContext<ApplicationDbContext>(options =>
//    {
//        options
//            .UseSqlServer("Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore17;Integrated Security=true;MultipleActiveResultSets=true;")
//            .LogTo(Console.WriteLine)
//            .ReplaceService<IModelCacheKeyFactory, StrategySchemaModelCacheKey>()
//            .EnableSensitiveDataLogging();
//    });

// ESTRATEGIA COM: BANCO DE DADOS
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddScoped<ApplicationDbContext>(provider =>
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
        var tenantId = httpContext?.GetTenantId();

        //var connectionString = Configuration.GetConnectionString(tenantId);
        var connectionString = builder.Configuration.GetConnectionString("custom").Replace("_DATABASE_", tenantId);

        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging();

        return new ApplicationDbContext(optionsBuilder.Options);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//DatabaseInitialize(app);

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();

void DatabaseInitialize(IApplicationBuilder app)
{
    using var db = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    for (int i = 0; i <= 5; i++)
    {
        db.Persons.Add(new Person { Name = $"Person {i}" });
        db.Products.Add(new Product { Description = $"Product {i}" });
    }

    db.SaveChanges();
}