namespace trabalhoUninter.Models
{
    public class Profissional
    {
        public int Id { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        // Relacionamentos 1:N
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
        public virtual ICollection<Exame> Exames { get; set; } = new List<Exame>();
        public virtual ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
        public virtual ICollection<Prontuario> Prontuarios { get; set; } = new List<Prontuario>();
        public virtual ICollection<TelemedinaSessao> TelemedinaSessoes { get; set; } = new List<TelemedinaSessao>();
    }
}
