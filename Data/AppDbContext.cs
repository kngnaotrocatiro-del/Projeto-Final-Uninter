using Microsoft.EntityFrameworkCore;
using trabalhoUninter.Models;

namespace trabalhoUninter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<PacienteHistorico> PacienteHistoricos => Set<PacienteHistorico>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
