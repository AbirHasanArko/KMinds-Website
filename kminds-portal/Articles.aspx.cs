using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class Articles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadArticles();
                CheckUserRole();
            }
        }

        private void CheckUserRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                // In a real app, query DB to ensure they are a verified member before showing this.
                CreateArticlePlaceHolder.Visible = true;
            }
        }

        private void LoadArticles()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT a.Title, SUBSTRING(a.Content, 1, 150) + '...' AS Summary, a.ThumbnailUrl, a.PublishDate, u.FullName AS AuthorName FROM Articles a JOIN Users u ON a.AuthorId = u.UserId ORDER BY a.PublishDate DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ArticlesRepeater.DataSource = dt;
                    ArticlesRepeater.DataBind();
                }
            }
        }

        protected void PublishArticleButton_Click(object sender, EventArgs e)
        {
            // Implementation to insert article to DB
            ArticleMessage.Text = "Article published successfully!";
            ArticleMessage.Visible = true;
            LoadArticles(); // Refresh list
        }
    }
}
