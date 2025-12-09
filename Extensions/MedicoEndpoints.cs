using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;
using trabalhoUninter.DTOs;
using trabalhoUninter.Models;

namespace trabalhoUninter.Extensions
{
    public static class MedicoEndpoints
    {
        public static void MapMedicoEndpoints(this WebApplication app, string sobrenome)
        {
            // ======== ENDPOINTS DE MÉDICOS (CRUD COM HISTÓRICO) ========
            app.MapGet("/medicos", GetMedicos)
                .WithName("GetMedicos");

            app.MapGet("/medicos/{id:int}", GetMedicoById)
                .WithName("GetMedicoById");

            app.MapPost("/medicos", CreateMedico)
                .WithName("CreateMedico");

            app.MapPut("/medicos/{id:int}", UpdateMedico)
                .WithName("UpdateMedico");

            app.MapDelete("/medicos/{id:int}", DeleteMedico)
                .WithName("DeleteMedico");
        }

        private static async Task<IResult> GetMedicos(AppDbContext db)
        {
            return Results.Ok(await db.Medicos.ToListAsync());
        }

        private static async Task<IResult> GetMedicoById(int id, AppDbContext db)
        {
            return await db.Medicos.FindAsync(id) is Medico m
                ? Results.Ok(m)
                : Results.NotFound();
        }

        private static async Task<IResult> CreateMedico(MedicoRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var nextId = (await db.Medicos.AnyAsync())
                ? await db.Medicos.MaxAsync(x => x.Id) + 1
                : 1;

            var medico = new Medico
            {
                Id = nextId,
                CPF = request.CPF,
                CRM = request.CRM,
                Nome = request.Nome,
                Especialidade = request.Especialidade
            };

            db.Medicos.Add(medico);
            await db.SaveChangesAsync();

            // Registra no histórico
            var historico = new MedicoHistorico
            {
                MedicoId = medico.Id,
                CPF = medico.CPF,
                CRM = medico.CRM,
                Nome = medico.Nome,
                Especialidade = medico.Especialidade,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "INCLUSÃO"
            };
            db.MedicoHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Created($"/medicos/{medico.Id}", medico);
        }

        private static async Task<IResult> UpdateMedico(int id, MedicoRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var medico = await db.Medicos.FindAsync(id);
            if (medico is null) return Results.NotFound();

            medico.CPF = request.CPF;
            medico.CRM = request.CRM;
            medico.Nome = request.Nome;
            medico.Especialidade = request.Especialidade;
            await db.SaveChangesAsync();

            // Registra no histórico
            var historico = new MedicoHistorico
            {
                MedicoId = medico.Id,
                CPF = medico.CPF,
                CRM = medico.CRM,
                Nome = medico.Nome,
                Especialidade = medico.Especialidade,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "ALTERAÇÃO"
            };
            db.MedicoHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Ok(medico);
        }

        private static async Task<IResult> DeleteMedico(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var medico = await db.Medicos.FindAsync(id);
            if (medico is null) return Results.NotFound();

            // Registra no histórico antes de deletar
            var historico = new MedicoHistorico
            {
                MedicoId = medico.Id,
                CPF = medico.CPF,
                CRM = medico.CRM,
                Nome = medico.Nome,
                Especialidade = medico.Especialidade,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "EXCLUSÃO"
            };
            db.MedicoHistoricos.Add(historico);

            db.Medicos.Remove(medico);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }
    }
}
