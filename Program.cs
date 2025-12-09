using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using trabalhoUninter.Data;
using trabalhoUninter.Extensions;

var builder = WebApplication.CreateBuilder(args);

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("PacientesDb"));

// Swagger + Basic definition
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API SGHSS - Sistema de Gestão Hospitalar", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Use seu sobrenome como usuário e seu RU como senha."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Garanta que o banco e o seed sejam criados
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona raiz para /swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

const string SOBRENOME = "Mancini";
const string RU = "4576701";

// Middleware de autenticação para endpoints protegidos
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    
    // Endpoints protegidos
    var protectedPaths = new[] { "/pacientes", "/profissionais", "/medicos", "/unidades-saude", "/hospitais", "/consultas", "/exames", "/prescricoes", "/prontuarios", "/telemedicina" };
    
    if (protectedPaths.Any(p => path.StartsWith(p)))
    {
        if (!IsAuthenticated(context, SOBRENOME, RU))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Usuário não autenticado. Contate o administrador.");
            return;
        }
    }
    await next();
});

// Registrar todos os endpoints através das extensões
app.MapPacienteEndpoints(SOBRENOME);
app.MapProfissionalEndpoints(SOBRENOME);
app.MapMedicoEndpoints(SOBRENOME);
app.MapUnidadeSaudeEndpoints(SOBRENOME);
app.MapConsultaExameEndpoints(SOBRENOME);
app.MapProntuarioTelemedinaSessaoEndpoints(SOBRENOME);

app.Run();

// Função auxiliar para validação de autenticação
static bool IsAuthenticated(HttpContext context, string expectedUser, string expectedPassword)
{
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
    {
        try
        {
            var encoded = authHeader["Basic ".Length..].Trim();
            var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var parts = decoded.Split(':', 2);
            var username = parts.ElementAtOrDefault(0);
            var password = parts.ElementAtOrDefault(1);

            return username == expectedUser && password == expectedPassword;
        }
        catch { }
    }
    return false;
}
