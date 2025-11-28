using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace GKFashionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FashionController : ControllerBase
    {
        // Connection String: read from env var `DB_CONNECTION` on Render (fallback to local)
        private string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
            ?? "Server=gateway01.ap-southeast-1.prod.aws.tidbcloud.com;Port=4000;Database=gkfashion_db;Uid=3nccSBxwtHzCDoP.root;Pwd=W3GgF0y5FTMwcUDm;SslMode=Required;";

        // ✅ NEW: Server Test Endpoint (Database ki zaroorat nahi)
        // URL: http://localhost:xxxx/api/fashion/test
        [HttpGet("test")]
        public IActionResult TestConnection()
        {
            return Ok("🎉 API is working perfectly! Server is connected.");
        }

        // 1. DATA LANA (GET from DB)
        // URL: http://localhost:xxxx/api/fashion
        [HttpGet]
        public IActionResult GetItems()
        {
            List<FashionItem> items = new List<FashionItem>();
            try 
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM FashionItems", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new FashionItem
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Size = reader["Size"].ToString(),
                                Price = Convert.ToDouble(reader["Price"]),
                                ImageUrl = reader["ImageUrl"].ToString(),
                                Description = reader["Description"].ToString()
                            });
                        }
                    }
                }
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Database Error: " + ex.Message);
            }
        }

        // ... (Baaki AddItem aur DeleteItem waisa hi rahega) ...
        [HttpPost]
        public IActionResult AddItem([FromBody] FashionItem item)
        {
             // ... (Tumhara purana code) ...
             try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO FashionItems (Name, Size, Price, ImageUrl, Description) VALUES (@Name, @Size, @Price, @Img, @Desc)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Size", item.Size);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Img", item.ImageUrl);
                    cmd.Parameters.AddWithValue("@Desc", item.Description);
                    cmd.ExecuteNonQuery();
                }
                return Ok(new { message = "Item Added Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Save Error: " + ex.Message);
            }
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteItem(int id)
        {
             // ... (Tumhara purana code) ...
             try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM FashionItems WHERE Id=@Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                return Ok(new { message = "Deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Delete Error: " + ex.Message);
            }
        }
    }
}