using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace PersonApi.Controllers
{
    [Route("[controller]")]
    public class PacienteController : Controller
    {
        private readonly String StringConector;

        public PacienteController(IConfiguration config)
        {
            StringConector = config.GetConnectionString("MySqlConnection");
        }

        [HttpGet]
        public async Task<IActionResult> ListarPacientes()
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM PACIENTE";

                    List<Paciente> pacientes = new List<Paciente>();

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    using (var lector = await comandos.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            pacientes.Add(new Paciente
                            {
                                IdPaciente = lector.GetInt32(0),
                                NombrePac = lector.GetString(1),
                                ApellidoPac = lector.GetString(2),
                                RunPac = lector.GetString(3),
                                Nacionalidad = lector.GetString(4),
                                Visa = lector.GetString(5),
                                Genero = lector.GetString(6),
                                SintomasPac = lector.GetString(7),
                                MedicoIdMedico = lector.GetInt32(8)
                            });
                        }

                        return StatusCode(200, pacientes);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo listar los registros por: " + ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarPaciente(int id)
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM PACIENTE WHERE IDPACIENTE = @id";

                    Paciente paciente = new Paciente();

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    {
                        comandos.Parameters.AddWithValue("@id", id);

                        using (var lector = await comandos.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                paciente.IdPaciente = lector.GetInt32(0);
                                paciente.NombrePac = lector.GetString(1);
                                paciente.ApellidoPac = lector.GetString(2);
                                paciente.RunPac = lector.GetString(3);
                                paciente.Nacionalidad = lector.GetString(4);
                                paciente.Visa = lector.GetString(5);
                                paciente.Genero = lector.GetString(6);
                                paciente.SintomasPac = lector.GetString(7);
                                paciente.MedicoIdMedico = lector.GetInt32(8);

                                return StatusCode(200, paciente);
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

        [HttpPost]
        public async Task<IActionResult> CrearPaciente([FromBody] Paciente paciente)
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "INSERT INTO PACIENTE (NOMBREPAC, APELLIDOPAC, RUNPAC, NACIONALIDAD, VISA, GENERO, SINTOMASPAC, MEDICOIDMEDICO) VALUES (@nombrePac, @apellidoPac, @runPac, @nacionalidad, @visa, @genero, @sintomasPac, @medicoIdMedico)";

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    {
                        comandos.Parameters.AddWithValue("@nombrePac", paciente.NombrePac);
                        comandos.Parameters.AddWithValue("@apellidoPac", paciente.ApellidoPac);
                        comandos.Parameters.AddWithValue("@runPac", paciente.RunPac);
                        comandos.Parameters.AddWithValue("@nacionalidad", paciente.Nacionalidad);
                        comandos.Parameters.AddWithValue("@visa", paciente.Visa);
                        comandos.Parameters.AddWithValue("@genero", paciente.Genero);
                        comandos.Parameters.AddWithValue("@sintomasPac", paciente.SintomasPac);
                        comandos.Parameters.AddWithValue("@medicoIdMedico", paciente.MedicoIdMedico);

                        await comandos.ExecuteNonQueryAsync();

                        return StatusCode(201, $"Paciente creado correctamente: {paciente}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo crear el paciente por: " + ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] Paciente paciente)
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "UPDATE PACIENTE SET NOMBREPAC = @nombrePac, APELLIDOPAC = @apellidoPac, RUNPAC = @runPac, NACIONALIDAD = @nacionalidad, VISA = @visa, GENERO = @genero, SINTOMASPAC = @sintomasPac, MEDICOIDMEDICO = @medicoIdMedico WHERE IDPACIENTE = @id";

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    {
                        comandos.Parameters.AddWithValue("@nombrePac", paciente.NombrePac);
                        comandos.Parameters.AddWithValue("@apellidoPac", paciente.ApellidoPac);
                        comandos.Parameters.AddWithValue("@runPac", paciente.RunPac);
                        comandos.Parameters.AddWithValue("@nacionalidad", paciente.Nacionalidad);
                        comandos.Parameters.AddWithValue("@visa", paciente.Visa);
                        comandos.Parameters.AddWithValue("@genero", paciente.Genero);
                        comandos.Parameters.AddWithValue("@sintomasPac", paciente.SintomasPac);
                        comandos.Parameters.AddWithValue("@medicoIdMedico", paciente.MedicoIdMedico);
                        comandos.Parameters.AddWithValue("@id", id);

                        await comandos.ExecuteNonQueryAsync();

                        return StatusCode(200, "Paciente actualizado correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo actualizar el paciente por: " + ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "DELETE FROM PACIENTE WHERE IDPACIENTE = @id";

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    {
                        comandos.Parameters.AddWithValue("@id", id);

                        var borrado = await comandos.ExecuteNonQueryAsync();

                        if (borrado == 0)
                        {
                            return StatusCode(404, "Registro no encontrado!!!");
                        }
                        else
                        {
                            return StatusCode(200, $"Paciente con el ID {id} eliminado correctamente");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo eliminar el registro por: " + ex);
            }
        }
    }
}
