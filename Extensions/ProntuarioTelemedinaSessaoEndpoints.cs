using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;
using trabalhoUninter.DTOs;
using trabalhoUninter.Models;

namespace trabalhoUninter.Extensions
{
    public static class ProntuarioTelemedinaSessaoEndpoints
    {
        public static void MapProntuarioTelemedinaSessaoEndpoints(this WebApplication app, string sobrenome)
        {
            // ======== ENDPOINTS DE PRONTUÁRIOS (CRUD COM HISTÓRICO) ========
            app.MapGet("/prontuarios", GetProntuarios).WithName("GetProntuarios");
            app.MapGet("/prontuarios/{id:int}", GetProntuarioById).WithName("GetProntuarioById");
            app.MapPost("/prontuarios", CreateProntuario).WithName("CreateProntuario");
            app.MapPut("/prontuarios/{id:int}", UpdateProntuario).WithName("UpdateProntuario");
            app.MapDelete("/prontuarios/{id:int}", DeleteProntuario).WithName("DeleteProntuario");
            app.MapGet("/prontuarios/{id:int}/historico", GetProntuarioHistorico).WithName("GetProntuarioHistorico");

            // ======== ENDPOINTS DE TELEMEDICINA (CRUD COM HISTÓRICO) ========
            app.MapGet("/telemedicina", GetTelemedinaSessoes).WithName("GetTelemedinaSessoes");
            app.MapGet("/telemedicina/{id:int}", GetTelemedinaSessaoById).WithName("GetTelemedinaSessaoById");
            app.MapPost("/telemedicina", CreateTelemedinaSessao).WithName("CreateTelemedinaSessao");
            app.MapPut("/telemedicina/{id:int}", UpdateTelemedinaSessao).WithName("UpdateTelemedinaSessao");
            app.MapDelete("/telemedicina/{id:int}", DeleteTelemedinaSessao).WithName("DeleteTelemedinaSessao");
            app.MapGet("/telemedicina/{id:int}/historico", GetTelemedinaSessaoHistorico).WithName("GetTelemedinaSessaoHistorico");
        }

        // ===== PRONTUÁRIOS =====
        private static async Task<IResult> GetProntuarios(AppDbContext db) => Results.Ok(await db.Prontuarios.ToListAsync());
        private static async Task<IResult> GetProntuarioById(int id, AppDbContext db) =>
            await db.Prontuarios.FindAsync(id) is Prontuario pr ? Results.Ok(pr) : Results.NotFound();

        private static async Task<IResult> CreateProntuario(ProntuarioRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var prontuarioId = (await db.Prontuarios.AnyAsync()) ? await db.Prontuarios.MaxAsync(x => x.Id) + 1 : 1;
            
            var prontuario = new Prontuario
            {
                Id = prontuarioId,
                IdConsulta = request.IdConsulta,
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                Texto = request.Texto
            };

            db.Prontuarios.Add(prontuario);
            await db.SaveChangesAsync();

            var historico = new ProntuarioHistorico
            {
                ProntuarioId = prontuario.Id, IdConsulta = prontuario.IdConsulta,
                IdPaciente = prontuario.IdPaciente, IdMedico = prontuario.IdMedico, Texto = prontuario.Texto,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "INCLUSÃO"
            };
            db.ProntuarioHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Created($"/prontuarios/{prontuario.Id}", prontuario);
        }

        private static async Task<IResult> UpdateProntuario(int id, ProntuarioRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var prontuario = await db.Prontuarios.FindAsync(id);
            if (prontuario is null) return Results.NotFound();

            prontuario.IdConsulta = request.IdConsulta;
            prontuario.IdPaciente = request.IdPaciente;
            prontuario.IdMedico = request.IdMedico;
            prontuario.Texto = request.Texto;
            await db.SaveChangesAsync();

            var historico = new ProntuarioHistorico
            {
                ProntuarioId = prontuario.Id, IdConsulta = prontuario.IdConsulta,
                IdPaciente = prontuario.IdPaciente, IdMedico = prontuario.IdMedico, Texto = prontuario.Texto,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "ALTERAÇÃO"
            };
            db.ProntuarioHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Ok(prontuario);
        }

