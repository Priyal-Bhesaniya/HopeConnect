using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CurdNew.Models
{
    public class HomeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectModels;Initial Catalog=HopeConnect;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


        // Insert User
        public bool Insert(HomeModel home)
        {
            string query = "INSERT INTO Users (Name, Email, MobileNo, Password) VALUES (@Name, @Email, @MobileNo, @Password)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", home.Name);
            cmd.Parameters.AddWithValue("@Email", home.Email);
            cmd.Parameters.AddWithValue("@MobileNo", home.MobileNo);
            cmd.Parameters.AddWithValue("@Password", home.Password);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i > 0;
        }

        //  Update User by ID
        public bool Update(HomeModel home)
        {
            string query = "UPDATE Users SET Name=@Name, Email=@Email, MobileNo=@MobileNo, Password=@Password WHERE Id=@Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", home.Id);
            cmd.Parameters.AddWithValue("@Name", home.Name);
            cmd.Parameters.AddWithValue("@Email", home.Email);
            cmd.Parameters.AddWithValue("@MobileNo", home.MobileNo);
            cmd.Parameters.AddWithValue("@Password", home.Password);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i > 0;
        }

        //  Delete User by ID
        public bool Delete(int id)
        {
            string query = "DELETE FROM Users WHERE Id=@Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i > 0;
        }

        //  Get User by ID
        public HomeModel GetHomeById(int id)
        {
            string query = "SELECT * FROM Users WHERE Id=@Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            HomeModel home = null;
            if (reader.Read())
            {
                home = new HomeModel
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Password = reader["Password"].ToString()
                };
            }
            con.Close();
            return home;
        }

        //  Get All Users
        public List<HomeModel> GetAllHomes()
        {
            List<HomeModel> lsthome = new List<HomeModel>();
            string query = "SELECT * FROM Users";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lsthome.Add(new HomeModel
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Password = reader["Password"].ToString()
                });
            }
            con.Close();
            return lsthome;
        }
    }
}
