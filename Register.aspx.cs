using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class Register : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BlogPlatformDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            var user = txtUser.Text.Trim();
            var email = txtEmail.Text.Trim();
            var pass = txtPass.Text;
            var full = txtFullName.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                lblMsg.Text = "Vui lòng điền đầy đủ thông tin.";
                return;
            }

            var passHash = ComputeSha256Hash(pass);

            using (var conn = new SqlConnection(connStr))
            {
                var sql = "INSERT INTO Users (Username, Email, PasswordHash, FullName) VALUES (@u,@e,@p,@f); SELECT SCOPE_IDENTITY();";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", passHash);
                    cmd.Parameters.AddWithValue("@f", string.IsNullOrEmpty(full) ? (object)DBNull.Value : full);
                    conn.Open();
                    try
                    {
                        var id = cmd.ExecuteScalar();
                        // assign role 'User'
                        var assignRoleSql = @"INSERT INTO UserRoles (UserID, RoleID) 
                                              SELECT @uid, RoleID FROM Roles WHERE RoleName='User'";
                        using (var cmd2 = new SqlCommand(assignRoleSql, conn))
                        {
                            cmd2.Parameters.AddWithValue("@uid", Convert.ToInt32(id));
                            cmd2.ExecuteNonQuery();
                        }

                        Session["UserID"] = Convert.ToInt32(id);
                        Session["Username"] = user;
                        Response.Redirect("Default.aspx");
                    }
                    catch (SqlException ex)
                    {
                        lblMsg.Text = "Lỗi: có thể username hoặc email đã tồn tại.";
                    }
                }
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var sb = new StringBuilder();
                foreach (var t in bytes) sb.Append(t.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}