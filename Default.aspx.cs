using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class Default : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BlogPlatformDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadPosts();
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
                    rptCategories.DataSource = dt;
                    rptCategories.DataBind();
                }
            }
        }

        private void LoadPosts()
        {
            int? cat = null;
            if (int.TryParse(Request.QueryString["cat"], out int c)) cat = c;

            using (var conn = new SqlConnection(connStr))
            {
                var sql = @"SELECT p.PostID, p.Title, p.Content, p.CreatedAt, u.Username
                            FROM Posts p
                            JOIN Users u ON p.AuthorID = u.UserID
                            WHERE p.Status = 1";

                if (cat.HasValue)
                {
                    sql += " AND p.CategoryID = @cat";
                }
                sql += " ORDER BY p.CreatedAt DESC";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (cat.HasValue) cmd.Parameters.AddWithValue("@cat", cat.Value);
                    var dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    // create excerpt
                    foreach (DataRow r in dt.Rows)
                    {
                        var content = r["Content"].ToString();
                        var excerpt = StripHtml(content);
                        if (excerpt.Length > 300) excerpt = excerpt.Substring(0, 300) + "...";
                        r["Content"] = excerpt;
                    }
                    // map fields for repeater: Excerpt property
                    var mapped = new DataTable();
                    mapped.Columns.Add("PostID");
                    mapped.Columns.Add("Title");
                    mapped.Columns.Add("Username");
                    mapped.Columns.Add("CreatedAt");
                    mapped.Columns.Add("Excerpt");
                    foreach (DataRow r in dt.Rows)
                    {
                        mapped.Rows.Add(r["PostID"], r["Title"], r["Username"], r["CreatedAt"], r["Content"]);
                    }
                    rptPosts.DataSource = mapped;
                    rptPosts.DataBind();
                }
            }
        }

        private string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}