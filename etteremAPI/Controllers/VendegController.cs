using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics.Eventing.Reader;
using etteremAPI.Models;
using etteremAPI.Models.DTOs;

namespace etteremAPI.Controllers
{

    [Route("vendeg")]

    [ApiController]
    public class VendegController : ControllerBase
    {
        public string ConnectionString = "server=localhost;user=root;password=;database=etterem";

        [HttpGet("bynameandemail")]
        public object GetVendegByNameAndEmail(int id)
        {   
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();
            string sql = @"SELECT * FROM vendeg WHERE id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            reader.Read();

            var vendeg = new Vendeg
            {
                age = reader.GetInt32(0),
                email = reader.GetString(1),
                id = reader.GetInt32(2),
                name = reader.GetString(3),
                password = reader.GetString(4),
                registrationTime = reader.GetDateTime(5)


            };


            return new { message = "sikeres talalat", result = vendeg };
        }


        [HttpGet("vendegname")]

        public object GetVendegNameAndDishesDescriptions(Vendeg vendeg)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT vendeg.name, rendeles.description FROM vendeg JOIN rendeles ON vendeg.id = rendeles.VendegId WHERE vendeg.id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", vendeg.id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var istenem = new VendegNameAndDishesDescriptions
            {
                name = reader.GetString(0),
                description = reader.GetString(1)
            };
            return new { message = "sikeres talalat", result = istenem };
        }

    }
}
