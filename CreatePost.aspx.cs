using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class CreatePost : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BlogPlatformDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName", conn))
                {
                    var dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    ddlCategory.DataSource = dt;
                    ddlCategory.DataTextField = "CategoryName";
                    ddlCategory.DataValueField = "CategoryID";
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Không chọn--", ""));
                }
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            SavePost(1);
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            SavePost(0);
        }

        private void SavePost(int status)
        {
            var title = txtTitle.Text.Trim();
            var content = txtContent.Text.Trim();
            var catVal = ddlCategory.SelectedValue;
            int? catId = null;
            if (int.TryParse(catVal, out int cv)) catId = cv;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                lblInfo.Text = "Tiêu đề và nội dung là bắt buộc.";
                return;
            }

            using (var conn = new SqlConnection(connStr))
            {
                var sql = "INSERT INTO Posts (Title, Content, CreatedAt, Status, AuthorID, CategoryID) VALUES (@t,@c,GETDATE(),@s,@a,@cat)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@t", title);
                    cmd.Parameters.AddWithValue("@c", content);
                    cmd.Parameters.AddWithValue("@s", status);
                    cmd.Parameters.AddWithValue("@a", (int)Session["UserID"]);
                    if (catId.HasValue) cmd.Parameters.AddWithValue("@cat", catId.Value);
                    else cmd.Parameters.AddWithValue("@cat", DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblInfo.Text = status == 1 ? "Đã xuất bản." : "Đã lưu nháp.";
                    txtTitle.Text = "";
                    txtContent.Text = "";
                }
            }
        }
    }
}