﻿ using System;
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

        public int AddBook(String name, String author, String barcode, int pages)
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
            catch (SqlException e)
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
                    sb.Append("Select * from [dbo].[User] ");
                    sb.Append("WHERE name = '" + name + "';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                StaticData.CurrentUser = new User((int)reader.GetValue(0), (string)reader.GetValue(1), (string)reader.GetValue(2), null, (string)reader.GetValue(4), null, null);
                                connection.Close();
                                return null;//userData;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            return null;
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

        public int ReturnBook (Book delThis)
        {
            //Reik sukurt lentelę, kur dėsim perskaitytas knygas - knygos ID ir reader ID
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    sb.Append("DELETE FROM [dbo].[UserBook] a ");
                    sb.Append("WHERE " + StaticData.CurrentUser.ID + " = a.UserID and " + "a.BookID = " + delThis.ID +";");
                    sb2.Append("INSERT INTO [dbo].[BooksRead] ");
                    sb2.Append("VALUES(" + StaticData.CurrentUser.ID + ", " + delThis.ID + ");");
                        
                    String sql = sb.ToString();
                    String sql2 = sb2.ToString();

                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                    }
                    using (var sqlCommand = new SqlCommand(sql2, connection))
                    {
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                        var book = StaticData.Books.Find(y => y.ID == delThis.ID);
                        book.setQuantityPlius();
                        //reik ištrinti knygas iš listo  UserBooks tą knygą ir įdėt į BooksRead
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

        public void GetAllBooks(string name)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select * from [dbo].[Book];");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StaticData.Books.Add(new Book(reader.GetString(1), reader.GetString(2), reader.GetString(4), reader.GetString(5), (int)reader.GetValue(7), (int)reader.GetValue(6), reader.GetString(3), (int)reader.GetValue(0)));
                                return;
                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return;

            }
        }

        public void GetAllUserBooks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select BookID from [dbo].[UserBook] ");
                    sb.Append("WHERE UserID = " + StaticData.CurrentUser.ID + ";");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<int> bookIDList = new List<int>();
                            while (reader.Read())
                            {
                                StaticData.CurrentUser.getUserBooks().Add(StaticData.Books.Find(x => x.ID == (int)reader.GetValue(0)));
                                return;
                            }
                        }
                        connection.Close();
                    }

                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                return;
            }

        }
        public void BorrowBook (Book addThis)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();
                    sb.Append("INSERT INTO [dbo].[UserBook] ");
                    sb.Append("VALUES(" + StaticData.CurrentUser.ID + ", " + addThis.ID + ");");
                    sb2.Append("UPDATE [dbo].[Book] ");
                    sb2.Append("SET Quantity = Quantity-1 ");
                    sb2.Append("WHERE ID = " +  addThis.ID + ";");

                    String sql = sb.ToString();
                    String sql2 = sb2.ToString();
                    using (var sqlCommand = new SqlCommand(sql, connection))
                    {
                        
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                    }
                    using (var sqlCommand = new SqlCommand(sql2, connection))
                    {

                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " = rows affected.");
                        var book = StaticData.Books.Find(y => y.ID == addThis.ID);
                        book.setQuantityMinus();
                        //reikia knygą įdėti į UserBooks
     
                    }
                    connection.Close();
                }
            
            }
            catch(SqlException e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }
    }

    
}
