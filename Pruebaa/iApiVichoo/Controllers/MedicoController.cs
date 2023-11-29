using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace PersonApi.Controllers
{
    [Route("[controller]")]
    public class MedicoController : Controller
    {
        private readonly String StringConector;

        public MedicoController(IConfiguration config)
        {
            StringConector = config.GetConnectionString("MySqlConnection");
        }

        [HttpGet]
        public async Task<IActionResult> ListarMedicos()
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM MEDICO";

                    List<Medico> medicos = new List<Medico>();

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    using (var lector = await comandos.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            medicos.Add(new Medico
                            {
                                IdMedico = lector.GetInt32(0),
                                NombreMed = lector.GetString(1),
                                ApellidoMed = lector.GetString(2),
                                RunMed = lector.GetString(3),
                                Eunacom = lector.GetString(4),
                                NacionalidadMed = lector.GetString(5),
                                Especialidad = lector.GetString(6),
                                Horarios = lector.GetString(7),
                                TarifaHr = lector.GetInt32(8)
                            });
                        }

                        return StatusCode(200, medicos);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo listar los registros por: " + ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarMedico(int id)
        {
            try
            {
                using (MySqlConnection conectar = new MySqlConnection(StringConector))
                {
                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM MEDICO WHERE IDMEDICO = @id";

                    Medico medico = new Medico();

                    using (MySqlCommand comandos = new MySqlCommand(sentencia, conectar))
                    {
                        comandos.Parameters.AddWithValue("@id", id);

                        using (var lector = await comandos.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                medico.IdMedico = lector.GetInt32(0);
                                medico.NombreMed = lector.GetString(1);
                                medico.ApellidoMed = lector.GetString(2);
                                medico.RunMed = lector.GetString(3);
                                medico.Eunacom = lector.GetString(4);
                                medico.NacionalidadMed = lector.GetString(5);
                                medico.Especialidad = lector.GetString(6);
                                medico.Horarios = lector.GetString(7);
                                medico.TarifaHr = lector.GetInt32(8);

                                return StatusCode(200, medico);
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

    }
}
