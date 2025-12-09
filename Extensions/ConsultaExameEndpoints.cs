using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;
using trabalhoUninter.DTOs;
using trabalhoUninter.Models;

namespace trabalhoUninter.Extensions
{
    public static class ConsultaExameEndpoints
    {
        public static void MapConsultaExameEndpoints(this WebApplication app, string sobrenome)
        {
            // ======== ENDPOINTS DE CONSULTAS (CRUD COM HISTÓRICO) ========
            app.MapGet("/consultas", GetConsultas).WithName("GetConsultas");
            app.MapGet("/consultas/{id:int}", GetConsultaById).WithName("GetConsultaById");
            app.MapPost("/consultas", CreateConsulta).WithName("CreateConsulta");
            app.MapPut("/consultas/{id:int}", UpdateConsulta).WithName("UpdateConsulta");
            app.MapDelete("/consultas/{id:int}", DeleteConsulta).WithName("DeleteConsulta");
            app.MapGet("/consultas/{id:int}/historico", GetConsultaHistorico).WithName("GetConsultaHistorico");

            // ======== ENDPOINTS DE EXAMES (CRUD COM HISTÓRICO) ========
            app.MapGet("/exames", GetExames).WithName("GetExames");
            app.MapGet("/exames/{id:int}", GetExameById).WithName("GetExameById");
            app.MapPost("/exames", CreateExame).WithName("CreateExame");
            app.MapPut("/exames/{id:int}", UpdateExame).WithName("UpdateExame");
            app.MapDelete("/exames/{id:int}", DeleteExame).WithName("DeleteExame");
            app.MapGet("/exames/{id:int}/historico", GetExameHistorico).WithName("GetExameHistorico");

            // ======== ENDPOINTS DE PRESCRIÇÕES (CRUD COM HISTÓRICO) ========
            app.MapGet("/prescricoes", GetPrescricoes).WithName("GetPrescricoes");
            app.MapGet("/prescricoes/{id:int}", GetPrescricaoById).WithName("GetPrescricaoById");
            app.MapPost("/prescricoes", CreatePrescricao).WithName("CreatePrescricao");
            app.MapPut("/prescricoes/{id:int}", UpdatePrescricao).WithName("UpdatePrescricao");
            app.MapDelete("/prescricoes/{id:int}", DeletePrescricao).WithName("DeletePrescricao");
            app.MapGet("/prescricoes/{id:int}/historico", GetPrescricaoHistorico).WithName("GetPrescricaoHistorico");
        }

        // ===== CONSULTAS =====
        private static async Task<IResult> GetConsultas(AppDbContext db) => Results.Ok(await db.Consultas.ToListAsync());
        private static async Task<IResult> GetConsultaById(int id, AppDbContext db) =>
            await db.Consultas.FindAsync(id) is Consulta c ? Results.Ok(c) : Results.NotFound();

        private static async Task<IResult> CreateConsulta(ConsultaRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var consultaId = (await db.Consultas.AnyAsync()) ? await db.Consultas.MaxAsync(x => x.Id) + 1 : 1;
            
            var consulta = new Consulta
            {
                Id = consultaId,
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdUnidade = request.IdUnidade,
                Tipo = request.Tipo,
                Data = request.Data,
                Horario = request.Horario
            };

            db.Consultas.Add(consulta);
            await db.SaveChangesAsync();

            var historico = new ConsultaHistorico
            {
                ConsultaId = consulta.Id, IdPaciente = consulta.IdPaciente, IdMedico = consulta.IdMedico,
                IdUnidade = consulta.IdUnidade, Tipo = consulta.Tipo, Data = consulta.Data, Horario = consulta.Horario,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "INCLUSÃO"
            };
            db.ConsultaHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Created($"/consultas/{consulta.Id}", consulta);
        }

        private static async Task<IResult> UpdateConsulta(int id, ConsultaRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var consulta = await db.Consultas.FindAsync(id);
            if (consulta is null) return Results.NotFound();

            consulta.IdPaciente = request.IdPaciente;
            consulta.IdMedico = request.IdMedico;
            consulta.IdUnidade = request.IdUnidade;
            consulta.Tipo = request.Tipo;
            consulta.Data = request.Data;
            consulta.Horario = request.Horario;
            await db.SaveChangesAsync();

            var historico = new ConsultaHistorico
            {
                ConsultaId = consulta.Id, IdPaciente = consulta.IdPaciente, IdMedico = consulta.IdMedico,
                IdUnidade = consulta.IdUnidade, Tipo = consulta.Tipo, Data = consulta.Data, Horario = consulta.Horario,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "ALTERAÇÃO"
            };
            db.ConsultaHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Ok(consulta);
        }

