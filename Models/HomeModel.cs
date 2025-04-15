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

        SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectModels;Initial Catalog=HopeConnect;Integrated Security=True");

        public bool Insert(PostModel post)
        {
            string query = @"INSERT INTO Posts (UserId, ThoughtText, PhotoPath, Latitude, Longitude, LocationName, Organization)
                             VALUES (@UserId, @ThoughtText, @PhotoPath, @Latitude, @Longitude, @LocationName, @Organization)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", post.UserId);
            cmd.Parameters.AddWithValue("@ThoughtText", post.ThoughtText);
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
    }
}
