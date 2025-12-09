namespace trabalhoUninter.Models
{
    public class ConsultaHistorico
    {
        public int Id { get; set; }
        public int ConsultaId { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdUnidade { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Horario { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
