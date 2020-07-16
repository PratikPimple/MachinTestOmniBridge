using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
namespace Test
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RBYes.Name = "GST";
            RBNo.Name = "GST";
            RBYes.Checked = true;
            ExpiryDate.Value = DateTime.Today.AddDays(-1);
            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            ExpiryDate.MinDate = new DateTime(year,month,day);
            
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == 46)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool flag = true;
            try
            {
                if (txtName.Text == "" || txtprice.Text == "" || txtQty.Text == "" ||ddlColor.SelectedIndex==-1)
                {
                    flag = false;
                }
                if (flag == true)
                {
                    string consString1 = System.Configuration.ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                    SqlConnection con1 = new SqlConnection(consString1);
                    if (con1.State == ConnectionState.Closed)
                        con1.Open();
                    SqlCommand cmd1 = new SqlCommand("IFExist", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@action", 1);

                    cmd1.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    int UserExist = (int)cmd1.ExecuteScalar();
                    if (UserExist == 0)
                    {

                        string RB = "";
                        if (RBYes.Checked == true)
                        {
                            RB = "Yes";
                        }
                        else if (RBNo.Checked == true)
                        {
                            RB = "No";

                        }
                        int i = 0;
                        string consString = System.Configuration.ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                        SqlConnection con = new SqlConnection(consString);
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlCommand cmd = new SqlCommand("InserProduct", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Price", txtprice.Text.Trim());
                        cmd.Parameters.AddWithValue("@Quantity", txtQty.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsGstApplicable", RB.Trim());
                        cmd.Parameters.AddWithValue("@PurchaseDate", PurchaseDate.Value);
                        cmd.Parameters.AddWithValue("@ExpiryDate", ExpiryDate.Value);
                        cmd.Parameters.AddWithValue("@color", ddlColor.Text);

                        i = cmd.ExecuteNonQuery();
                        con.Close();
                        if (i > 0)
                        {
                            MessageBox.Show("Record Saved");
                            Form2 obj = new Form2();
                            obj.Show();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Product name is allready inserted");
                    }
                }else
                {

                    MessageBox.Show("All field is mendetory");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = txtprice.Text = txtQty.Text = "";
            ddlColor.SelectedIndex = 0;

               
        }
    }
}
