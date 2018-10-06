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
            builder.Password = File.ReadAllText(Application.StartupPath + "/SQLPassword.txt");
            builder.InitialCatalog = "VirtualLib";
        }

        public int AddBook(String name, String author, int barcode, int pages)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO [dbo].[Book] ");
                    sb.Append("VALUES('" + name + "','" + author + "'," + barcode + "," + pages + ");");
                    
                    String sql = sb.ToString();
                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                    }

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
                    String sql = sb.ToString();
                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                    }

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

        public int SearchUser(string name)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select name from [dbo].[user] ");
                    sb.Append("WHERE name = '" + name + "';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                return 2;
                        }
                    }
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

        public String[] GetUser(string name)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select * from [dbo].[user] ");
                    sb.Append("WHERE name = '" + name + "';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            String[] userData = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3) };
                            return userData;
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
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
                    String sql = sb.ToString();
                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                    }

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

        public int DelUserBook(Book delThis)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("DELETE FROM [dbo].[UserBook] a ");
                    sb.Append("WHERE " + StaticData.CurrentUser.ID + " = a.UserID and " + delThis.ID + " = a.BookID;");
                    String sql = sb.ToString();
                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                    }

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
