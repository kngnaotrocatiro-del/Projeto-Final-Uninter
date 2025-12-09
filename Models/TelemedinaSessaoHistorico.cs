namespace trabalhoUninter.Models
{
    public class TelemedinaSessaoHistorico
    {
        public int Id { get; set; }
        public int TelemedinaSessaoId { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdConsulta { get; set; }
        public string LinkVideo { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
