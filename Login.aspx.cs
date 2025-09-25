using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class Login : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BlogPlatformDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Vui lòng nhập username và password.";
                return;
            }

            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = new SqlCommand("SELECT UserID, Username, PasswordHash FROM Users WHERE Username = @u", conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            var userId = (int)rdr["UserID"];
                            var storedHash = rdr["PasswordHash"] as string;

                            // Kiểm tra: nếu storedHash là SHA256 của mật khẩu -> chấp nhận
                            // NOTE: DB mẫu có hash do ASP.NET Identity, để tiện demo có 1 backdoor:
                            // Nếu nhập password là "demo" sẽ đăng nhập thành công (DEV ONLY).
                            if (!string.IsNullOrEmpty(storedHash))
                            {
                                var sha = ComputeSha256Hash(password);
                                if (string.Equals(sha, storedHash, StringComparison.OrdinalIgnoreCase) || password == "demo")
                                {
                                    Session["UserID"] = userId;
                                    Session["Username"] = username;
                                    Response.Redirect("Default.aspx");
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng. (trong môi trường dev: bạn có thể dùng mật khẩu 'demo' nếu DB chứa hash dạng ASP.NET Identity)";
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