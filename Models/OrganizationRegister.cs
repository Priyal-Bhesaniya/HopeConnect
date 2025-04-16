using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace HopeConnect.Models
{
    public class OrganizationModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrganizationModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration?.GetConnectionString("DefaultConnection") ?? "your-default-fallback-string";
        }

        public int OrgId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public bool IsEmailVerified { get; set; }
        public string EmailVerificationToken { get; set; }

        public bool Insert(OrganizationModel org)
        {
            if (Exists(org.Email)) return false;

            string query = "INSERT INTO OrganizationRegister (Name, Email, MobileNo, Password, Type, IsEmailVerified, EmailVerificationToken) " +
                           "VALUES (@Name, @Email, @MobileNo, @Password, @Type, @IsEmailVerified, @EmailVerificationToken)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Name", org.Name);
                cmd.Parameters.AddWithValue("@Email", org.Email);
                cmd.Parameters.AddWithValue("@MobileNo", org.MobileNo);
                cmd.Parameters.AddWithValue("@Password", org.Password);
                cmd.Parameters.AddWithValue("@Type", org.Type);
                cmd.Parameters.AddWithValue("@IsEmailVerified", org.IsEmailVerified);
                cmd.Parameters.AddWithValue("@EmailVerificationToken", org.EmailVerificationToken);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Exists(string email)
        {
            string query = "SELECT COUNT(*) FROM OrganizationRegister WHERE Email = @Email";
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public OrganizationModel GetByEmailAndPassword(string email, string password)
        {
            string query = "SELECT * FROM OrganizationRegister WHERE Email = @Email AND IsEmailVerified = 1";
            if (!string.IsNullOrEmpty(password))
                query += " AND Password = @Password";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                if (!string.IsNullOrEmpty(password))
                    cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrganizationModel(_configuration)
                        {
                            OrgId = (int)reader["OrgId"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNo = reader["MobileNo"].ToString(),
                            Password = reader["Password"].ToString(),
                            Type = reader["Type"].ToString(),
                            IsEmailVerified = (bool)reader["IsEmailVerified"],
                            EmailVerificationToken = reader["EmailVerificationToken"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public OrganizationModel GetByToken(string token)
        {
            string query = "SELECT * FROM OrganizationRegister WHERE EmailVerificationToken = @Token";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Token", token);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrganizationModel(_configuration)
                        {
                            OrgId = (int)reader["OrgId"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNo = reader["MobileNo"].ToString(),
                            Password = reader["Password"].ToString(),
                            Type = reader["Type"].ToString(),
                            IsEmailVerified = (bool)reader["IsEmailVerified"],
                            EmailVerificationToken = reader["EmailVerificationToken"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public OrganizationModel GetByEmail(string email)
        {
            string query = "SELECT * FROM OrganizationRegister WHERE Email = @Email";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrganizationModel(_configuration)
                        {
                            OrgId = (int)reader["OrgId"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNo = reader["MobileNo"].ToString(),
                            Password = reader["Password"].ToString(),
                            Type = reader["Type"].ToString(),
                            IsEmailVerified = (bool)reader["IsEmailVerified"],
                            EmailVerificationToken = reader["EmailVerificationToken"].ToString()
                        };
                    }
                }
            }
            return null;
        }
        public bool VerifyEmail(string token)
        {
            string query = "UPDATE OrganizationRegister SET IsEmailVerified = 1 WHERE EmailVerificationToken = @Token";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Token", token);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}