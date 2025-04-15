using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HopeConnect.Models
{
    public class PostModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string ThoughtText { get; set; }
        public string PhotoPath { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationName { get; set; }
        public string Organization { get; set; }
        public DateTime CreatedAt { get; set; }

        // Extra property to show user name from join
        public string Name { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectModels;Initial Catalog=HopeConnect;Integrated Security=True");

        // Insert a new post
        public bool Insert(PostModel post)
        {
            string query = @"INSERT INTO Posts (UserId, ThoughtText, PhotoPath, Latitude, Longitude, LocationName, Organization, CreatedAt)
                             VALUES (@UserId, @ThoughtText, @PhotoPath, @Latitude, @Longitude, @LocationName, @Organization, GETDATE())";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", post.UserId);
            cmd.Parameters.AddWithValue("@ThoughtText", post.ThoughtText ?? "");
            cmd.Parameters.AddWithValue("@PhotoPath", post.PhotoPath ?? "");
            cmd.Parameters.AddWithValue("@Latitude", post.Latitude);
            cmd.Parameters.AddWithValue("@Longitude", post.Longitude);
            cmd.Parameters.AddWithValue("@LocationName", post.LocationName ?? "");
            cmd.Parameters.AddWithValue("@Organization", post.Organization ?? "");

            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();

            return result > 0;
        }

        // Fetch all posts (with user name)
        public List<PostModel> GetAllPosts()
        {
            List<PostModel> posts = new List<PostModel>();
            string query = @"
                SELECT P.PostId, P.UserId, P.ThoughtText, P.PhotoPath, P.Latitude, P.Longitude, P.LocationName, 
                       P.Organization, P.CreatedAt, U.Name 
                FROM Posts P 
                INNER JOIN Users U ON P.UserId = U.UserId
                ORDER BY P.CreatedAt DESC";

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            HomeModel user = null;

            if (reader.Read())
            {
                user = new HomeModel
                {
                    UserId = (int)reader["UserId"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Password = reader["Password"].ToString(),
                    IsEmailVerified = (bool)reader["IsEmailVerified"],
                    EmailVerificationToken = reader["EmailVerificationToken"].ToString()
                };
            }

            con.Close();
            return user;
        }

        // Get user by email (for Profile fetching)
        public HomeModel GetUserByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            HomeModel user = null;

            if (reader.Read())
            {
                user = new HomeModel
                {
                    UserId = (int)reader["UserId"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Password = reader["Password"].ToString(),
                    IsEmailVerified = (bool)reader["IsEmailVerified"],
                    EmailVerificationToken = reader["EmailVerificationToken"].ToString()
                };
            }

            con.Close();
            return user;
        }

        // Update user profile (Name, MobileNo, Password) using email
        public bool UpdateUserProfile(HomeModel user)
        {
            string query = "UPDATE Users SET Name = @Name, MobileNo = @MobileNo, Password = @Password WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@MobileNo", user.MobileNo);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();

            return result > 0;
        }


    }
}
