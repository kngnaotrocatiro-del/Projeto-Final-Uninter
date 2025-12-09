using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Models;

namespace trabalhoUninter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Pacientes
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<PacienteHistorico> PacienteHistoricos => Set<PacienteHistorico>();

        // Profissionais
        public DbSet<Profissional> Profissionais => Set<Profissional>();
        public DbSet<ProfissionalHistorico> ProfissionalHistoricos => Set<ProfissionalHistorico>();

        // Médicos
        public DbSet<Medico> Medicos => Set<Medico>();
        public DbSet<MedicoHistorico> MedicoHistoricos => Set<MedicoHistorico>();

        // Unidades de Saúde
        public DbSet<UnidadeSaude> UnidadesSaude => Set<UnidadeSaude>();
        public DbSet<UnidadeSaudeHistorico> UnidadesSaudeHistorico => Set<UnidadeSaudeHistorico>();

        // Hospitais
        public DbSet<Hospital> Hospitais => Set<Hospital>();
        public DbSet<HospitalHistorico> HospitalHistoricos => Set<HospitalHistorico>();

        // Consultas
        public DbSet<Consulta> Consultas => Set<Consulta>();
        public DbSet<ConsultaHistorico> ConsultaHistoricos => Set<ConsultaHistorico>();

        // Exames
        public DbSet<Exame> Exames => Set<Exame>();
        public DbSet<ExameHistorico> ExameHistoricos => Set<ExameHistorico>();

        // Prescrições
        public DbSet<Prescricao> Prescricoes => Set<Prescricao>();
        public DbSet<PrescricaoHistorico> PrescricaoHistoricos => Set<PrescricaoHistorico>();

        // Prontuários
        public DbSet<Prontuario> Prontuarios => Set<Prontuario>();
        public DbSet<ProntuarioHistorico> ProntuarioHistoricos => Set<ProntuarioHistorico>();

        // Telemedicina
        public DbSet<TelemedinaSessao> TelemedinaSessoes => Set<TelemedinaSessao>();
        public DbSet<TelemedinaSessaoHistorico> TelemedinaSessaoHistoricos => Set<TelemedinaSessaoHistorico>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do relacionamento 1:1 entre UnidadeSaude e Hospital
            modelBuilder.Entity<UnidadeSaude>()
                .HasOne(u => u.Hospital)
                .WithOne(h => h.UnidadeSaude)
                .HasForeignKey<Hospital>(h => h.UnidadeSaudeId);

            // Configuração do relacionamento 1:1 entre Consulta e TelemedinaSessao
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.TelemedinaSessao)
                .WithOne(t => t.Consulta)
                .HasForeignKey<TelemedinaSessao>(t => t.IdConsulta);

            // Configuração dos relacionamentos N:1 para Consulta
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.IdPaciente);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Medico)
                .WithMany(m => m.Consultas)
                .HasForeignKey(c => c.IdMedico);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.UnidadeSaude)
                .WithMany(u => u.Consultas)
                .HasForeignKey(c => c.IdUnidade);

            // Configuração dos relacionamentos N:1 para Exame
            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Paciente)
                .WithMany(p => p.Exames)
                .HasForeignKey(e => e.IdPaciente);

            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Medico)
                .WithMany(m => m.Exames)
                .HasForeignKey(e => e.IdMedico);

            modelBuilder.Entity<Exame>()
                .HasOne(e => e.UnidadeSaude)
                .WithMany(u => u.Exames)
                .HasForeignKey(e => e.IdUnidade);

            // Configuração dos relacionamentos N:1 para Prescrição
            modelBuilder.Entity<Prescricao>()
                .HasOne(p => p.Paciente)
                .WithMany(pat => pat.Prescricoes)
                .HasForeignKey(p => p.IdPaciente);

            modelBuilder.Entity<Prescricao>()
                .HasOne(p => p.Medico)
                .WithMany(m => m.Prescricoes)
                .HasForeignKey(p => p.IdMedico);

            modelBuilder.Entity<Prescricao>()
                .HasOne(p => p.Consulta)
                .WithMany(c => c.Prescricoes)
                .HasForeignKey(p => p.IdConsulta);

            // Configuração dos relacionamentos N:1 para Prontuário
            modelBuilder.Entity<Prontuario>()
                .HasOne(pr => pr.Consulta)
                .WithMany(c => c.Prontuarios)
                .HasForeignKey(pr => pr.IdConsulta);

            modelBuilder.Entity<Prontuario>()
                .HasOne(pr => pr.Paciente)
                .WithMany(pat => pat.Prontuarios)
                .HasForeignKey(pr => pr.IdPaciente);

            modelBuilder.Entity<Prontuario>()
                .HasOne(pr => pr.Medico)
                .WithMany(m => m.Prontuarios)
                .HasForeignKey(pr => pr.IdMedico);

            // Configuração dos relacionamentos N:1 para TelemedinaSessao
            modelBuilder.Entity<TelemedinaSessao>()
                .HasOne(t => t.Paciente)
                .WithMany(pat => pat.TelemedinaSessoes)
                .HasForeignKey(t => t.IdPaciente);

            modelBuilder.Entity<TelemedinaSessao>()
                .HasOne(t => t.Medico)
                .WithMany(m => m.TelemedinaSessoes)
                .HasForeignKey(t => t.IdMedico);

            // Configuração dos relacionamentos para Profissional
            modelBuilder.Entity<Profissional>()
                .HasMany(prof => prof.Consultas)
                .WithMany(c => c.Profissionais)
                .UsingEntity("ProfissionalConsultas");

            modelBuilder.Entity<Profissional>()
                .HasMany(prof => prof.Exames)
                .WithMany(e => e.Profissionais)
                .UsingEntity("ProfissionalExames");

            modelBuilder.Entity<Profissional>()
                .HasMany(prof => prof.Prescricoes)
                .WithMany(p => p.Profissionais)
                .UsingEntity("ProfissionalPrescricoes");

            modelBuilder.Entity<Profissional>()
                .HasMany(prof => prof.Prontuarios)
                .WithMany(pr => pr.Profissionais)
                .UsingEntity("ProfissionalProntuarios");

            modelBuilder.Entity<Profissional>()
                .HasMany(prof => prof.TelemedinaSessoes)
                .WithMany(t => t.Profissionais)
                .UsingEntity("ProfissionalTelemedinaSessoes");
        }
    }
}
