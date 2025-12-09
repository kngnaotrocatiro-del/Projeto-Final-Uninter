namespace trabalhoUninter.Models
{
    public class UnidadeSaudeHistorico
    {
        public int Id { get; set; }
        public int UnidadeSaudeId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
