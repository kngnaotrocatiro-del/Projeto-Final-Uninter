using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;
using trabalhoUninter.DTOs;
using trabalhoUninter.Models;

namespace trabalhoUninter.Extensions
{
    public static class ProfissionalEndpoints
    {
        public static void MapProfissionalEndpoints(this WebApplication app, string sobrenome)
        {
            // ======== ENDPOINTS DE PROFISSIONAIS (CRUD COM HISTÓRICO) ========
            app.MapGet("/profissionais", GetProfissionais)
                .WithName("GetProfissionais");

            app.MapGet("/profissionais/{id:int}", GetProfissionalById)
                .WithName("GetProfissionalById");

            app.MapPost("/profissionais", CreateProfissional)
                .WithName("CreateProfissional");

            app.MapPut("/profissionais/{id:int}", UpdateProfissional)
                .WithName("UpdateProfissional");

            app.MapDelete("/profissionais/{id:int}", DeleteProfissional)
                .WithName("DeleteProfissional");
        }

        private static async Task<IResult> GetProfissionais(AppDbContext db)
        {
            return Results.Ok(await db.Profissionais.ToListAsync());
        }

        private static async Task<IResult> GetProfissionalById(int id, AppDbContext db)
        {
            return await db.Profissionais.FindAsync(id) is Profissional p
                ? Results.Ok(p)
                : Results.NotFound();
        }

        private static async Task<IResult> CreateProfissional(ProfissionalRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var nextId = (await db.Profissionais.AnyAsync())
                ? await db.Profissionais.MaxAsync(x => x.Id) + 1
                : 1;

            var profissional = new Profissional
            {
                Id = nextId,
                CPF = request.CPF,
                Nome = request.Nome
            };

            db.Profissionais.Add(profissional);
            await db.SaveChangesAsync();

            // Registra no histórico
            var historico = new ProfissionalHistorico
            {
                ProfissionalId = profissional.Id,
                CPF = profissional.CPF,
                Nome = profissional.Nome,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "INCLUSÃO"
            };
            db.ProfissionalHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Created($"/profissionais/{profissional.Id}", profissional);
        }

        private static async Task<IResult> UpdateProfissional(int id, ProfissionalRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var profissional = await db.Profissionais.FindAsync(id);
            if (profissional is null) return Results.NotFound();

            profissional.CPF = request.CPF;
            profissional.Nome = request.Nome;
            await db.SaveChangesAsync();

            // Registra no histórico
            var historico = new ProfissionalHistorico
            {
                ProfissionalId = profissional.Id,
                CPF = profissional.CPF,
                Nome = profissional.Nome,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "ALTERAÇÃO"
            };
            db.ProfissionalHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Ok(profissional);
        }

        private static async Task<IResult> DeleteProfissional(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var profissional = await db.Profissionais.FindAsync(id);
            if (profissional is null) return Results.NotFound();

            // Registra no histórico antes de deletar
            var historico = new ProfissionalHistorico
            {
                ProfissionalId = profissional.Id,
                CPF = profissional.CPF,
                Nome = profissional.Nome,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "EXCLUSÃO"
            };
            db.ProfissionalHistoricos.Add(historico);

            db.Profissionais.Remove(profissional);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }

        private static async Task<IResult> GetProfissionalHistorico(int id, AppDbContext db)
        {
            return Results.Ok(await db.ProfissionalHistoricos
                .Where(h => h.ProfissionalId == id)
                .ToListAsync());
        }
    }
}
