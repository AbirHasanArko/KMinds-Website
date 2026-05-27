using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
                }
            }
        }

        private void LoadDatasets()
        {
            string connString = ConfigurationManager.ConnectionStrings["KMindsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Title, Description, DownloadLink, Size, Domain, '' AS ThumbnailUrl FROM Datasets ORDER BY UploadDate DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DatasetsRepeater.DataSource = dt;
                    DatasetsRepeater.DataBind();
                }
            }
        }

        protected void UploadDatasetButton_Click(object sender, EventArgs e)
        {
            DatasetMessage.Text = "Dataset uploaded successfully!";
            DatasetMessage.Visible = true;
            LoadDatasets();
        }
    }
}