        private static async Task<IResult> DeleteConsulta(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var consulta = await db.Consultas.FindAsync(id);
            if (consulta is null) return Results.NotFound();

            var historico = new ConsultaHistorico
            {
                ConsultaId = consulta.Id, IdPaciente = consulta.IdPaciente, IdMedico = consulta.IdMedico,
                IdUnidade = consulta.IdUnidade, Tipo = consulta.Tipo, Data = consulta.Data, Horario = consulta.Horario,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "EXCLUSÃO"
            };
            db.ConsultaHistoricos.Add(historico);
            db.Consultas.Remove(consulta);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> GetConsultaHistorico(int id, AppDbContext db) =>
            Results.Ok(await db.ConsultaHistoricos.Where(h => h.ConsultaId == id).ToListAsync());

        // ===== EXAMES =====
        private static async Task<IResult> GetExames(AppDbContext db) => Results.Ok(await db.Exames.ToListAsync());
        private static async Task<IResult> GetExameById(int id, AppDbContext db) =>
            await db.Exames.FindAsync(id) is Exame e ? Results.Ok(e) : Results.NotFound();

        private static async Task<IResult> CreateExame(ExameRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var exameId = (await db.Exames.AnyAsync()) ? await db.Exames.MaxAsync(x => x.Id) + 1 : 1;
            
            var exame = new Exame
            {
                Id = exameId,
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdUnidade = request.IdUnidade,
                Tipo = request.Tipo,
                Data = request.Data,
                Horario = request.Horario
            };

            db.Exames.Add(exame);
            await db.SaveChangesAsync();

            var historico = new ExameHistorico
            {
                ExameId = exame.Id, IdPaciente = exame.IdPaciente, IdMedico = exame.IdMedico,
                IdUnidade = exame.IdUnidade, Tipo = exame.Tipo, Data = exame.Data, Horario = exame.Horario,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "INCLUSÃO"
            };
            db.ExameHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Created($"/exames/{exame.Id}", exame);
        }

        private static async Task<IResult> UpdateExame(int id, ExameRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var exame = await db.Exames.FindAsync(id);
            if (exame is null) return Results.NotFound();

            exame.IdPaciente = request.IdPaciente;
            exame.IdMedico = request.IdMedico;
            exame.IdUnidade = request.IdUnidade;
            exame.Tipo = request.Tipo;
            exame.Data = request.Data;
            exame.Horario = request.Horario;
            await db.SaveChangesAsync();

            var historico = new ExameHistorico
            {
                ExameId = exame.Id, IdPaciente = exame.IdPaciente, IdMedico = exame.IdMedico,
                IdUnidade = exame.IdUnidade, Tipo = exame.Tipo, Data = exame.Data, Horario = exame.Horario,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "ALTERAÇÃO"
            };
            db.ExameHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Ok(exame);
        }

        private static async Task<IResult> DeleteExame(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var exame = await db.Exames.FindAsync(id);
            if (exame is null) return Results.NotFound();

            var historico = new ExameHistorico
            {
                ExameId = exame.Id, IdPaciente = exame.IdPaciente, IdMedico = exame.IdMedico,
                IdUnidade = exame.IdUnidade, Tipo = exame.Tipo, Data = exame.Data, Horario = exame.Horario,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "EXCLUSÃO"
            };
            db.ExameHistoricos.Add(historico);
            db.Exames.Remove(exame);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> GetExameHistorico(int id, AppDbContext db) =>
            Results.Ok(await db.ExameHistoricos.Where(h => h.ExameId == id).ToListAsync());

        // ===== PRESCRIÇÕES =====
        private static async Task<IResult> GetPrescricoes(AppDbContext db) => Results.Ok(await db.Prescricoes.ToListAsync());
        private static async Task<IResult> GetPrescricaoById(int id, AppDbContext db) =>
            await db.Prescricoes.FindAsync(id) is Prescricao p ? Results.Ok(p) : Results.NotFound();

        private static async Task<IResult> CreatePrescricao(PrescricaoRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var prescricaoId = (await db.Prescricoes.AnyAsync()) ? await db.Prescricoes.MaxAsync(x => x.Id) + 1 : 1;
            
            var prescricao = new Prescricao
            {
                Id = prescricaoId,
                IdPaciente = request.IdPaciente,
                IdMedico = request.IdMedico,
                IdConsulta = request.IdConsulta,
                Texto = request.Texto
            };

            db.Prescricoes.Add(prescricao);
            await db.SaveChangesAsync();

            var historico = new PrescricaoHistorico
            {
                PrescricaoId = prescricao.Id, IdPaciente = prescricao.IdPaciente, IdMedico = prescricao.IdMedico,
                IdConsulta = prescricao.IdConsulta, Texto = prescricao.Texto,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "INCLUSÃO"
            };
            db.PrescricaoHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Created($"/prescricoes/{prescricao.Id}", prescricao);
        }

        private static async Task<IResult> UpdatePrescricao(int id, PrescricaoRequest request, AppDbContext db, string sobrenome = "Mancini")
        {
            var prescricao = await db.Prescricoes.FindAsync(id);
            if (prescricao is null) return Results.NotFound();

            prescricao.IdPaciente = request.IdPaciente;
            prescricao.IdMedico = request.IdMedico;
            prescricao.IdConsulta = request.IdConsulta;
            prescricao.Texto = request.Texto;
            await db.SaveChangesAsync();

            var historico = new PrescricaoHistorico
            {
                PrescricaoId = prescricao.Id, IdPaciente = prescricao.IdPaciente, IdMedico = prescricao.IdMedico,
                IdConsulta = prescricao.IdConsulta, Texto = prescricao.Texto,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "ALTERAÇÃO"
            };
            db.PrescricaoHistoricos.Add(historico);
            await db.SaveChangesAsync();
            return Results.Ok(prescricao);
        }

        private static async Task<IResult> DeletePrescricao(int id, AppDbContext db, string sobrenome = "Mancini")
        {
            var prescricao = await db.Prescricoes.FindAsync(id);
            if (prescricao is null) return Results.NotFound();

            var historico = new PrescricaoHistorico
            {
                PrescricaoId = prescricao.Id, IdPaciente = prescricao.IdPaciente, IdMedico = prescricao.IdMedico,
                IdConsulta = prescricao.IdConsulta, Texto = prescricao.Texto,
                Timestamp = DateTime.Now, Usuario = sobrenome, Acao = "EXCLUSÃO"
            };
            db.PrescricaoHistoricos.Add(historico);
            db.Prescricoes.Remove(prescricao);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> GetPrescricaoHistorico(int id, AppDbContext db) =>
            Results.Ok(await db.PrescricaoHistoricos.Where(h => h.PrescricaoId == id).ToListAsync());
    }
}
