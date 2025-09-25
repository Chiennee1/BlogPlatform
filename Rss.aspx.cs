using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace BlogPlatform
{
    public partial class Rss : Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["BlogPlatformDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/rss+xml; charset=utf-8";
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<rss version=\"2.0\">");
            sb.AppendLine("<channel>");
            sb.AppendLine("<title>My Blog Platform - RSS</title>");
            sb.AppendLine("<link>" + Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "</link>");
            sb.AppendLine("<description>RSS feed của Blog Platform</description>");

            using (var conn = new SqlConnection(connStr))
            {
                var sql = @"SELECT TOP 20 p.PostID, p.Title, p.Content, p.CreatedAt FROM Posts p WHERE p.Status=1 ORDER BY p.CreatedAt DESC";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var id = rdr["PostID"].ToString();
                            var title = System.Security.SecurityElement.Escape(rdr["Title"].ToString());
                            var content = System.Security.SecurityElement.Escape(rdr["Content"].ToString());
                            var date = Convert.ToDateTime(rdr["CreatedAt"]).ToString("r");
                            sb.AppendLine("<item>");
                            sb.AppendLine("<title>" + title + "</title>");
                            sb.AppendLine("<link>" + Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/PostDetails.aspx?id=" + id) + "</link>");
                            sb.AppendLine("<pubDate>" + date + "</pubDate>");
                            sb.AppendLine("<description><![CDATA[" + rdr["Content"].ToString() + "]]></description>");
                            sb.AppendLine("</item>");
                        }
                    }
                }
            }

            sb.AppendLine("</channel>");
            sb.AppendLine("</rss>");
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}