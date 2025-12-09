namespace trabalhoUninter.Models
{
    public class MedicoHistorico
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string CRM { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
