using etteremAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.ObjectPool;
using MySqlConnector;

namespace etteremAPI.Controllers
{
    [Route("rendeles")]
    [ApiController]
    public class RendelesController : ControllerBase
    {
        public string ConnectionString = "server=localhost;user=root;password=;database=etterem";

        [HttpGet("all")]

        public object GetAllRendeles()
        {
            List<Models.rendeles> rendelesek = new List<Models.rendeles>();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM rendeles", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.rendeles rendeles = new Models.rendeles
                            {
                                Id = reader.GetInt32("Id"),
                                Dish = reader.GetString("Dish"),
                                Description = reader.GetString("description"),
                                OrderTime = reader.GetDateTime("OrderTime"),
                                UpdateTIme = reader.GetDateTime("UpdateTIme"),
                                VendegId = reader.GetInt32("VendegId")
                            };
                            rendelesek.Add(rendeles);
                        }
                    }
                }
            }
            return rendelesek;
        }

        [HttpGet("getbyid")]
        public object GetRendelesById(int id)
        {
            using (var connector = new MySqlConnection(ConnectionString))
            {
                connector.Open();
                string sql = @"SELECT * FROM rendeles WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, connector))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var pofon = new rendeles
                        {
                            Id = reader.GetInt32("Id"),
                            Dish = reader.GetString("Dish"),
                            Description = reader.GetString("description"),
                            OrderTime = reader.GetDateTime("OrderTime"),
                            UpdateTIme = reader.GetDateTime("UpdateTIme"),
                            VendegId = reader.GetInt32("VendegId")
                        };

                        return pofon;
                    }
                }
            }
        }

        [HttpPost("add")]
        public object AddNewRendeles(Models.DTOs.AddNewrendeles rendeles)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO rendeles (Dish, description, OrderTime, UpdateTIme, VendegId) VALUES (@Dish, @description, @OrderTime, @UpdateTIme, @VendegId)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Dish", rendeles.Dish);
                    command.Parameters.AddWithValue("@description", rendeles.Description);
                    command.Parameters.AddWithValue("@OrderTime", DateTime.Now);
                    command.Parameters.AddWithValue("@UpdateTIme", DateTime.Now);
                    command.Parameters.AddWithValue("@VendegId", 1);
                    return ("sikeres felvetel");
                }
            }
        }
    }
    
}
