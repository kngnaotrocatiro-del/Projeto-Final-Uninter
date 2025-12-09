namespace trabalhoUninter.Models
{
    public class ProfissionalHistorico
    {
        public int Id { get; set; }
        public int ProfissionalId { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
