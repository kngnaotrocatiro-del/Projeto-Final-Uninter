using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;
using trabalhoUninter.DTOs;
using trabalhoUninter.Models;

namespace trabalhoUninter.Extensions
{
    public static class PacienteEndpoints
    {
        public static void MapPacienteEndpoints(this WebApplication app, string sobrenome)
        {
            // ======== ENDPOINTS DE PACIENTES (CRUD COM HISTÓRICO) ========
            app.MapGet("/pacientes", GetPacientes)
                .WithName("GetPacientes");

            app.MapGet("/pacientes/{id:int}", GetPacienteById)
                .WithName("GetPacienteById");

            app.MapPost("/pacientes", CreatePaciente)
                .WithName("CreatePaciente");

            app.MapPut("/pacientes/{id:int}", UpdatePaciente)
                .WithName("UpdatePaciente");

            app.MapDelete("/pacientes/{id:int}", DeletePaciente)
                .WithName("DeletePaciente");

            app.MapGet("/pacientes/historico", GetAllPacienteHistorico)
                .WithName("GetAllPacienteHistorico");

            app.MapGet("/pacientes/{id:int}/historico", GetPacienteHistorico)
                .WithName("GetPacienteHistorico");
        }

        private static async Task<IResult> GetPacientes(AppDbContext db)
        {
            return Results.Ok(await db.Pacientes.ToListAsync());
        }

        private static async Task<IResult> GetPacienteById(int id, AppDbContext db)
        {
            return await db.Pacientes.FindAsync(id) is Paciente p 
                ? Results.Ok(p) 
                : Results.NotFound();
        }

        private static async Task<IResult> CreatePaciente(PacienteRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var nextId = (await db.Pacientes.AnyAsync()) 
                ? await db.Pacientes.MaxAsync(x => x.Id) + 1 
                : 1;
            
            var paciente = new Paciente
            {
                Id = nextId,
                CPF = request.CPF,
                Nome = request.Nome,
                PlanoDeSaude = request.PlanoDeSaude
            };

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
                Usuario = sobrenome,
                Acao = "INCLUSÃO"
            };
            db.PacienteHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Created($"/pacientes/{paciente.Id}", paciente);
        }

        private static async Task<IResult> UpdatePaciente(int id, PacienteRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var paciente = await db.Pacientes.FindAsync(id);
            if (paciente is null) return Results.NotFound();

            paciente.CPF = request.CPF;
            paciente.Nome = request.Nome;
            paciente.PlanoDeSaude = request.PlanoDeSaude;
            await db.SaveChangesAsync();

            // Registra no histórico
            var historico = new PacienteHistorico
            {
                PacienteId = paciente.Id,
                CPF = paciente.CPF,
                Nome = paciente.Nome,
                PlanoDeSaude = paciente.PlanoDeSaude,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "ALTERAÇÃO"
            };
            db.PacienteHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Ok(paciente);
        }

        private static async Task<IResult> DeletePaciente(int id, AppDbContext db, string sobrenome = "Mancini")
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
                Usuario = sobrenome,
                Acao = "EXCLUSÃO"
            };
            db.PacienteHistoricos.Add(historico);

            db.Pacientes.Remove(paciente);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }

        private static async Task<IResult> GetAllPacienteHistorico(AppDbContext db)
        {
            return Results.Ok(await db.PacienteHistoricos.ToListAsync());
        }

        private static async Task<IResult> GetPacienteHistorico(int id, AppDbContext db)
        {
            return Results.Ok(await db.PacienteHistoricos
                .Where(h => h.PacienteId == id)
                .ToListAsync());
        }
    }
}
