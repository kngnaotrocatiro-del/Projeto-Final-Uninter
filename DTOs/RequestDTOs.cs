namespace trabalhoUninter.DTOs
{
    // ========== PACIENTE ==========
    public record PacienteRequest(string CPF, string Nome, string PlanoDeSaude);

    // ========== PROFISSIONAL ==========
    public record ProfissionalRequest(string CPF, string Nome);

    // ========== MÉDICO ==========
    public record MedicoRequest(string CPF, string CRM, string Nome, string Especialidade);

    // ========== UNIDADE DE SAÚDE ==========
    public record UnidadeSaudeRequest(string Tipo, string CNPJ, string Nome);

    // ========== HOSPITAL ==========
    public record HospitalRequest(string Nome, int NumeroLeitos, int NumeroProfissionais, int NumeroSuprimentos, decimal Gastos, decimal Lucros, DateTime DiaAtualizacao);

    // ========== CONSULTA ==========
    public record ConsultaRequest(int IdPaciente, int IdMedico, int IdUnidade, string Tipo, DateTime Data, string Horario);

    // ========== EXAME ==========
    public record ExameRequest(int IdPaciente, int IdMedico, int IdUnidade, string Tipo, DateTime Data, string Horario);

    // ========== PRESCRIÇÃO ==========
    public record PrescricaoRequest(int IdPaciente, int IdMedico, int IdConsulta, string Texto);

    // ========== PRONTUÁRIO ==========
    public record ProntuarioRequest(int IdConsulta, int IdPaciente, int IdMedico, string Texto);

    // ========== TELEMEDICINA ==========
    public record TelemedinaSessaoRequest(int IdPaciente, int IdMedico, int IdConsulta, string LinkVideo);
}
