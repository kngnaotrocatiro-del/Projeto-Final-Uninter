namespace trabalhoUninter.Models
{
    public class HospitalHistorico
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int NumeroLeitos { get; set; }
        public int NumeroProfissionais { get; set; }
        public int NumeroSuprimentos { get; set; }
        public decimal Gastos { get; set; }
        public decimal Lucros { get; set; }
        public DateTime DiaAtualizacao { get; set; }
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty; // INCLUSÃO, ALTERAÇÃO, EXCLUSÃO
    }
}
