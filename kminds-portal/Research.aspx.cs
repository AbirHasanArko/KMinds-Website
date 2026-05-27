using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class Research : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadResearch();
                if (User.Identity.IsAuthenticated)
                {
                    SubmitResearchPlaceHolder.Visible = true;
                }
            }
        }

        private void LoadResearch()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT r.Title, r.Abstract, r.DownloadLink, r.PublishDate, '' AS ThumbnailUrl, u.FullName AS AuthorName FROM Research r JOIN Users u ON r.AuthorId = u.UserId ORDER BY r.PublishDate DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ResearchRepeater.DataSource = dt;
                    ResearchRepeater.DataBind();
                }
            }
        }

        protected void ShareResearchButton_Click(object sender, EventArgs e)
        {
            ResearchMessage.Text = "Research item submitted successfully!";
            ResearchMessage.Visible = true;
            LoadResearch();
        }
    }
}
