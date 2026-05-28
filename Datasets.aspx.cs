using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace KMinds.Portal.Web
{
    public partial class Datasets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDatasets();
                if (User.Identity.IsAuthenticated)
                {
                    UploadDatasetPlaceHolder.Visible = true;
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
                    string query = "SELECT Title, Description, UploaderId FROM Datasets WHERE DatasetId = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", editId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int uploaderId = Convert.ToInt32(reader["UploaderId"]);
                                if (GetCurrentUserId(email) == uploaderId)
                                {
                                    DatasetTitleTextBox.Text = reader["Title"].ToString();
                                    DatasetDescTextBox.Text = reader["Description"].ToString();
                                    DatasetFileUpload.Attributes.Remove("required");
                                    
                                    UploadDatasetButton.Text = "Save Changes";
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

        private void LoadDatasets()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT DatasetId, UploaderId, Title, Description AS FullContent, SUBSTRING(Description, 1, 150) + CASE WHEN LEN(Description) > 150 THEN '...' ELSE '' END AS Summary, DownloadLink, Size, Domain, ISNULL(ThumbnailUrl, 'assets/images/research-preview.png') AS ThumbnailUrl FROM Datasets ORDER BY UploadDate DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DatasetsRepeater.DataSource = dt;
                    DatasetsRepeater.DataBind();
                }
            }
        }

        protected void DatasetsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                int uploaderId = Convert.ToInt32(drv["UploaderId"]);
                if (User.Identity.IsAuthenticated && GetCurrentUserId(User.Identity.Name) == uploaderId)
                {
                    HyperLink editLink = (HyperLink)e.Item.FindControl("EditLink");
                    LinkButton deleteBtn = (LinkButton)e.Item.FindControl("DeleteBtn");
                    if (editLink != null) editLink.Visible = true;
                    if (deleteBtn != null) deleteBtn.Visible = true;
                }
            }
        }

        protected void DatasetsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete" && User.Identity.IsAuthenticated)
            {
                int datasetId = Convert.ToInt32(e.CommandArgument);
                int uploaderId = GetCurrentUserId(User.Identity.Name);

                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "DELETE FROM Datasets WHERE DatasetId = @Id AND UploaderId = @UploaderId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", datasetId);
                        cmd.Parameters.AddWithValue("@UploaderId", uploaderId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadDatasets();
            }
        }

        protected void UploadDatasetButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) return;
            int uploaderId = GetCurrentUserId(User.Identity.Name);
            if (uploaderId == 0) return;

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            
            string downloadLink = null;
            string size = null;
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

            if (DatasetFileUpload.HasFile)
            {
                string dirPath = Server.MapPath("~/Uploads/Datasets/");
                if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);
                string fileName = Guid.NewGuid().ToString() + "_" + DatasetFileUpload.FileName;
                DatasetFileUpload.SaveAs(System.IO.Path.Combine(dirPath, fileName));
                downloadLink = "Uploads/Datasets/" + fileName;
                size = (DatasetFileUpload.PostedFile.ContentLength / 1024 / 1024) + " MB";
                if(size == "0 MB") size = (DatasetFileUpload.PostedFile.ContentLength / 1024) + " KB";
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                bool isUpdate = Request.QueryString["edit_id"] != null;
                string query = isUpdate 
                    ? "UPDATE Datasets SET Title=@Title, Description=@Desc" + (downloadLink != null ? ", DownloadLink=@Link, Size=@Size" : "") + (thumbnailUrl != null ? ", ThumbnailUrl=@Thumb" : "") + " WHERE DatasetId=@Id AND UploaderId=@UploaderId"
                    : "INSERT INTO Datasets (Title, Description, UploaderId, UploadDate, DownloadLink, Size, Domain, ThumbnailUrl) VALUES (@Title, @Desc, @UploaderId, GETDATE(), @Link, @Size, 'General', @Thumb)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", DatasetTitleTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Desc", DatasetDescTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@UploaderId", uploaderId);
                    
                    if (isUpdate)
                    {
                        cmd.Parameters.AddWithValue("@Id", Request.QueryString["edit_id"]);
                        if (thumbnailUrl != null) cmd.Parameters.AddWithValue("@Thumb", thumbnailUrl);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Thumb", thumbnailUrl ?? (object)DBNull.Value);
                    }

                    if (downloadLink != null || !isUpdate)
                    {
                        cmd.Parameters.AddWithValue("@Link", downloadLink ?? "#");
                        cmd.Parameters.AddWithValue("@Size", size ?? "Unknown");
                    }
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            if (Request.QueryString["edit_id"] != null) Response.Redirect("Datasets.aspx");

            DatasetMessage.Text = "Dataset saved successfully!";
            DatasetMessage.Visible = true;
            
            DatasetTitleTextBox.Text = "";
            DatasetDescTextBox.Text = "";

            LoadDatasets();
        }
    }
}
