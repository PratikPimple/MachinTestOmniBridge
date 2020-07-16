using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
namespace Test
{
    public partial class Form2 : Form
    {
        SqlDataAdapter adapt;
        DataTable dt = new DataTable();
        private bool sortAscending = false;
        private TestEntities db = new TestEntities();
        tblProduct obj = new tblProduct();
        public Form2()
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {    
            DisplayData("",2);
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            dataGridView1.Sort(dataGridView1.Columns[5], ListSortDirection.Ascending);
        }
        
        private void DisplayData(string name,int action)
        {
            string consString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            SqlConnection con = new SqlConnection(consString);
            con.Open();
            dt = new DataTable();
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand("IFExist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", action);

            cmd.Parameters.AddWithValue("@Name", name);
            adapt = new SqlDataAdapter(cmd);
            adapt.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;

            }else
            {
                MessageBox.Show("Data Not Found");
            }
            con.Close();
        }
        private DataTable Getdata(string name, int action)
        {
            string consString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            SqlConnection con = new SqlConnection(consString);
            con.Open();
            dt = new DataTable();
            
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand("IFExist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", action);

            cmd.Parameters.AddWithValue("@Name", name);
            adapt = new SqlDataAdapter(cmd);
            adapt.Fill(dt);
            
            con.Close();
            return dt;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            //if (sortAscending)
            //    dataGridView1.DataSource = db.tblProducts.ToList().OrderBy(dataGridView1.Columns[e.ColumnIndex].DataPropertyName,sortAscending).ToList();
            //else
            //    dataGridView1.DataSource = list.OrderBy(dataGridView1.Columns[e.ColumnIndex].DataPropertyName).Reverse().ToList();
            //sortAscending = !sortAscending;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(txtName.Text.Trim()!="")
            {
                DisplayData(txtName.Text.Trim(), 3);
            }else { MessageBox.Show("Enter a Name"); }
            
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // creating Excel Application  
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
            // save the application  
            workbook.SaveAs("F:\\output.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application  
            app.Quit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // creating Excel Application  
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  

            dt = new DataTable();
            dt = Getdata("", 2);
            for (int i = 1; i < dt.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dt.Columns[i - 1].ToString();
            }
            // storing Each row and column value to excel sheet  

            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                }
            }
            // save the application  
            workbook.SaveAs("F:\\Alloutput.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application  
            app.Quit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayData("", 2);
        }
    }
}
