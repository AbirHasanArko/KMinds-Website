using System;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            string email = User.Identity.Name;
            UserNameLiteral.Text = email; // Fallback

            int userId = 0;
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserId, FullName, Role FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = Convert.ToInt32(reader["UserId"]);
                                UserNameLiteral.Text = reader["FullName"].ToString();
                                string role = reader["Role"].ToString().ToLower();
                                
                                // Show admin panel for officers
                                if (role == "president" || role == "admin" || role == "vice-president" || role == "general-secretary" || role == "treasurer")
                                {
                                    AdminActionsPlaceHolder.Visible = true;
                                }
                            }
                        }

                        if (userId > 0)
                        {
                            using (SqlCommand cmdArticles = new SqlCommand("SELECT COUNT(*) FROM Articles WHERE AuthorId = @UserId", conn))
                            {
                                cmdArticles.Parameters.AddWithValue("@UserId", userId);
                                ArticlesCountLiteral.Text = cmdArticles.ExecuteScalar().ToString();
                            }

                            using (SqlCommand cmdResearch = new SqlCommand("SELECT COUNT(*) FROM Research WHERE AuthorId = @UserId", conn))
                            {
                                cmdResearch.Parameters.AddWithValue("@UserId", userId);
                                ResearchCountLiteral.Text = cmdResearch.ExecuteScalar().ToString();
                            }

                            using (SqlCommand cmdDatasets = new SqlCommand("SELECT COUNT(*) FROM Datasets WHERE UploaderId = @UserId", conn))
                            {
                                cmdDatasets.Parameters.AddWithValue("@UserId", userId);
                                DatasetsCountLiteral.Text = cmdDatasets.ExecuteScalar().ToString();
                            }
                        }

                        using (SqlCommand cmdEvents = new SqlCommand("SELECT COUNT(*) FROM Events WHERE EventDate >= GETDATE()", conn))
                        {
                            EventsCountLiteral.Text = cmdEvents.ExecuteScalar().ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle DB error silently or log
                    }
                }
            }
        }
    }
}
