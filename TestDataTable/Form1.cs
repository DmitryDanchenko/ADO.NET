using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDataTable
{
    public partial class Form1 : Form
    {
        SqlDataAdapter da;
        SqlCommandBuilder cmd;
        DataSet Set;
        DataTable table;
        string Filename;

        static readonly SqlConnection conn = new SqlConnection();
        //{
        //    ConnectionString = ConfigurationManager.ConnectionStrings["LibraryConnection"].ConnectionString
        //};


        //string cs = ConfigurationManager.ConnectionStrings["LibraryConnection"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
             SqlConnection conn = new SqlConnection(cs);
                SqlCommand comm = new SqlCommand
                {
                    CommandText = "select* from Books",
                    Connection = conn
                };


                Set = new DataSet();
                string sql = textBox1.Text;

                da = new SqlDataAdapter(sql, conn);
                //cmd = new SqlCommandBuilder(da);

                cmd = new SqlCommandBuilder(da);

                //SqlCommand UpdateCmd = new SqlCommand("Update Authos set FirstName  = @pPrice where id = @pId", conn);
                //UpdateCmd.Parameters.Add(new SqlParameter("@pPrice", SqlDbType.Int));
                //UpdateCmd.Parameters["@pPrice"].SourceVersion = DataRowVersion.Current;
                //UpdateCmd.Parameters["@pPrice"].SourceColumn = "Price";
                //UpdateCmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
                //UpdateCmd.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
                //UpdateCmd.Parameters["@pId"].SourceColumn = "id";
                ////вставляем созданный объект SqlCommand в свойство
                ////UpdateCommand SqlDataAdapter
                //da.UpdateCommand = UpdateCmd;

                //da.Fill(Set, "mybook");
                //dataGridView1.DataSource = Set.Tables["mybook"];

                //Debug.WriteLine(cmd.GetInsertCommand().CommandText);

                SqlCommand sqlCommand = new SqlCommand("UpdateBooks", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameterCollection cparams = sqlCommand.Parameters;
                cparams.Add("@id", SqlDbType.Int, 0, "id");
                cparams["@id"].SourceVersion = DataRowVersion.Original;
                cparams.Add("@AuthorId", SqlDbType.Int,0, "@AuthorId");
                cparams.Add("@Title", SqlDbType.Int, 0, "@Title");
                cparams.Add("@Prise", SqlDbType.Int, 0, "@Prise");
                cparams.Add("@Pages", SqlDbType.Int, 0, "@Pages");

                 da.Fill(Set, "mybook");
               dataGridView1.DataSource = Set.Tables["mybook"];
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            da.Update(Set, "mybook");
            Debug.WriteLine(cmd.GetUpdateCommand().CommandText);
        }

        private void loadPichureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {

            };
            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    Filename = openFileDialog.FileName;
                    var bytes = CreateCopyImage(Filename);
                    conn.Open();
                    if((toolStripComboBox1.Text?.Length?? 0) !=0&&
                        int.TryParse(toolStripComboBox1.Text, out int index))
                    {
                        var comm = new SqlCommand("Insert into Pictures(bookid,name, picture) values (@bookid, 2name, @picture)", conn);
                        comm.Parameters.AddWithValue("@bookid", index);
                        comm.Parameters.AddWithValue("@name", Path.GetFileName(Filename));
                        comm.Parameters.AddWithValue("@picture", bytes);
                        comm.ExecuteNonQuery();
                        MessageBox.Show("Image was saved to DB");
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        private object CreateCopyImage(string filename)
        {
            var img = Image.FromFile(Filename);
            int maxWidht = 300;
            int maxHight = 300;
            int newWidht = (int)(img.Width * (double)maxWidht / img.Width);
            int newHeight = (int)(img.Height * (double)maxHight / img.Height);

            var imageration = new Bitmap(newWidht, newHeight);
            var g = Graphics.FromImage(imageration);
            g.DrawImage(img, 0, 0, newWidht, newHeight);

            using (var stream = new MemoryStream())
            using (var reader = new BinaryReader(stream))
            {
                imageration.Save(stream, ImageFormat.Jpeg);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return reader.ReadByte(int)stream.Length);
            }
        }

        //public void bbb()
        //{
        //        try
        //        {
        //            SqlCommand comm = new SqlCommand
        //            {
        //                CommandText = "select* from Books",
        //                Connection = conn
        //            };

        //            conn.Open();
        //            table = new DataTable();
        //            using (var reader = comm.ExecuteReader())
        //            {
        //                do
        //                {
        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                        table.Columns.Add(reader.GetName(i));

        //                    while (reader.Read())
        //                    {
        //                        DataRow row = table.NewRow();
        //                        for (int i = 0; i < reader.FieldCount; i++)
        //                            row[i] = reader[i];
        //                        table.Rows.Add(row);

        //                    }
        //                } while (reader.NextResult());
        //                dataGridView1.DataSource = table;

        //                conn.Close();
        //            }
        //        }
        //        catch (Exception exp)
        //        {
        //            MessageBox.Show(exp.ToString());
        //        }


        //    }

        //}
    }
}
