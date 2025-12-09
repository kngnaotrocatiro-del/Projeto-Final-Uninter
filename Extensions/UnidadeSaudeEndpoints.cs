using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;
using trabalhoUninter.DTOs;
using trabalhoUninter.Models;

namespace trabalhoUninter.Extensions
{
    public static class UnidadeSaudeEndpoints
    {
        public static void MapUnidadeSaudeEndpoints(this WebApplication app, string sobrenome)
        {
            // ======== ENDPOINTS DE UNIDADES DE SAÚDE (CRUD COM HISTÓRICO) ========
            app.MapGet("/unidades-saude", GetUnidadesSaude)
                .WithName("GetUnidadesSaude");

            app.MapGet("/unidades-saude/{id:int}", GetUnidadeSaudeById)
                .WithName("GetUnidadeSaudeById");

            app.MapPost("/unidades-saude", CreateUnidadeSaude)
                .WithName("CreateUnidadeSaude");

            app.MapPut("/unidades-saude/{id:int}", UpdateUnidadeSaude)
                .WithName("UpdateUnidadeSaude");

            app.MapDelete("/unidades-saude/{id:int}", DeleteUnidadeSaude)
                .WithName("DeleteUnidadeSaude");

            // ======== ENDPOINTS DE HOSPITAIS (CRUD COM HISTÓRICO) ========
            app.MapGet("/hospitais", GetHospitais)
                .WithName("GetHospitais");

            app.MapGet("/hospitais/{id:int}", GetHospitalById)
                .WithName("GetHospitalById");

            app.MapPost("/hospitais", CreateHospital)
                .WithName("CreateHospital");

            app.MapPut("/hospitais/{id:int}", UpdateHospital)
                .WithName("UpdateHospital");

            app.MapDelete("/hospitais/{id:int}", DeleteHospital)
                .WithName("DeleteHospital");
        }

        // ===== UNIDADES DE SAÚDE =====
        private static async Task<IResult> GetUnidadesSaude(AppDbContext db)
        {
            return Results.Ok(await db.UnidadesSaude.ToListAsync());
        }

        private static async Task<IResult> GetUnidadeSaudeById(int id, AppDbContext db)
        {
            return await db.UnidadesSaude.FindAsync(id) is UnidadeSaude u
                ? Results.Ok(u)
                : Results.NotFound();
        }

        private static async Task<IResult> CreateUnidadeSaude(UnidadeSaudeRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var nextId = (await db.UnidadesSaude.AnyAsync())
                ? await db.UnidadesSaude.MaxAsync(x => x.Id) + 1
                : 1;

            var unidade = new UnidadeSaude
            {
                Id = nextId,
                Tipo = request.Tipo,
                CNPJ = request.CNPJ,
                Nome = request.Nome
            };

            db.UnidadesSaude.Add(unidade);
            await db.SaveChangesAsync();

            var historico = new UnidadeSaudeHistorico
            {
                UnidadeSaudeId = unidade.Id,
                Tipo = unidade.Tipo,
                CNPJ = unidade.CNPJ,
                Nome = unidade.Nome,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "INCLUSÃO"
            };
            db.UnidadesSaudeHistorico.Add(historico);
            await db.SaveChangesAsync();

            return Results.Created($"/unidades-saude/{unidade.Id}", unidade);
        }

        private static async Task<IResult> UpdateUnidadeSaude(int id, UnidadeSaudeRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var unidade = await db.UnidadesSaude.FindAsync(id);
            if (unidade is null) return Results.NotFound();

            unidade.Tipo = request.Tipo;
            unidade.CNPJ = request.CNPJ;
            unidade.Nome = request.Nome;
            await db.SaveChangesAsync();

            var historico = new UnidadeSaudeHistorico
            {
                UnidadeSaudeId = unidade.Id,
                Tipo = unidade.Tipo,
                CNPJ = unidade.CNPJ,
                Nome = unidade.Nome,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "ALTERAÇÃO"
            };
            db.UnidadesSaudeHistorico.Add(historico);
            await db.SaveChangesAsync();

            return Results.Ok(unidade);
        }

