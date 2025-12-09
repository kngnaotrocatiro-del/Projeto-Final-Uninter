namespace trabalhoUninter.Models
{
    public class ProntuarioHistorico
    {
        public int Id { get; set; }
        public int ProntuarioId { get; set; }
        public int IdConsulta { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public string Texto { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
