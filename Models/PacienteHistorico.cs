namespace trabalhoUninter.Models
{
    public class PacienteHistorico
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string PlanoDeSaude { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // "INCLUSÃO", "ALTERAÇÃO", "EXCLUSÃO"
    }
}
