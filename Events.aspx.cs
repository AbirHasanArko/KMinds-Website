using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

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

        private bool IsUserAdmin()
        {
            if (!User.Identity.IsAuthenticated) return false;
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
                        return (role == "president" || role == "vice-president" || role == "general-secretary" || role == "treasurer");
                    }
                }
            }
            return false;
        }

        private void CheckAdminRole()
        {
            if (IsUserAdmin())
            {
                AdminEventCreatePlaceHolder.Visible = true;
                CheckEditMode();
            }
        }

        private void CheckEditMode()
        {
            if (Request.QueryString["edit_id"] != null)
            {
                string editId = Request.QueryString["edit_id"];
                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT Title, Description, EventDate, Location FROM Events WHERE EventId = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", editId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                EventTitleTextBox.Text = reader["Title"].ToString();
                                EventDescTextBox.Text = reader["Description"].ToString();
                                DateTime eventDate = Convert.ToDateTime(reader["EventDate"]);
                                EventDateTextBox.Text = eventDate.ToString("yyyy-MM-dd");
                                EventTimeTextBox.Text = eventDate.ToString("HH:mm");
                                EventLocationTextBox.Text = reader["Location"].ToString();
                                
                                CreateEventButton.Text = "Save Changes";
                            }
                        }
                    }
                }
            }
        }

        // Removed CheckAdminRole as it is moved above

        private void LoadEvents()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT EventId, Title, Description AS FullContent, SUBSTRING(Description, 1, 150) + CASE WHEN LEN(Description) > 150 THEN '...' ELSE '' END AS Summary, EventDate, Location, ImageUrl FROM Events ORDER BY EventDate ASC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    EventsRepeater.DataSource = dt;
                    EventsRepeater.DataBind();
                }
            }
        }

        protected void EventsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (IsUserAdmin())
                {
                    HyperLink editLink = (HyperLink)e.Item.FindControl("EditLink");
                    LinkButton deleteBtn = (LinkButton)e.Item.FindControl("DeleteBtn");
                    if (editLink != null) editLink.Visible = true;
                    if (deleteBtn != null) deleteBtn.Visible = true;
                }
            }
        }

        protected void EventsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete" && IsUserAdmin())
            {
                int eventId = Convert.ToInt32(e.CommandArgument);
                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "DELETE FROM Events WHERE EventId = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", eventId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadEvents();
            }
        }

        protected void CreateEventButton_Click(object sender, EventArgs e)
        {
            if (!IsUserAdmin()) return;

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            string imageUrl = null;

            if (EventImageUpload.HasFile)
            {
                string ext = System.IO.Path.GetExtension(EventImageUpload.FileName).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string dirPath = Server.MapPath("~/Uploads/Events/");
                    if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);
                    string fileName = Guid.NewGuid().ToString() + ext;
                    EventImageUpload.SaveAs(System.IO.Path.Combine(dirPath, fileName));
                    imageUrl = "Uploads/Events/" + fileName;
                }
            }

            DateTime eventDateTime = Convert.ToDateTime(EventDateTextBox.Text + " " + EventTimeTextBox.Text);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                bool isUpdate = Request.QueryString["edit_id"] != null;
                string query = isUpdate 
                    ? "UPDATE Events SET Title=@Title, Description=@Desc, EventDate=@Date, Location=@Location" + (imageUrl != null ? ", ImageUrl=@Img" : "") + " WHERE EventId=@Id"
                    : "INSERT INTO Events (Title, Description, EventDate, Location, ImageUrl) VALUES (@Title, @Desc, @Date, @Location, @Img)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", EventTitleTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Desc", EventDescTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Date", eventDateTime);
                    cmd.Parameters.AddWithValue("@Location", EventLocationTextBox.Text.Trim());
                    
                    if (isUpdate)
                    {
                        cmd.Parameters.AddWithValue("@Id", Request.QueryString["edit_id"]);
                        if (imageUrl != null) cmd.Parameters.AddWithValue("@Img", imageUrl);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Img", imageUrl ?? "assets/images/event-placeholder.png");
                    }
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            if (Request.QueryString["edit_id"] != null) Response.Redirect("Events.aspx");

            EventMessage.Text = "Event saved successfully!";
            EventMessage.Visible = true;
            
            EventTitleTextBox.Text = "";
            EventDescTextBox.Text = "";
            EventLocationTextBox.Text = "";

            LoadEvents();
        }
    }
}
