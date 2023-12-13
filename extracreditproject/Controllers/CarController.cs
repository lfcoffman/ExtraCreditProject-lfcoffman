using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace extracreditproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        // GET: api/Car
        [HttpGet]
        public List<Car> Get()
        {
            List<Car> myCars = new List<Car>();
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"SELECT ID, Make, Model, Date, Mileage, `Delete`, `Hold` FROM cars";
            using var cmd = new MySqlCommand(stm, con);
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    Car car = new Car
                    {
                        ID = reader.GetInt32("ID"),
                        Make = reader.GetString("Make"),
                        Model = reader.GetString("Model"),
                        Date = reader.GetString("Date"),
                        Mileage = reader.GetInt32("Mileage"),
                        Delete = reader.GetBoolean("Delete"),
                        Hold = reader.GetBoolean("Hold")
                    };
                    myCars.Add(car);
                }
            }
            con.Close();
            return myCars;
        }

        // GET: api/Car/5
        [HttpGet("{id}", Name = "Get")]
        public Car Get(int id)
                    {
                        Car myCar = new Car();
                        ConnectionString myConnection = new ConnectionString();
                        string cs = myConnection.cs;
                        using var con = new MySqlConnection(cs);
                        con.Open();

                        string stm = @"SELECT ID, Make, Model, Date, Mileage, `Delete`, `Hold` FROM cars WHERE ID = @id";
                        
                        using var cmd = new MySqlCommand(stm, con);
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the existing 'myCar' object instead of creating a new 'Car' object
                                myCar.ID = reader.GetInt32("ID");
                                myCar.Make = reader.GetString("Make");
                                myCar.Model = reader.GetString("Model");
                                myCar.Date = reader.GetString("Date");
                                myCar.Mileage = reader.GetInt32("Mileage");
                                myCar.Delete = reader.GetBoolean("Delete");
                                myCar.Hold = reader.GetBoolean("Hold");
                            }
                        }
                        con.Close();
                        return myCar;
                    }

        // POST: api/Car
        [HttpPost]
        public void Post([FromBody] Car myCar)
        {
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO cars(make, model, date, mileage) values(@make, @model, @date, @mileage)";
            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@make", myCar.Make);
            cmd.Parameters.AddWithValue("@model", myCar.Model);
            cmd.Parameters.AddWithValue("@date", myCar.Date);
            cmd.Parameters.AddWithValue("@mileage", myCar.Mileage);
            cmd.ExecuteNonQuery();
        }

        // PUT: api/Car/5
        [HttpPut("{id}")]
        public void Put([FromBody] Car myCar)
        {
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open();
            string stm = @"UPDATE cars Set `Hold` = @hold WHERE ID = @id";
            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", myCar.ID);
            cmd.Parameters.AddWithValue("@hold", myCar.Hold);
            cmd.ExecuteNonQuery();
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            using var con = new MySqlConnection(cs);
            con.Open();
            string stm = @"UPDATE cars Set `Delete` = true WHERE ID = @id";
            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
