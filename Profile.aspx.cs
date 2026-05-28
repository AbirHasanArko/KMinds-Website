using System;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            if (!User.Identity.IsAuthenticated) return;

            string email = User.Identity.Name;
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT FullName, Email, Role, Department, RollNumber, YearTerm, ISNULL(PaymentStatus, 'Pending') AS PaymentStatus, PaymentRef, ProfileImageUrl FROM Users WHERE Email = @Email";
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
                                string fullName = reader["FullName"].ToString();
                                string role = reader["Role"].ToString();

                                FullNameLiteral.Text = fullName;
                                EmailLiteral.Text = reader["Email"].ToString();
                                RoleLiteral.Text = role;
                                RoleDetailLiteral.Text = role;

                                // Generate initials
                                string[] names = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                InitialsLiteral.Text = names.Length > 1 
                                    ? names[0].Substring(0, 1).ToUpper() + names[names.Length - 1].Substring(0, 1).ToUpper() 
                                    : (fullName.Length >= 2 ? fullName.Substring(0, 2).ToUpper() : fullName.ToUpper());

                                string profileImageUrl = reader["ProfileImageUrl"].ToString();
                                if (!string.IsNullOrEmpty(profileImageUrl))
                                {
                                    ProfileImage.ImageUrl = profileImageUrl;
                                    ProfileImage.Visible = true;
                                    avatar_initials.Visible = false;
                                }
                                else
                                {
                                    ProfileImage.Visible = false;
                                    avatar_initials.Visible = true;
                                }

                                // Bind real database data
                                DeptLiteral.Text = reader["Department"].ToString();
                                YearTermLiteral.Text = reader["YearTerm"].ToString();
                                RollLiteral.Text = reader["RollNumber"].ToString();
                                
                                string status = reader["PaymentStatus"].ToString();
                                StatusBadge.Text = status;
                                StatusBadge.CssClass = "status status-" + status.ToLower();
                                PaymentStatusText.Text = status;
                                PaymentStatusText.CssClass = "status status-" + status.ToLower();
                                
                                if (reader["PaymentRef"] != DBNull.Value)
                                {
                                    BkashRefTextBox.Text = reader["PaymentRef"].ToString();
                                }
                                
                                if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                                {
                                    PaymentUpdateFormPlaceholder.Visible = false;
                                }
                                else
                                {
                                    PaymentUpdateFormPlaceholder.Visible = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle
                    }
                }
            }
        }

        protected void UpdatePaymentRefButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) return;
            string email = User.Identity.Name;
            string newRef = BkashRefTextBox.Text.Trim();
            
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Users SET PaymentRef = @Ref, PaymentStatus = 'Pending' WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ref", newRef);
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            
            PaymentMessage.Text = "Payment reference submitted for review.";
            PaymentMessage.Visible = true;
            LoadUserProfile();
        }

        protected void EditProfileButton_Click(object sender, EventArgs e)
        {
            EditFullName.Text = FullNameLiteral.Text;
            EditDept.Text = DeptLiteral.Text;
            EditYearTerm.Text = YearTermLiteral.Text;
            EditRoll.Text = RollLiteral.Text;
            ProfileMultiView.ActiveViewIndex = 1;
        }

        protected void CancelEditButton_Click(object sender, EventArgs e)
        {
            ProfileMultiView.ActiveViewIndex = 0;
        }

        protected void SaveProfileButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated) return;
            string email = User.Identity.Name;
            
            string profileImageUrl = null;
            if (EditProfileImageUpload.HasFile)
            {
                string ext = System.IO.Path.GetExtension(EditProfileImageUpload.FileName).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string dirPath = Server.MapPath("~/Uploads/Profiles/");
                    if (!System.IO.Directory.Exists(dirPath))
                    {
                        System.IO.Directory.CreateDirectory(dirPath);
                    }
                    string fileName = Guid.NewGuid().ToString() + ext;
                    EditProfileImageUpload.SaveAs(System.IO.Path.Combine(dirPath, fileName));
                    profileImageUrl = "Uploads/Profiles/" + fileName;
                }
            }

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Users SET FullName = @Name, Department = @Dept, YearTerm = @YT, RollNumber = @Roll" +
                               (profileImageUrl != null ? ", ProfileImageUrl = @Pic" : "") + 
                               " WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", EditFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Dept", EditDept.Text.Trim());
                    cmd.Parameters.AddWithValue("@YT", EditYearTerm.Text.Trim());
                    cmd.Parameters.AddWithValue("@Roll", EditRoll.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", email);
                    if (profileImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@Pic", profileImageUrl);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            ProfileMultiView.ActiveViewIndex = 0;
            LoadUserProfile();
        }
    }
}
