using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class PostDetails : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BlogPlatformDB"].ConnectionString;
        protected int PostId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (PostId <= 0) Response.Redirect("Default.aspx");
                LoadPost();
                LoadComments();
                pnlAddComment.Visible = Session["UserID"] != null;
                pnlLoginAsk.Visible = Session["UserID"] == null;
            }
        }

        private void LoadPost()
        {
            using (var conn = new SqlConnection(connStr))
            {
                var sql = @"SELECT p.Title, p.Content, p.CreatedAt, u.Username FROM Posts p
                            JOIN Users u ON p.AuthorID = u.UserID WHERE p.PostID=@id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", PostId);
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            litTitle.Text = Server.HtmlEncode(rdr["Title"].ToString());
                            litAuthor.Text = Server.HtmlEncode(rdr["Username"].ToString());
                            litDate.Text = Convert.ToDateTime(rdr["CreatedAt"]).ToString("yyyy-MM-dd HH:mm");
                            // Content may contain HTML; for demo we render as-is. In production sanitize.
                            litContent.Text = rdr["Content"].ToString();
                        }
                        else
                        {
                            Response.Redirect("Default.aspx");
                        }
                    }
                }
            }
        }

        private void LoadComments()
        {
            using (var conn = new SqlConnection(connStr))
            {
                var sql = @"SELECT c.Content, c.CreatedAt, u.Username FROM Comments c
                            JOIN Users u ON c.UserID = u.UserID
                            WHERE c.PostID=@id ORDER BY c.CreatedAt ASC";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", PostId);
                    var dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    rptComments.DataSource = dt;
                    rptComments.DataBind();
                }
            }
        }

        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) { Response.Redirect("Login.aspx"); return; }
            var content = txtComment.Text.Trim();
            if (string.IsNullOrEmpty(content)) { lblCommentMsg.Text = "Nội dung không được rỗng."; return; }

            using (var conn = new SqlConnection(connStr))
            {
                var sql = "INSERT INTO Comments (Content, CreatedAt, PostID, UserID) VALUES (@c,GETDATE(),@p,@u)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@c", content);
                    cmd.Parameters.AddWithValue("@p", PostId);
                    cmd.Parameters.AddWithValue("@u", (int)Session["UserID"]);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    txtComment.Text = "";
                    lblCommentMsg.Text = "Đã gửi bình luận.";
                    LoadComments();
                }
            }
        }
    }
}