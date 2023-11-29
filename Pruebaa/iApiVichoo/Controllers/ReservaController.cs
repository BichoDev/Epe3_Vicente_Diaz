using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace PersonApi.Controllers
{
    [Route("[controller]")]
    public class ReservaController : Controller
    {
        private readonly String StringConector;

        public ReservaController(IConfiguration config)
        {
            StringConector = config.GetConnectionString("MySqlConnection");
        }

        [HttpGet]
        public async Task<IActionResult> ListarReservas()
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM RESERVA";

                    List<Reserva> reservas = new List<Reserva>();

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    using (var lector = await comandos.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            reservas.Add(new Reserva
                            {
                                IdReserva = lector.GetInt32(0),
                                Especialidad = lector.GetString(1),
                                DiaReserva = lector.GetString(2),
                                PacienteIdPaciente = lector.GetInt32(3)
                                // Assuming there is no direct reference to Paciente in the Reserva model
                            });
                        }

                        return StatusCode(200, reservas);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo listar los registros por: " + ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarReserva(int id)
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM RESERVA WHERE IDRESERVA = @id";

                    Reserva reserva = new Reserva();

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    {
                        comandos.Parameters.AddWithValue("@id", id);

                        using (var lector = await comandos.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                reserva.IdReserva = lector.GetInt32(0);
                                reserva.Especialidad = lector.GetString(1);
                                reserva.DiaReserva = lector.GetString(2);
                                reserva.PacienteIdPaciente = lector.GetInt32(3);

                                return StatusCode(200, reserva);
                            }
                            else
                            {
                                return StatusCode(404, "No se encuentra el registro");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se puede realizar la petición por: " + ex);
            }
        }

        // Other CRUD methods can be added similarly.
    }
}
