using System;
using System.Data.SqlClient;
using System.Configuration;

namespace KMinds.Portal.Web
{
    public partial class Profile : System.Web.UI.Page
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
                string query = "SELECT FullName, Email, Role FROM Users WHERE Email = @Email";
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
                                string[] names = fullName.Split(' ');
                                InitialsLiteral.Text = names.Length > 1 
                                    ? names[0].Substring(0, 1) + names[names.Length - 1].Substring(0, 1) 
                                    : fullName.Substring(0, 2).ToUpper();

                                // Stubbed data for demo
                                DeptLiteral.Text = "CSE";
                                YearTermLiteral.Text = "3-1";
                                RollLiteral.Text = "2105001";
                                
                                StatusBadge.Text = "Pending";
                                PaymentStatusText.Text = "Pending Verification";
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
            PaymentMessage.Text = "Payment reference submitted for review.";
            PaymentMessage.Visible = true;
        }
    }
}
