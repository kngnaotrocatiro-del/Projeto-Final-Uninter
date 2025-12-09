using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Data;

namespace trabalhoUninter.Extensions
{
    public static class HistoricoEndpoints
    {
        public static void MapHistoricoEndpoints(this WebApplication app)
        {
            // ===== HISTÓRICO DE PACIENTES =====
            app.MapGet("/historico/pacientes", GetPacienteHistorico)
                .WithName("GetAllPacienteHistorico");

            // ===== HISTÓRICO DE PROFISSIONAIS =====
            app.MapGet("/historico/profissionais", GetProfissionalHistorico)
                .WithName("GetAllProfissionalHistorico");

            // ===== HISTÓRICO DE MÉDICOS =====
            app.MapGet("/historico/medicos", GetMedicoHistorico)
                .WithName("GetAllMedicoHistorico");

            // ===== HISTÓRICO DE UNIDADES DE SAÚDE =====
            app.MapGet("/historico/unidades-saude", GetUnidadeSaudeHistorico)
                .WithName("GetAllUnidadeSaudeHistorico");

            // ===== HISTÓRICO DE HOSPITAIS =====
            app.MapGet("/historico/hospitais", GetHospitalHistorico)
                .WithName("GetAllHospitalHistorico");

            // ===== HISTÓRICO DE CONSULTAS =====
            app.MapGet("/historico/consultas", GetConsultaHistorico)
                .WithName("GetAllConsultaHistorico");

            // ===== HISTÓRICO DE EXAMES =====
            app.MapGet("/historico/exames", GetExameHistorico)
                .WithName("GetAllExameHistorico");

            // ===== HISTÓRICO DE PRESCRIÇÕES =====
            app.MapGet("/historico/prescricoes", GetPrescricaoHistorico)
                .WithName("GetAllPrescricaoHistorico");

            // ===== HISTÓRICO DE PRONTUÁRIOS =====
            app.MapGet("/historico/prontuarios", GetProntuarioHistorico)
                .WithName("GetAllProntuarioHistorico");

            // ===== HISTÓRICO DE TELEMEDICINA =====
            app.MapGet("/historico/telemedicina", GetTelemedinaSessaoHistorico)
                .WithName("GetAllTelemedinaSessaoHistorico");
        }

        private static async Task<IResult> GetPacienteHistorico(AppDbContext db) =>
            Results.Ok(await db.PacienteHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetProfissionalHistorico(AppDbContext db) =>
            Results.Ok(await db.ProfissionalHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetMedicoHistorico(AppDbContext db) =>
            Results.Ok(await db.MedicoHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetUnidadeSaudeHistorico(AppDbContext db) =>
            Results.Ok(await db.UnidadesSaudeHistorico.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetHospitalHistorico(AppDbContext db) =>
            Results.Ok(await db.HospitalHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetConsultaHistorico(AppDbContext db) =>
            Results.Ok(await db.ConsultaHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetExameHistorico(AppDbContext db) =>
            Results.Ok(await db.ExameHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetPrescricaoHistorico(AppDbContext db) =>
            Results.Ok(await db.PrescricaoHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetProntuarioHistorico(AppDbContext db) =>
            Results.Ok(await db.ProntuarioHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());

        private static async Task<IResult> GetTelemedinaSessaoHistorico(AppDbContext db) =>
            Results.Ok(await db.TelemedinaSessaoHistoricos.OrderByDescending(h => h.Timestamp).ToListAsync());
    }
}
