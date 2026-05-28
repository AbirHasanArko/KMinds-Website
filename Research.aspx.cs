using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

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
                    CheckEditMode();
                }
            }
        }

        private void CheckEditMode()
        {
            if (Request.QueryString["edit_id"] != null)
            {
                string editId = Request.QueryString["edit_id"];
                string email = User.Identity.Name;
                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT Title, Abstract, DownloadLink, AuthorId FROM Research WHERE ResearchId = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", editId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int authorId = Convert.ToInt32(reader["AuthorId"]);
                                if (GetCurrentUserId(email) == authorId)
                                {
                                    ResearchTitleTextBox.Text = reader["Title"].ToString();
                                    ResearchAbstractTextBox.Text = reader["Abstract"].ToString();
                                    ResearchLinkTextBox.Text = reader["DownloadLink"].ToString();
                                    
                                    ShareResearchButton.Text = "Save Changes";
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private int GetCurrentUserId(string email)
        {
            int authorId = 0;
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserId FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null) authorId = Convert.ToInt32(result);
                }
            }
            return authorId;
        }

        private void LoadResearch()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT r.ResearchId, r.AuthorId, r.Title, r.Abstract AS FullContent, SUBSTRING(r.Abstract, 1, 150) + CASE WHEN LEN(r.Abstract) > 150 THEN '...' ELSE '' END AS Summary, r.DownloadLink, r.PublishDate, ISNULL(r.ThumbnailUrl, 'assets/images/research-preview.png') AS ThumbnailUrl, u.FullName AS AuthorName FROM Research r JOIN Users u ON r.AuthorId = u.UserId ORDER BY r.PublishDate DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ResearchRepeater.DataSource = dt;
                    ResearchRepeater.DataBind();
                }
            }
        }

        protected void ResearchRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                int authorId = Convert.ToInt32(drv["AuthorId"]);
                if (User.Identity.IsAuthenticated && GetCurrentUserId(User.Identity.Name) == authorId)
                {
                    HyperLink editLink = (HyperLink)e.Item.FindControl("EditLink");
                    LinkButton deleteBtn = (LinkButton)e.Item.FindControl("DeleteBtn");
                    if (editLink != null) editLink.Visible = true;
                    if (deleteBtn != null) deleteBtn.Visible = true;
                }
            }
        }

        protected void ResearchRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete" && User.Identity.IsAuthenticated)
            {
                int researchId = Convert.ToInt32(e.CommandArgument);
                int authorId = GetCurrentUserId(User.Identity.Name);

                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "DELETE FROM Research WHERE ResearchId = @Id AND AuthorId = @AuthorId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", researchId);
                        cmd.Parameters.AddWithValue("@AuthorId", authorId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadResearch();
            }
        }

        protected void ShareResearchButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) return;
            int authorId = GetCurrentUserId(User.Identity.Name);
            if (authorId == 0) return;

            string thumbnailUrl = null;
            if (ThumbnailUpload.HasFile)
            {
                string ext = System.IO.Path.GetExtension(ThumbnailUpload.FileName).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string dirPath = Server.MapPath("~/Uploads/Images/");
                    if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);
                    string fileName = Guid.NewGuid().ToString() + ext;
                    ThumbnailUpload.SaveAs(System.IO.Path.Combine(dirPath, fileName));
                    thumbnailUrl = "Uploads/Images/" + fileName;
                }
            }

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                bool isUpdate = Request.QueryString["edit_id"] != null;
                string query = isUpdate 
                    ? "UPDATE Research SET Title=@Title, Abstract=@Abstract, DownloadLink=@Link" + (thumbnailUrl != null ? ", ThumbnailUrl=@Thumb" : "") + " WHERE ResearchId=@Id AND AuthorId=@AuthorId"
                    : "INSERT INTO Research (Title, Abstract, AuthorId, PublishDate, DownloadLink, ThumbnailUrl) VALUES (@Title, @Abstract, @AuthorId, GETDATE(), @Link, @Thumb)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", ResearchTitleTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Abstract", ResearchAbstractTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Link", ResearchLinkTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@AuthorId", authorId);
                    
                    if (isUpdate)
                    {
                        cmd.Parameters.AddWithValue("@Id", Request.QueryString["edit_id"]);
                        if (thumbnailUrl != null) cmd.Parameters.AddWithValue("@Thumb", thumbnailUrl);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Thumb", thumbnailUrl ?? (object)DBNull.Value);
                    }
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            if (Request.QueryString["edit_id"] != null) Response.Redirect("Research.aspx");

            ResearchMessage.Text = "Research item submitted successfully!";
            ResearchMessage.Visible = true;
            
            ResearchTitleTextBox.Text = "";
            ResearchAbstractTextBox.Text = "";
            ResearchLinkTextBox.Text = "";

            LoadResearch();
        }
    }
}
