using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace KMinds.Portal.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("Dashboard.aspx");
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text; // In production, this should be compared against a hash!

            if (AuthenticateUser(email, password))
            {
                FormsAuthentication.RedirectFromLoginPage(email, false);
            }
            else
            {
                ErrorMessage.Text = "Invalid email or password.";
                ErrorMessage.Visible = true;
            }
        }

        private bool AuthenticateUser(string email, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Query compares PasswordHash (For simplicity here it compares raw string, but ALWAYS hash passwords in production)
                string query = "SELECT UserId, PasswordHash, Role FROM Users WHERE Email = @Email";
                
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
                                string dbPasswordHash = reader["PasswordHash"].ToString();
                                
                                // TODO: Replace this with proper hashing verification like BCrypt.Verify(password, dbPasswordHash)
                                if (password == dbPasswordHash) 
                                {
                                    // Authentication successful
                                    return true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.Text = "A database error occurred. Please try again later.";
                        ErrorMessage.Visible = true;
                        // Log exception (ex)
                    }
                }
            }
            return false;
        }
    }
}
