using Microsoft.Data.SqlClient;

namespace CurdNew.Models
{
    public class HomeModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public bool IsEmailVerified { get; set; }
        public string EmailVerificationToken { get; set; }

        SqlConnection con = new SqlConnection("Data Source=(localdb)\\ProjectModels;Initial Catalog=HopeConnect;Integrated Security=True");

        // Insert new user
        public bool Insert(HomeModel home)
        {
            string query = "INSERT INTO Users (Name, Email, MobileNo, Password, IsEmailVerified, EmailVerificationToken) " +
                           "VALUES (@Name, @Email, @MobileNo, @Password, @IsEmailVerified, @EmailVerificationToken)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", home.Name);
            cmd.Parameters.AddWithValue("@Email", home.Email);
            cmd.Parameters.AddWithValue("@MobileNo", home.MobileNo);
            cmd.Parameters.AddWithValue("@Password", home.Password);
            cmd.Parameters.AddWithValue("@IsEmailVerified", home.IsEmailVerified);
            cmd.Parameters.AddWithValue("@EmailVerificationToken", home.EmailVerificationToken);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i > 0;
        }

        // Get user using token
        public HomeModel GetUserByToken(string token)
        {
            string query = "SELECT * FROM Users WHERE EmailVerificationToken = @Token";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Token", token);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            HomeModel user = null;

            if (reader.Read())
            {
                user = new HomeModel
                {
                    UserId = (int)reader["UserId"],  // Changed from Id
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

        // Update verification status
        public bool UpdateEmailVerification(HomeModel user)
        {
            string query = "UPDATE Users SET IsEmailVerified = @IsEmailVerified WHERE EmailVerificationToken = @Token";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@IsEmailVerified", user.IsEmailVerified);
            cmd.Parameters.AddWithValue("@Token", user.EmailVerificationToken);

            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();

            return result > 0;
        }

        public HomeModel GetUserByEmailAndPassword(string email, string password)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password AND IsEmailVerified = 1";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

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