        private static async Task<IResult> DeleteProntuario(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var prontuario = await db.Prontuarios.FindAsync(id);
            if (prontuario is null) return Results.NotFound();

            var historico = new ProntuarioHistorico
            {
                ProntuarioId = prontuario.Id, IdConsulta = prontuario.IdConsulta,
                IdPaciente = prontuario.IdPaciente, IdMedico = prontuario.IdMedico, Texto = prontuario.Texto,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "EXCLUSÃO"
            };
            db.ProntuarioHistoricos.Add(historico);
            db.Prontuarios.Remove(prontuario);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> GetProntuarioHistorico(int id, AppDbContext db) =>
            Results.Ok(await db.ProntuarioHistoricos.Where(h => h.ProntuarioId == id).ToListAsync());

        // ===== TELEMEDICINA SESSÕES =====
        private static async Task<IResult> GetTelemedinaSessoes(AppDbContext db) => Results.Ok(await db.TelemedinaSessoes.ToListAsync());
        private static async Task<IResult> GetTelemedinaSessaoById(int id, AppDbContext db) =>
            await db.TelemedinaSessoes.FindAsync(id) is TelemedinaSessao t ? Results.Ok(t) : Results.NotFound();

        private static async Task<IResult> CreateTelemedinaSessao(TelemedinaSessaoRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var sessaoId = (await db.TelemedinaSessoes.AnyAsync()) ? await db.TelemedinaSessoes.MaxAsync(x => x.Id) + 1 : 1;
            
            var sessao = new TelemedinaSessao
            {
                Id = sessaoId,
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdConsulta = request.IdConsulta,
                LinkVideo = request.LinkVideo
            };

            db.TelemedinaSessoes.Add(sessao);
            await db.SaveChangesAsync();

            var historico = new TelemedinaSessaoHistorico
            {
                TelemedinaSessaoId = sessao.Id, IdPaciente = sessao.IdPaciente, IdMedico = sessao.IdMedico,
                IdConsulta = sessao.IdConsulta, LinkVideo = sessao.LinkVideo,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "INCLUSÃO"
            };
            db.TelemedinaSessaoHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Created($"/telemedicina/{sessao.Id}", sessao);
        }

        private static async Task<IResult> UpdateTelemedinaSessao(int id, TelemedinaSessaoRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var sessao = await db.TelemedinaSessoes.FindAsync(id);
            if (sessao is null) return Results.NotFound();

            sessao.IdPaciente = request.IdPaciente;
            sessao.IdMedico = request.IdMedico;
            sessao.IdConsulta = request.IdConsulta;
            sessao.LinkVideo = request.LinkVideo;
            await db.SaveChangesAsync();

            var historico = new TelemedinaSessaoHistorico
            {
                TelemedinaSessaoId = sessao.Id, IdPaciente = sessao.IdPaciente, IdMedico = sessao.IdMedico,
                IdConsulta = sessao.IdConsulta, LinkVideo = sessao.LinkVideo,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "ALTERAÇÃO"
            };
            db.TelemedinaSessaoHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Ok(sessao);
        }

        private static async Task<IResult> DeleteTelemedinaSessao(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var sessao = await db.TelemedinaSessoes.FindAsync(id);
            if (sessao is null) return Results.NotFound();

            var historico = new TelemedinaSessaoHistorico
            {
                TelemedinaSessaoId = sessao.Id, IdPaciente = sessao.IdPaciente, IdMedico = sessao.IdMedico,
                IdConsulta = sessao.IdConsulta, LinkVideo = sessao.LinkVideo,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "EXCLUSÃO"
            };
            db.TelemedinaSessaoHistoricos.Add(historico);
            db.TelemedinaSessoes.Remove(sessao);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> GetTelemedinaSessaoHistorico(int id, AppDbContext db) =>
            Results.Ok(await db.TelemedinaSessaoHistoricos.Where(h => h.TelemedinaSessaoId == id).ToListAsync());
    }
}
