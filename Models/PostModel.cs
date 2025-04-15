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

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                posts.Add(new PostModel
                {
                    PostId = (int)reader["PostId"],
                    UserId = (int)reader["UserId"],
                    ThoughtText = reader["ThoughtText"].ToString(),
                    PhotoPath = reader["PhotoPath"].ToString(),
                    Latitude = Convert.ToDouble(reader["Latitude"]),
                    Longitude = Convert.ToDouble(reader["Longitude"]),
                    LocationName = reader["LocationName"].ToString(),
                    Organization = reader["Organization"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    Name = reader["Name"].ToString()
                });
            }

            con.Close();
            return posts;
        }
    }
}



