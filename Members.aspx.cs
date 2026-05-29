using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace KMinds.Portal.Web
{
    public partial class Members : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMembers();
            }
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            LoadMembers();
        }

        private void LoadMembers()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserId, FullName, Email, Role, ISNULL(Department, 'N/A') AS Department, ISNULL(PaymentRef, 'None') AS PaymentRef, ISNULL(PaymentStatus, 'Pending') AS PaymentStatus FROM Users";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    MembersRepeater.DataSource = dt;
                    MembersRepeater.DataBind();
                }
            }
        }

        protected void MembersRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            string status = "";
            if (e.CommandName == "Approve")
            {
                status = "Approved";
            }
            else if (e.CommandName == "Reject")
            {
                status = "Rejected";
            }
            
            if (!string.IsNullOrEmpty(status))
            {
                string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Users SET PaymentStatus = @Status WHERE UserId = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            LoadMembers(); // Refresh grid
        }
    }
}
