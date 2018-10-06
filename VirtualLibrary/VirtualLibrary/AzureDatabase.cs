using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    class AzureDatabase
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public AzureDatabase()
        {
            builder.DataSource = "virlib.database.windows.net";
            builder.UserID = "ILBooks";
            builder.Password = File.ReadAllText(Application.StartupPath + "SQLPassword.txt");
            builder.InitialCatalog = "VirtualLib";
        }

        public int AddBook(String name, String author, int barcode, int pages)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO [dbo].[Book] ");
                    sb.Append("VALUES('" + name + "','" + author + "'," + barcode + "," + pages + ");");
                    connection.Close();
                }
            }
            catch(SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

        public int AddUser(String name, String Password, String email, int Permission)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO [dbo].[User] ");
                    sb.Append("VALUES('" + name + "', '" + Password + "', '" + email + "', " + Permission + ");");
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

        public int AddUserBook(Book addThis)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO [dbo].[UserBook] ");
                    sb.Append("VALUES(" + StaticData.CurrentUser.ID + ", " + addThis.ID + ");");
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }

    }
}
