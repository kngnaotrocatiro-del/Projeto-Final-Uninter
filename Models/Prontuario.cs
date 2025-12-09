namespace trabalhoUninter.Models
{
    public class Prontuario
    {
        public int Id { get; set; }
        public int IdConsulta { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public string Texto { get; set; } = string.Empty;

        // Relacionamentos
        public virtual Consulta? Consulta { get; set; }
        public virtual Paciente? Paciente { get; set; }
        public virtual Medico? Medico { get; set; }
        public virtual ICollection<Profissional> Profissionais { get; set; } = new List<Profissional>(); // M:M
    }
}
