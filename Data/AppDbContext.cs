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

            // Seed de dados de teste
            SeedTestData(modelBuilder);
        }

        private static void SeedTestData(ModelBuilder modelBuilder)
        {
            // ===== PACIENTES =====
            modelBuilder.Entity<Paciente>().HasData(
                new Paciente { Id = 1, CPF = "123.456.789-00", Nome = "João Silva", PlanoDeSaude = "Unimed" },
                new Paciente { Id = 2, CPF = "987.654.321-11", Nome = "Maria Santos", PlanoDeSaude = "SulAmérica" },
                new Paciente { Id = 3, CPF = "456.789.123-22", Nome = "Pedro Oliveira", PlanoDeSaude = "Bradesco Saúde" }
            );

            // ===== PROFISSIONAIS =====
            modelBuilder.Entity<Profissional>().HasData(
                new Profissional { Id = 1, CPF = "111.222.333-44", Nome = "Enfermeira Ana" },
                new Profissional { Id = 2, CPF = "222.333.444-55", Nome = "Técnico Carlos" },
                new Profissional { Id = 3, CPF = "333.444.555-66", Nome = "Auxiliar Beatriz" }
            );

            // ===== MÉDICOS =====
            modelBuilder.Entity<Medico>().HasData(
                new Medico { Id = 1, CPF = "444.555.666-77", CRM = "123456/SP", Nome = "Dr. Fernando Lima", Especialidade = "Cardiologia" },
                new Medico { Id = 2, CPF = "555.666.777-88", CRM = "234567/RJ", Nome = "Dra. Lucia Costa", Especialidade = "Pediatria" },
                new Medico { Id = 3, CPF = "666.777.888-99", CRM = "345678/MG", Nome = "Dr. Roberto Alves", Especialidade = "Ortopedia" }
            );

            // ===== UNIDADES DE SAÚDE =====
            modelBuilder.Entity<UnidadeSaude>().HasData(
                new UnidadeSaude { Id = 1, Tipo = "hospital", CNPJ = "12.345.678/0001-90", Nome = "Hospital Central" },
                new UnidadeSaude { Id = 2, Tipo = "clinica", CNPJ = "98.765.432/0001-21", Nome = "Clínica Saúde Plus" },
                new UnidadeSaude { Id = 3, Tipo = "laboratorio", CNPJ = "55.555.555/0001-50", Nome = "Lab Análises Médicas" }
            );

            // ===== HOSPITAIS =====
            modelBuilder.Entity<Hospital>().HasData(
                new Hospital 
                { 
                    Id = 1, 
                    Nome = "Hospital Central", 
                    NumeroLeitos = 200, 
                    NumeroProfissionais = 150, 
                    NumeroSuprimentos = 5000,
                    Gastos = 500000.00m,
                    Lucros = 150000.00m,
                    DiaAtualizacao = DateTime.Now,
                    UnidadeSaudeId = 1
                },
                new Hospital 
                { 
                    Id = 2, 
                    Nome = "Hospital Metropolitano", 
                    NumeroLeitos = 300, 
                    NumeroProfissionais = 250, 
                    NumeroSuprimentos = 8000,
                    Gastos = 800000.00m,
                    Lucros = 300000.00m,
                    DiaAtualizacao = DateTime.Now,
                    UnidadeSaudeId = 1
                },
                new Hospital 
                { 
                    Id = 3, 
                    Nome = "Hospital Regional", 
                    NumeroLeitos = 100, 
                    NumeroProfissionais = 80, 
                    NumeroSuprimentos = 3000,
                    Gastos = 300000.00m,
                    Lucros = 80000.00m,
                    DiaAtualizacao = DateTime.Now,
                    UnidadeSaudeId = 1
                }
            );

            // ===== CONSULTAS =====
            modelBuilder.Entity<Consulta>().HasData(
                new Consulta 
                { 
                    Id = 1, 
                    IdPaciente = 1, 
                    IdMedico = 1, 
                    IdUnidade = 1, 
                    Tipo = "Presencial", 
                    Data = DateTime.Now.AddDays(1),
                    Horario = "09:00"
                },
                new Consulta 
                { 
                    Id = 2, 
                    IdPaciente = 2, 
                    IdMedico = 2, 
                    IdUnidade = 1, 
                    Tipo = "Presencial", 
                    Data = DateTime.Now.AddDays(2),
                    Horario = "14:00"
                },
                new Consulta 
                { 
                    Id = 3, 
                    IdPaciente = 3, 
                    IdMedico = 3, 
                    IdUnidade = 1, 
                    Tipo = "Telemedicina", 
                    Data = DateTime.Now.AddDays(3),
                    Horario = "10:30"
                }
            );

            // ===== EXAMES =====
            modelBuilder.Entity<Exame>().HasData(
                new Exame 
                { 
                    Id = 1, 
                    IdPaciente = 1, 
                    IdMedico = 1, 
                    IdUnidade = 3, 
                    Tipo = "Eletrocardiograma", 
                    Data = DateTime.Now.AddDays(1),
                    Horario = "10:00"
                },
                new Exame 
                { 
                    Id = 2, 
                    IdPaciente = 2, 
                    IdMedico = 2, 
                    IdUnidade = 3, 
                    Tipo = "Ultrassom", 
                    Data = DateTime.Now.AddDays(2),
                    Horario = "15:00"
                },
                new Exame 
                { 
                    Id = 3, 
                    IdPaciente = 3, 
                    IdMedico = 3, 
                    IdUnidade = 3, 
                    Tipo = "Radiografia", 
                    Data = DateTime.Now.AddDays(4),
                    Horario = "11:00"
                }
            );

            // ===== PRESCRIÇÕES =====
            modelBuilder.Entity<Prescricao>().HasData(
                new Prescricao 
                { 
                    Id = 1, 
                    IdPaciente = 1, 
                    IdMedico = 1, 
                    IdConsulta = 1, 
                    Texto = "Dipirona 500mg - tomar 1 comprimido a cada 6 horas por 7 dias" 
                },
                new Prescricao 
                { 
                    Id = 2, 
                    IdPaciente = 2, 
                    IdMedico = 2, 
                    IdConsulta = 2, 
                    Texto = "Amoxicilina 500mg - tomar 1 comprimido a cada 8 horas por 10 dias" 
                },
                new Prescricao 
                { 
                    Id = 3, 
                    IdPaciente = 3, 
                    IdMedico = 3, 
                    IdConsulta = 3, 
                    Texto = "Ibuprofeno 400mg - tomar 1 comprimido a cada 6 horas conforme dor" 
                }
            );

            // ===== PRONTUÁRIOS =====
            modelBuilder.Entity<Prontuario>().HasData(
                new Prontuario 
                { 
                    Id = 1, 
                    IdConsulta = 1, 
                    IdPaciente = 1, 
                    IdMedico = 1, 
                    Texto = "Paciente apresenta hipertensão. Recomenda-se controle de sódio e atividade física." 
                },
                new Prontuario 
                { 
                    Id = 2, 
                    IdConsulta = 2, 
                    IdPaciente = 2, 
                    IdMedico = 2, 
                    Texto = "Criança com infecção de ouvido. Prescrição de antibiótico. Seguimento em 1 semana." 
                },
                new Prontuario 
                { 
                    Id = 3, 
                    IdConsulta = 3, 
                    IdPaciente = 3, 
                    IdMedico = 3, 
                    Texto = "Paciente com dor nas costas. Encaminhado para fisioterapia. Sessões 3x por semana." 
                }
            );

            // ===== TELEMEDICINA SESSÕES =====
            modelBuilder.Entity<TelemedinaSessao>().HasData(
                new TelemedinaSessao 
                { 
                    Id = 1, 
                    IdPaciente = 1, 
                    IdMedico = 1, 
                    IdConsulta = 3, 
                    LinkVideo = "https://meet.google.com/abc-def-ghi" 
                },
                new TelemedinaSessao 
                { 
                    Id = 2, 
                    IdPaciente = 2, 
                    IdMedico = 2, 
                    IdConsulta = 2, 
                    LinkVideo = "https://meet.google.com/xyz-uvw-rst" 
                },
                new TelemedinaSessao 
                { 
                    Id = 3, 
                    IdPaciente = 3, 
                    IdMedico = 3, 
                    IdConsulta = 3, 
                    LinkVideo = "https://meet.google.com/mno-pqr-stu" 
                }
            );

            // ===== HISTÓRICOS =====
            var now = DateTime.Now;

            // Histórico de Pacientes
            modelBuilder.Entity<PacienteHistorico>().HasData(
                new PacienteHistorico { Id = 1, PacienteId = 1, CPF = "123.456.789-00", Nome = "João Silva", PlanoDeSaude = "Unimed", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new PacienteHistorico { Id = 2, PacienteId = 1, CPF = "123.456.789-00", Nome = "João Silva", PlanoDeSaude = "Unimed", Timestamp = now.AddHours(-2), Usuario = "Mancini", Acao = "ALTERAÇÃO" },
                new PacienteHistorico { Id = 3, PacienteId = 2, CPF = "987.654.321-11", Nome = "Maria Santos", PlanoDeSaude = "SulAmérica", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new PacienteHistorico { Id = 4, PacienteId = 3, CPF = "456.789.123-22", Nome = "Pedro Oliveira", PlanoDeSaude = "Bradesco Saúde", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Profissionais
            modelBuilder.Entity<ProfissionalHistorico>().HasData(
                new ProfissionalHistorico { Id = 1, ProfissionalId = 1, CPF = "111.222.333-44", Nome = "Enfermeira Ana", Timestamp = now.AddHours(-6), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ProfissionalHistorico { Id = 2, ProfissionalId = 1, CPF = "111.222.333-44", Nome = "Enfermeira Ana", Timestamp = now.AddHours(-1), Usuario = "Mancini", Acao = "ALTERAÇÃO" },
                new ProfissionalHistorico { Id = 3, ProfissionalId = 2, CPF = "222.333.444-55", Nome = "Técnico Carlos", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ProfissionalHistorico { Id = 4, ProfissionalId = 3, CPF = "333.444.555-66", Nome = "Auxiliar Beatriz", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Médicos
            modelBuilder.Entity<MedicoHistorico>().HasData(
                new MedicoHistorico { Id = 1, MedicoId = 1, CPF = "444.555.666-77", CRM = "123456/SP", Nome = "Dr. Fernando Lima", Especialidade = "Cardiologia", Timestamp = now.AddHours(-7), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new MedicoHistorico { Id = 2, MedicoId = 1, CPF = "444.555.666-77", CRM = "123456/SP", Nome = "Dr. Fernando Lima", Especialidade = "Cardiologia", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "ALTERAÇÃO" },
                new MedicoHistorico { Id = 3, MedicoId = 2, CPF = "555.666.777-88", CRM = "234567/RJ", Nome = "Dra. Lucia Costa", Especialidade = "Pediatria", Timestamp = now.AddHours(-6), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new MedicoHistorico { Id = 4, MedicoId = 3, CPF = "666.777.888-99", CRM = "345678/MG", Nome = "Dr. Roberto Alves", Especialidade = "Ortopedia", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Unidades de Saúde
            modelBuilder.Entity<UnidadeSaudeHistorico>().HasData(
                new UnidadeSaudeHistorico { Id = 1, UnidadeSaudeId = 1, Tipo = "hospital", CNPJ = "12.345.678/0001-90", Nome = "Hospital Central", Timestamp = now.AddHours(-8), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new UnidadeSaudeHistorico { Id = 2, UnidadeSaudeId = 2, Tipo = "clinica", CNPJ = "98.765.432/0001-21", Nome = "Clínica Saúde Plus", Timestamp = now.AddHours(-7), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new UnidadeSaudeHistorico { Id = 3, UnidadeSaudeId = 3, Tipo = "laboratorio", CNPJ = "55.555.555/0001-50", Nome = "Lab Análises Médicas", Timestamp = now.AddHours(-6), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Hospitais
            modelBuilder.Entity<HospitalHistorico>().HasData(
                new HospitalHistorico { Id = 1, HospitalId = 1, Nome = "Hospital Central", NumeroLeitos = 200, NumeroProfissionais = 150, NumeroSuprimentos = 5000, Gastos = 500000.00m, Lucros = 150000.00m, DiaAtualizacao = now, Timestamp = now.AddHours(-8), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new HospitalHistorico { Id = 2, HospitalId = 1, Nome = "Hospital Central", NumeroLeitos = 200, NumeroProfissionais = 160, NumeroSuprimentos = 5200, Gastos = 520000.00m, Lucros = 160000.00m, DiaAtualizacao = now, Timestamp = now.AddHours(-1), Usuario = "Mancini", Acao = "ALTERAÇÃO" },
                new HospitalHistorico { Id = 3, HospitalId = 2, Nome = "Hospital Metropolitano", NumeroLeitos = 300, NumeroProfissionais = 250, NumeroSuprimentos = 8000, Gastos = 800000.00m, Lucros = 300000.00m, DiaAtualizacao = now, Timestamp = now.AddHours(-7), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new HospitalHistorico { Id = 4, HospitalId = 3, Nome = "Hospital Regional", NumeroLeitos = 100, NumeroProfissionais = 80, NumeroSuprimentos = 3000, Gastos = 300000.00m, Lucros = 80000.00m, DiaAtualizacao = now, Timestamp = now.AddHours(-6), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Consultas
            modelBuilder.Entity<ConsultaHistorico>().HasData(
                new ConsultaHistorico { Id = 1, ConsultaId = 1, IdPaciente = 1, IdMedico = 1, IdUnidade = 1, Tipo = "Presencial", Data = now.AddDays(1), Horario = "09:00", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ConsultaHistorico { Id = 2, ConsultaId = 2, IdPaciente = 2, IdMedico = 2, IdUnidade = 1, Tipo = "Presencial", Data = now.AddDays(2), Horario = "14:00", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ConsultaHistorico { Id = 3, ConsultaId = 3, IdPaciente = 3, IdMedico = 3, IdUnidade = 1, Tipo = "Telemedicina", Data = now.AddDays(3), Horario = "10:30", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Exames
            modelBuilder.Entity<ExameHistorico>().HasData(
                new ExameHistorico { Id = 1, ExameId = 1, IdPaciente = 1, IdMedico = 1, IdUnidade = 3, Tipo = "Eletrocardiograma", Data = now.AddDays(1), Horario = "10:00", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ExameHistorico { Id = 2, ExameId = 2, IdPaciente = 2, IdMedico = 2, IdUnidade = 3, Tipo = "Ultrassom", Data = now.AddDays(2), Horario = "15:00", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ExameHistorico { Id = 3, ExameId = 3, IdPaciente = 3, IdMedico = 3, IdUnidade = 3, Tipo = "Radiografia", Data = now.AddDays(4), Horario = "11:00", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Prescrições
            modelBuilder.Entity<PrescricaoHistorico>().HasData(
                new PrescricaoHistorico { Id = 1, PrescricaoId = 1, IdPaciente = 1, IdMedico = 1, IdConsulta = 1, Texto = "Dipirona 500mg - tomar 1 comprimido a cada 6 horas por 7 dias", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new PrescricaoHistorico { Id = 2, PrescricaoId = 2, IdPaciente = 2, IdMedico = 2, IdConsulta = 2, Texto = "Amoxicilina 500mg - tomar 1 comprimido a cada 8 horas por 10 dias", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new PrescricaoHistorico { Id = 3, PrescricaoId = 3, IdPaciente = 3, IdMedico = 3, IdConsulta = 3, Texto = "Ibuprofeno 400mg - tomar 1 comprimido a cada 6 horas conforme dor", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Prontuários
            modelBuilder.Entity<ProntuarioHistorico>().HasData(
                new ProntuarioHistorico { Id = 1, ProntuarioId = 1, IdConsulta = 1, IdPaciente = 1, IdMedico = 1, Texto = "Paciente apresenta hipertensão. Recomenda-se controle de sódio e atividade física.", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ProntuarioHistorico { Id = 2, ProntuarioId = 2, IdConsulta = 2, IdPaciente = 2, IdMedico = 2, Texto = "Criança com infecção de ouvido. Prescrição de antibiótico. Seguimento em 1 semana.", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new ProntuarioHistorico { Id = 3, ProntuarioId = 3, IdConsulta = 3, IdPaciente = 3, IdMedico = 3, Texto = "Paciente com dor nas costas. Encaminhado para fisioterapia. Sessões 3x por semana.", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );

            // Histórico de Telemedicina Sessões
            modelBuilder.Entity<TelemedinaSessaoHistorico>().HasData(
                new TelemedinaSessaoHistorico { Id = 1, TelemedinaSessaoId = 1, IdPaciente = 1, IdMedico = 1, IdConsulta = 3, LinkVideo = "https://meet.google.com/abc-def-ghi", Timestamp = now.AddHours(-5), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new TelemedinaSessaoHistorico { Id = 2, TelemedinaSessaoId = 2, IdPaciente = 2, IdMedico = 2, IdConsulta = 2, LinkVideo = "https://meet.google.com/xyz-uvw-rst", Timestamp = now.AddHours(-4), Usuario = "Mancini", Acao = "INCLUSÃO" },
                new TelemedinaSessaoHistorico { Id = 3, TelemedinaSessaoId = 3, IdPaciente = 3, IdMedico = 3, IdConsulta = 3, LinkVideo = "https://meet.google.com/mno-pqr-stu", Timestamp = now.AddHours(-3), Usuario = "Mancini", Acao = "INCLUSÃO" }
            );
        }
    }
}