        private static async Task<IResult> DeleteUnidadeSaude(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var unidade = await db.UnidadesSaude.FindAsync(id);
            if (unidade is null) return Results.NotFound();

            var historico = new UnidadeSaudeHistorico
            {
                UnidadeSaudeId = unidade.Id,
                Tipo = unidade.Tipo,
                CNPJ = unidade.CNPJ,
                Nome = unidade.Nome,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "EXCLUSÃO"
            };
            db.UnidadesSaudeHistorico.Add(historico);

            db.UnidadesSaude.Remove(unidade);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }



        // ===== HOSPITAIS =====
        private static async Task<IResult> GetHospitais(AppDbContext db)
        {
            return Results.Ok(await db.Hospitais.ToListAsync());
        }

        private static async Task<IResult> GetHospitalById(int id, AppDbContext db)
        {
            return await db.Hospitais.FindAsync(id) is Hospital h
                ? Results.Ok(h)
                : Results.NotFound();
        }

        private static async Task<IResult> CreateHospital(HospitalRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var nextId = (await db.Hospitais.AnyAsync())
                ? await db.Hospitais.MaxAsync(x => x.Id) + 1
                : 1;

            var hospital = new Hospital
            {
                Id = nextId,
                Nome = request.Nome,
                NumeroLeitos = request.NumeroLeitos,
                NumeroProfissionais = request.NumeroProfissionais,
                NumeroSuprimentos = request.NumeroSuprimentos,
                Gastos = request.Gastos,
                Lucros = request.Lucros,
                DiaAtualizacao = request.DiaAtualizacao
            };

            db.Hospitais.Add(hospital);
            await db.SaveChangesAsync();

            var historico = new HospitalHistorico
            {
                HospitalId = hospital.Id,
                Nome = hospital.Nome,
                NumeroLeitos = hospital.NumeroLeitos,
                NumeroProfissionais = hospital.NumeroProfissionais,
                NumeroSuprimentos = hospital.NumeroSuprimentos,
                Gastos = hospital.Gastos,
                Lucros = hospital.Lucros,
                DiaAtualizacao = hospital.DiaAtualizacao,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "INCLUSÃO"
            };
            db.HospitalHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Created($"/hospitais/{hospital.Id}", hospital);
        }

        private static async Task<IResult> UpdateHospital(int id, HospitalRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var hospital = await db.Hospitais.FindAsync(id);
            if (hospital is null) return Results.NotFound();

            hospital.Nome = request.Nome;
            hospital.NumeroLeitos = request.NumeroLeitos;
            hospital.NumeroProfissionais = request.NumeroProfissionais;
            hospital.NumeroSuprimentos = request.NumeroSuprimentos;
            hospital.Gastos = request.Gastos;
            hospital.Lucros = request.Lucros;
            hospital.DiaAtualizacao = request.DiaAtualizacao;
            await db.SaveChangesAsync();

            var historico = new HospitalHistorico
            {
                HospitalId = hospital.Id,
                Nome = hospital.Nome,
                NumeroLeitos = hospital.NumeroLeitos,
                NumeroProfissionais = hospital.NumeroProfissionais,
                NumeroSuprimentos = hospital.NumeroSuprimentos,
                Gastos = hospital.Gastos,
                Lucros = hospital.Lucros,
                DiaAtualizacao = hospital.DiaAtualizacao,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "ALTERAÇÃO"
            };
            db.HospitalHistoricos.Add(historico);
            await db.SaveChangesAsync();

            return Results.Ok(hospital);
        }

        private static async Task<IResult> DeleteHospital(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var hospital = await db.Hospitais.FindAsync(id);
            if (hospital is null) return Results.NotFound();

            var historico = new HospitalHistorico
            {
                HospitalId = hospital.Id,
                Nome = hospital.Nome,
                NumeroLeitos = hospital.NumeroLeitos,
                NumeroProfissionais = hospital.NumeroProfissionais,
                NumeroSuprimentos = hospital.NumeroSuprimentos,
                Gastos = hospital.Gastos,
                Lucros = hospital.Lucros,
                DiaAtualizacao = hospital.DiaAtualizacao,
                Timestamp = DateTime.Now,
                Usuario = sobrenome,
                Acao = "EXCLUSÃO"
            };
            db.HospitalHistoricos.Add(historico);

            db.Hospitais.Remove(hospital);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }


    }
}
