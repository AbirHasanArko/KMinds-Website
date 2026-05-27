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

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT FullName, Role FROM Users WHERE Email = @Email";
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
                                UserNameLiteral.Text = reader["FullName"].ToString();
                                string role = reader["Role"].ToString().ToLower();
                                
                                // Show admin panel for officers
                                if (role == "president" || role == "vice-president" || role == "general-secretary" || role == "treasurer")
                                {
                                    AdminActionsPlaceHolder.Visible = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle DB error
                    }
                }
            }
            
            // In a complete implementation, you'd add more queries here to populate:
            // ArticlesCountLiteral.Text
            // ResearchCountLiteral.Text
            // DatasetsCountLiteral.Text
            // EventsCountLiteral.Text
        }
    }
}
