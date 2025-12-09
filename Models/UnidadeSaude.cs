namespace trabalhoUninter.Models
{
    public class UnidadeSaude
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // hospital, clinica, upa, etc
        public string CNPJ { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        // Relacionamentos
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
        public virtual ICollection<Exame> Exames { get; set; } = new List<Exame>();
        public virtual Hospital? Hospital { get; set; } // 1:1 quando tipo = hospital
    }
}
