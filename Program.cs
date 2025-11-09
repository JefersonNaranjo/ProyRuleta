using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Ruleta",
        Version = "v1",
        Description = "API para gestión de ruletas y apuestas"
    });

    c.AddSecurityDefinition("IdUsuarioHeader", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "IdUsuario",
        Type = SecuritySchemeType.ApiKey,
        Description = "Identificador del usuario (por ejemplo: usuario123)"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "IdUsuarioHeader"
                }
            },
            new string[] {}
        }
    });
});

// Conexión a SQL Server
builder.Services.AddDbContext<CasinoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias
builder.Services.AddScoped<RuletaService>();

var app = builder.Build();

// Configuración del middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Ruleta v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CasinoDbContext>();
    try
    {
        db.Database.CanConnect();
        Console.WriteLine("Conectado correctamente a la base de datos.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
    }
}


app.Run();
