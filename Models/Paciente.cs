namespace trabalhoUninter.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string PlanoDeSaude { get; set; } = string.Empty;
    }
}
