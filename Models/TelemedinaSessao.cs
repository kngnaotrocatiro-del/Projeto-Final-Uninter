namespace trabalhoUninter.Models
{
    public class TelemedinaSessao
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdConsulta { get; set; }
        public string LinkVideo { get; set; } = string.Empty;

        // Relacionamentos
        public virtual Paciente? Paciente { get; set; }
        public virtual Medico? Medico { get; set; }
        public virtual Consulta? Consulta { get; set; } // 1:1
        public virtual ICollection<Profissional> Profissionais { get; set; } = new List<Profissional>(); // M:M
    }
}
