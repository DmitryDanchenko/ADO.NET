using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson1
{
    class Program
    {
        static readonly SqlConnection conn = new SqlConnection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["LibraryConnection"].ConnectionString
        };
        static readonly SqlConnection conn1 = new SqlConnection()
        {
            ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Labrary; Integrated Security=SSPI;"
        };

        public string CommandText { get; private set; }

        public Program()
        {
            
        }
        public void InsertQ()
        {
            try
            {
                conn.Open();
                string insertString = @"insert into Authos (FirstName, LastName) values ('Ronald','Tolken')";
                SqlCommand command = new SqlCommand
                {
                    Connection = conn,
                    CommandText = insertString
                };

                command.ExecuteNonQuery();

                insertString = @"insert into Books (AuthosId, Title, PRICE, PAGES) values ('5','Lord of the Rings', '1100','1500')";
                SqlCommand command2 = new SqlCommand
                {
                    Connection = conn,
                    CommandText = insertString
                };

                command2.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
            finally
            {
                conn.Close();
            }
        }

           public void Delete()
            {
                conn.Open();
                string insertString = @"delete from Books where Title='Lord of the Rings' ";
                SqlCommand command = new SqlCommand
                {
                    Connection = conn,
                    CommandText = insertString
                };
                command.ExecuteNonQuery();
            }
        void SelectQ()
        {
            try
            {
                conn.Open();
                var command = new SqlCommand("select * from Authos; select Title from Books", conn); 
                var dataReader = command.ExecuteReader();

                do
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        Console.Write(dataReader.GetName(i) + " ");
                    }
                    Console.WriteLine();

                    while (dataReader.Read()) ;

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        Console.Write(dataReader.GetName(i) + " ");
                    }
                    Console.WriteLine();

                } while (dataReader.NextResult());

                //for (int i = 0; i < dataReader.FieldCount; i++)
                //{
                //   Console.Write (dataReader.GetName(i)+" ");
                //}
                //Console.WriteLine();


                //        while (dataReader.Read())
                //        {

                //            Console.WriteLine($"{dataReader[0]} \t{dataReader[1]} \t{dataReader[2]} \t");

                //            conn1.Open();
                //            var command1 = new SqlCommand()
                //            {
                //                CommandText = $"select Title from Books where AuthosId=@p1",
                //                Connection = conn1
                //            };
                //            command1.Parameters.Add("@p1", SqlDbType.Int).Value = dataReader["Id"];

                //            var readb = command1.ExecuteReader();
                //            while (readb.Read())
                //            {
                //                Console.WriteLine($"\t-{readb[0]}");

                //            }

                //            conn1.Close();

                //        }
                //        dataReader.Close();

                //    }
                //    catch(Exception exp)
                //    {
                //        Console.WriteLine(exp);
                   }
            finally
            {

                conn.Close();
            }
        }

        static void Main(string[] args)
        {
            
            Program program = new Program();
            program.SelectQ();
            //program.InsertQ();
            //program.Delete();
        }
    }
}
