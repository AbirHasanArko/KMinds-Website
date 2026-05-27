using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
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
            string password = PasswordTextBox.Text;

            string role = AuthenticateUser(email, password);
            if (!string.IsNullOrEmpty(role))
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                    false, // isPersistent
                    role // UserData holds the role
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                Response.Redirect(FormsAuthentication.GetRedirectUrl(email, false));
            }
            else
            {
                ErrorMessage.Text = "Invalid email or password.";
                ErrorMessage.Visible = true;
            }
        }

        private string AuthenticateUser(string email, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
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
                                string dbRole = reader["Role"].ToString();
                                
                                string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

                                // Fallback to plaintext comparison if the DB hasn't been migrated to hashes yet,
                                // but primarily use the hash comparison.
                                if (hashedPassword == dbPasswordHash || password == dbPasswordHash) 
                                {
                                    return dbRole;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.Text = "A database error occurred. Please try again later.";
                        ErrorMessage.Visible = true;
                    }
                }
            }
            return null;
        }
    }
}
