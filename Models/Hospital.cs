namespace trabalhoUninter.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int NumeroLeitos { get; set; }
        public int NumeroProfissionais { get; set; }
        public int NumeroSuprimentos { get; set; }
        public decimal Gastos { get; set; }
        public decimal Lucros { get; set; }
        public DateTime DiaAtualizacao { get; set; }

        // Relacionamentos
        public int UnidadeSaudeId { get; set; }
        public virtual UnidadeSaude? UnidadeSaude { get; set; } // 1:1 com UnidadeSaude
        public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
    }
}
