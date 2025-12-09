namespace trabalhoUninter.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdUnidade { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Horario { get; set; } = string.Empty;

        // Relacionamentos
        public virtual Paciente? Paciente { get; set; }
        public virtual Medico? Medico { get; set; }
        public virtual UnidadeSaude? UnidadeSaude { get; set; }
        public virtual ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
        public virtual ICollection<Prontuario> Prontuarios { get; set; } = new List<Prontuario>();
        public virtual TelemedinaSessao? TelemedinaSessao { get; set; } // 1:1
        public virtual ICollection<Profissional> Profissionais { get; set; } = new List<Profissional>(); // M:M
    }
}
