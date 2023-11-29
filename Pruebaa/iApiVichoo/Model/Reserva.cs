public class Reserva
{
    public int IdReserva { get; set; }
    public string Especialidad { get; set; }
    public String DiaReserva { get; set; }
    public int PacienteIdPaciente { get; set; }
    public Paciente Paciente { get; set; }
}
