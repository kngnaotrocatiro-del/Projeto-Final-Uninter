using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using trabalhoUninter.Data;
using trabalhoUninter.Models;

var builder = WebApplication.CreateBuilder(args);

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("PacientesDb"));

// Swagger + Basic definition
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Pacientes", Version = "v1" });
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

app.UseHttpsRedirection();


const string SOBRENOME = "Mancini";
const string RU = "4576701";


app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/pacientes"))
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

                if (username == SOBRENOME && password == RU)
                {
                    await next();
                    return;
                }
            }
            catch { }
        }
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Não autorizado. Use seu sobrenome e RU.");
        return;
    }
    await next();
});


// ======== ENDPOINTS DE PACIENTES (CRUD COM HISTÓRICO) ========
app.MapGet("/pacientes", async (AppDbContext db) => await db.Pacientes.ToListAsync())
.WithName("GetPacientes");

app.MapGet("/pacientes/{id:int}", async (int id, AppDbContext db) =>
    await db.Pacientes.FindAsync(id) is Paciente p ? Results.Ok(p) : Results.NotFound())
.WithName("GetPacienteById");

app.MapPost("/pacientes", async (Paciente paciente, AppDbContext db) =>
{
    if (paciente.Id == 0)
    {
        var nextId = (await db.Pacientes.AnyAsync()) ? await db.Pacientes.MaxAsync(x => x.Id) + 1 : 1;
        paciente.Id = nextId;
    }
    
    db.Pacientes.Add(paciente);
    await db.SaveChangesAsync();
    
    // Registra no histórico
    var historico = new PacienteHistorico
    {
        PacienteId = paciente.Id,
        CPF = paciente.CPF,
        Nome = paciente.Nome,
        PlanoDeSaude = paciente.PlanoDeSaude,
        Timestamp = DateTime.Now,
        Usuario = SOBRENOME,
        Acao = "INCLUSÃO"
    };
    db.PacienteHistoricos.Add(historico);
    await db.SaveChangesAsync();
    
    return Results.Created($"/pacientes/{paciente.Id}", paciente);
})
.WithName("CreatePaciente");

app.MapPut("/pacientes/{id:int}", async (int id, Paciente dados, AppDbContext db) =>
{
    var paciente = await db.Pacientes.FindAsync(id);
    if (paciente is null) return Results.NotFound();
    
    paciente.CPF = dados.CPF;
    paciente.Nome = dados.Nome;
    paciente.PlanoDeSaude = dados.PlanoDeSaude;
    await db.SaveChangesAsync();
    
    // Registra no histórico
    var historico = new PacienteHistorico
    {
        PacienteId = paciente.Id,
        CPF = paciente.CPF,
        Nome = paciente.Nome,
        PlanoDeSaude = paciente.PlanoDeSaude,
        Timestamp = DateTime.Now,
        Usuario = SOBRENOME,
        Acao = "ALTERAÇÃO"
    };
    db.PacienteHistoricos.Add(historico);
    await db.SaveChangesAsync();
    
    return Results.Ok(paciente);
})
.WithName("UpdatePaciente");

app.MapDelete("/pacientes/{id:int}", async (int id, AppDbContext db) =>
{
    var paciente = await db.Pacientes.FindAsync(id);
    if (paciente is null) return Results.NotFound();
    
    // Registra no histórico antes de deletar
    var historico = new PacienteHistorico
    {
        PacienteId = paciente.Id,
        CPF = paciente.CPF,
        Nome = paciente.Nome,
        PlanoDeSaude = paciente.PlanoDeSaude,
        Timestamp = DateTime.Now,
        Usuario = SOBRENOME,
        Acao = "EXCLUSÃO"
    };
    db.PacienteHistoricos.Add(historico);
    
    db.Pacientes.Remove(paciente);
    await db.SaveChangesAsync();
    
    return Results.NoContent();
})
.WithName("DeletePaciente");

// ======== ENDPOINT DE HISTÓRICO ========
app.MapGet("/pacientes/{id:int}/historico", async (int id, AppDbContext db) =>
    await db.PacienteHistoricos.Where(h => h.PacienteId == id).ToListAsync())
.WithName("GetPacienteHistorico");

app.Run();
