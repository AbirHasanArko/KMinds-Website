using System;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStats();
                LoadRecentHighlights();
            }
        }

        private void LoadStats()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users", conn))
                {
                    MemberCountLiteral.Text = cmd.ExecuteScalar().ToString();
                }
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Research", conn))
                {
                    ResearchCountLiteral.Text = cmd.ExecuteScalar().ToString();
                }
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Datasets", conn))
                {
                    DatasetCountLiteral.Text = cmd.ExecuteScalar().ToString();
                }
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Events", conn))
                {
                    EventCountLiteral.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }

        private void LoadRecentHighlights()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // 1. Recent Event
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 EventId, Title, Description, EventDate, ImageUrl FROM Events ORDER BY EventId DESC", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string title = reader["Title"].ToString();
                        string desc = reader["Description"].ToString();
                        if (desc.Length > 100) desc = desc.Substring(0, 100) + "...";
                        string date = Convert.ToDateTime(reader["EventDate"]).ToString("MMM dd, yyyy");
                        string img = reader["ImageUrl"].ToString();
                        if (string.IsNullOrEmpty(img)) img = "assets/images/event-placeholder.png";

                        RecentEventLiteral.Text = string.Format(@"
                        <div class='card'>
                          <img class='card-image' src='{0}' alt='{1}'>
                          <div class='card-body'>
                            <div class='card-meta'>
                              <span>🏆 Event</span><span>&middot; {2}</span>
                            </div>
                            <h3>{1}</h3>
                            <p>{3}</p>
                            <a href='Events.aspx' class='btn btn-secondary btn-sm' style='margin-top:0.5rem'>View Events &rarr;</a>
                          </div>
                        </div>", img, title, date, desc);
                    }
                }

                // 2. Recent Article
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 a.ArticleId, a.Title, a.Content, a.PublishDate, a.ThumbnailUrl, u.FullName FROM Articles a JOIN Users u ON a.AuthorId = u.UserId ORDER BY a.ArticleId DESC", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string title = reader["Title"].ToString();
                        string content = reader["Content"].ToString();
                        if (content.Length > 100) content = content.Substring(0, 100) + "...";
                        string date = Convert.ToDateTime(reader["PublishDate"]).ToString("MMM dd, yyyy");
                        string author = reader["FullName"].ToString();
                        string img = reader["ThumbnailUrl"].ToString();
                        if (string.IsNullOrEmpty(img)) img = "assets/images/article-preview.png";

                        RecentArticleLiteral.Text = string.Format(@"
                        <div class='card'>
                          <img class='card-image' src='{0}' alt='{1}'>
                          <div class='card-body'>
                            <div class='card-meta'>
                              <span>📝 Article</span><span>&middot; By {2}</span>
                            </div>
                            <h3>{1}</h3>
                            <p>{3}</p>
                            <a href='Articles.aspx' class='btn btn-secondary btn-sm' style='margin-top:0.5rem'>Read Articles &rarr;</a>
                          </div>
                        </div>", img, title, author, content);
                    }
                }

                // 3. Recent Research
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 r.ResearchId, r.Title, r.Abstract, r.PublishDate, u.FullName, ISNULL(r.ThumbnailUrl, 'assets/images/research-preview.png') AS ThumbnailUrl FROM Research r JOIN Users u ON r.AuthorId = u.UserId ORDER BY r.ResearchId DESC", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string title = reader["Title"].ToString();
                        string abstractText = reader["Abstract"].ToString();
                        if (abstractText.Length > 100) abstractText = abstractText.Substring(0, 100) + "...";
                        string author = reader["FullName"].ToString();
                        string img = reader["ThumbnailUrl"].ToString();
                        if (string.IsNullOrEmpty(img)) img = "assets/images/research-preview.png";

                        RecentResearchLiteral.Text = string.Format(@"
                        <div class='card'>
                          <img class='card-image' src='{0}' alt='{1}'>
                          <div class='card-body'>
                            <div class='card-meta'>
                              <span>🔬 Research</span><span>&middot; {2}</span>
                            </div>
                            <h3>{1}</h3>
                            <p>{3}</p>
                            <a href='Research.aspx' class='btn btn-secondary btn-sm' style='margin-top:0.5rem'>View Research &rarr;</a>
                          </div>
                        </div>", img, title, author, abstractText);
                    }
                }
            }
        }
    }
}
