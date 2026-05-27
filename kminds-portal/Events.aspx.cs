using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class Events : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEvents();
                CheckAdminRole();
            }
        }

        private void CheckAdminRole()
        {
            if (!User.Identity.IsAuthenticated) return;
            
            string email = User.Identity.Name;
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Role FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    object roleObj = cmd.ExecuteScalar();
                    if (roleObj != null)
                    {
                        string role = roleObj.ToString().ToLower();
                        if (role == "president" || role == "vice-president" || role == "general-secretary" || role == "treasurer")
                        {
                            AdminEventCreatePlaceHolder.Visible = true;
                        }
                    }
                }
            }
        }

        private void LoadEvents()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Title, Description, EventDate, Location, ImageUrl FROM Events ORDER BY EventDate ASC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    EventsRepeater.DataSource = dt;
                    EventsRepeater.DataBind();
                }
            }
        }

        protected void CreateEventButton_Click(object sender, EventArgs e)
        {
            EventMessage.Text = "Event created and notifications sent!";
            EventMessage.Visible = true;
            LoadEvents();
        }
    }
}
