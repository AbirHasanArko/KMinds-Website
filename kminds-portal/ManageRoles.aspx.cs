using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace KMinds.Portal.Web
{
    public partial class ManageRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsersGrid();
            }
        }

        private void BindUsersGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserId, FullName, Email, Department, Role FROM Users ORDER BY FullName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        UsersGridView.DataSource = dt;
                        UsersGridView.DataBind();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddl = (DropDownList)UsersGridView.Rows[i].FindControl("RoleDropDown");
                            string role = dt.Rows[i]["Role"].ToString();
                            if (ddl.Items.FindByValue(role) != null)
                            {
                                ddl.SelectedValue = role;
                            }
                        }
                    }
                }
            }
        }

        protected void UsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateRole")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = UsersGridView.Rows[index];

                int userId = Convert.ToInt32(UsersGridView.DataKeys[index].Value);
                DropDownList roleDropDown = (DropDownList)row.FindControl("RoleDropDown");
                string newRole = roleDropDown.SelectedValue;

                UpdateUserRole(userId, newRole);
                BindUsersGrid();

                StatusMessage.Text = "<div style='color:green; padding:10px; border:1px solid green; margin-bottom:15px;'>Role updated successfully!</div>";
                StatusMessage.Visible = true;
            }
        }

        private void UpdateUserRole(int userId, string newRole)
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Users SET Role = @Role WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Role", newRole);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
