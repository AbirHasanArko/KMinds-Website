using System;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("Dashboard.aspx");
            }
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != ConfirmPasswordTextBox.Text)
            {
                ErrorMessage.Text = "Passwords do not match.";
                ErrorMessage.Visible = true;
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "INSERT INTO Users (FullName, Email, PasswordHash, Role, JoinDate) VALUES (@FullName, @Email, @PasswordHash, 'Member', GETDATE())";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", FullNameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text.Trim());
                    // In a real application, Hash the password using BCrypt or similar!
                    cmd.Parameters.AddWithValue("@PasswordHash", PasswordTextBox.Text); 

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        // Redirect to Login upon successful registration
                        Response.Redirect("Login.aspx");
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627) // Unique constraint violation (Email)
                        {
                            ErrorMessage.Text = "An account with this email already exists.";
                        }
                        else
                        {
                            ErrorMessage.Text = "A database error occurred. Please try again later.";
                        }
                        ErrorMessage.Visible = true;
                    }
                }
            }
        }
    }
}
