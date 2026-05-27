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
            // Placeholder logic. You'd normally build a dynamic query based on the selected dropdown values.
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserId, FullName, Email, Role, 'CSE' AS Department, 'TXN12345' AS PaymentRef, 'Pending' AS PaymentStatus FROM Users";
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
            if (e.CommandName == "Approve")
            {
                // Update DB to approve member
            }
            else if (e.CommandName == "Reject")
            {
                // Update DB to reject member
            }
            LoadMembers(); // Refresh grid
        }
    }
}
