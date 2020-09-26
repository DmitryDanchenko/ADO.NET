using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace DBProFacrt
{
    public partial class Form1 : Form
    {
        DbConnection conn = null;
        DbProviderFactory fact = null;
        DataTable table = null;

        public Form1()
        {
            InitializeComponent();
            
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            //var adapter = fact.CreateDataAdapter();
            //adapter.SelectCommand = conn.CreateCommand();
            //adapter.SelectCommand.CommandText = textBox2.Text.ToString();
            //var set = new DataSet(); 
            //adapter.Fill(set);

            //DataViewManager dvm = new DataViewManager(set);
            //dvm.DataViewSettings[]

            try
            {
                DataTable t = DbProviderFactories.GetFactoryClasses();
                dataGridView1.DataSource = t;
                comboBox1.Items.Clear();
                foreach (DataRow dr in t.Rows)
                {
                    comboBox1.Items.Add(dr["InvariantName"]);
                }
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = GetConnectionStringByProvider(comboBox1.SelectedItem.ToString());
  
            fact = DbProviderFactories.GetFactory(comboBox1.SelectedItem.ToString());
            conn = fact.CreateConnection();
            conn.ConnectionString = textBox1.Text;
        }

        private string GetConnectionStringByProvider(string providerName)
        {
            var settings = ConfigurationManager.ConnectionStrings;
            if(settings != null)
            {
                foreach  (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                        return cs.ConnectionString;
                }
            }
            return String.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = textBox1.Text;
            DbDataAdapter adapter = fact.CreateDataAdapter();
            adapter.SelectCommand = conn.CreateCommand();
            adapter.SelectCommand.CommandText = textBox2.
            Text.ToString();
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = table;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 5)
                button2.Enabled = true;
            else
                button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            { 
            const string asyncEnable = "Asynchronus Processing=true";
            if (!textBox1.Text.Contains(asyncEnable))
            {
                textBox1.Text = string.Format("{0};{1}", textBox1.Text, asyncEnable);
            }
            conn.ConnectionString = textBox1.Text;
            conn.Open();

            using (var comm = (conn as SqlConnection).CreateCommand())
            { 
                comm.CommandText = $"WAITFOR DELAY '00:00:05'; {textBox2.Text};";
                comm.CommandType = CommandType.Text;
                            
                comm.CommandTimeout = 30;
                    
            }
            }
            catch(Exception exp)
            {

            }
        }

        private void CallBack(IAsyncResult result)
        {
            try
            {
                SqlCommand command = (SqlCommand)result.AsyncState;
                var dataReader = command.EndExecuteReader(result);
                table = new DataTable();
                do
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        table.Columns.Add(dataReader.GetName(i));
                    }
                    while (dataReader.Read())
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            row[i] = dataReader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (dataReader.NextResult());
                if (conn != null)
                    conn.Close();
                //ShowData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void ShowData()
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action)
                {

                }
            }
        }
    }
}
