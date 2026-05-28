using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

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
                CheckEditMode();
            }
        }

        private void CheckEditMode()
        {
            if (Request.QueryString["edit_id"] != null && User.Identity.IsAuthenticated)
            {
                string editId = Request.QueryString["edit_id"];
                string email = User.Identity.Name;
                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT Title, Content, AuthorId FROM Articles WHERE ArticleId = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", editId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Verify author
                                int authorId = Convert.ToInt32(reader["AuthorId"]);
                                if (GetCurrentUserId(email) == authorId)
                                {
                                    ArticleTitleTextBox.Text = reader["Title"].ToString();
                                    string content = reader["Content"].ToString();
                                    // simple split for summary vs content if it was saved with \n\n
                                    if(content.Contains("\n\n"))
                                    {
                                        ArticleSummaryTextBox.Text = content.Substring(0, content.IndexOf("\n\n"));
                                        ArticleContentTextBox.Text = content.Substring(content.IndexOf("\n\n") + 2);
                                    } 
                                    else 
                                    {
                                        ArticleContentTextBox.Text = content;
                                    }
                                    
                                    PublishArticleButton.Text = "Save Changes";
                                    CreateArticlePlaceHolder.Visible = true;
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
                string query = "SELECT a.ArticleId, a.AuthorId, a.Title, a.Content AS FullContent, SUBSTRING(a.Content, 1, 150) + '...' AS Summary, a.ThumbnailUrl, a.PublishDate, u.FullName AS AuthorName FROM Articles a JOIN Users u ON a.AuthorId = u.UserId ORDER BY a.PublishDate DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ArticlesRepeater.DataSource = dt;
                    ArticlesRepeater.DataBind();
                }
            }
        }

        protected void ArticlesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        protected void ArticlesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete" && User.Identity.IsAuthenticated)
            {
                int articleId = Convert.ToInt32(e.CommandArgument);
                int authorId = GetCurrentUserId(User.Identity.Name);

                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "DELETE FROM Articles WHERE ArticleId = @Id AND AuthorId = @AuthorId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", articleId);
                        cmd.Parameters.AddWithValue("@AuthorId", authorId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadArticles();
            }
        }

        protected void PublishArticleButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) return;
            string email = User.Identity.Name;

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            
            int authorId = GetCurrentUserId(email);
            if (authorId == 0) return; 

            string thumbnailUrl = null;
            if (ThumbnailUpload.HasFile)
            {
                string ext = System.IO.Path.GetExtension(ThumbnailUpload.FileName).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string dirPath = Server.MapPath("~/Uploads/Articles/");
                    if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);
                    string fileName = Guid.NewGuid().ToString() + ext;
                    ThumbnailUpload.SaveAs(System.IO.Path.Combine(dirPath, fileName));
                    thumbnailUrl = "Uploads/Articles/" + fileName;
                }
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                bool isUpdate = Request.QueryString["edit_id"] != null;
                string query = isUpdate 
                    ? "UPDATE Articles SET Title=@Title, Content=@Content" + (thumbnailUrl != null ? ", ThumbnailUrl=@ThumbnailUrl" : "") + " WHERE ArticleId=@Id AND AuthorId=@AuthorId"
                    : "INSERT INTO Articles (Title, Content, AuthorId, PublishDate, ThumbnailUrl) VALUES (@Title, @Content, @AuthorId, GETDATE(), @ThumbnailUrl)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", ArticleTitleTextBox.Text.Trim());
                    string fullContent = ArticleSummaryTextBox.Text.Trim() + "\n\n" + ArticleContentTextBox.Text.Trim();
                    cmd.Parameters.AddWithValue("@Content", fullContent);
                    cmd.Parameters.AddWithValue("@AuthorId", authorId);
                    
                    if (isUpdate)
                    {
                        cmd.Parameters.AddWithValue("@Id", Request.QueryString["edit_id"]);
                        if (thumbnailUrl != null) cmd.Parameters.AddWithValue("@ThumbnailUrl", thumbnailUrl);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ThumbnailUrl", thumbnailUrl ?? "assets/images/article-preview.png");
                    }
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            if (Request.QueryString["edit_id"] != null) Response.Redirect("Articles.aspx");

            ArticleMessage.Text = "Article published successfully!";
            ArticleMessage.Visible = true;
            
            ArticleTitleTextBox.Text = "";
            ArticleSummaryTextBox.Text = "";
            ArticleContentTextBox.Text = "";

            LoadArticles();
        }
    }
}
