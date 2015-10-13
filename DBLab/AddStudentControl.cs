using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DBLabs
{
    public partial class AddStudentControl : UserControl
    {
        private DBConnection dbconn;

        public AddStudentControl()
        {
            /*
             * Constructor the control
             * 
             * You DONT need to edit this constructor.
             * 
             */
            InitializeComponent();
        }

        public void AddStudentControlSettings(ref DBConnection dbconn)
        {
            /*
             * Since UserControls cannot take arguments to the constructor 
             * this function is called after the constructor to perform this.
             * 
             * You DONT need to edit this function.
             * 
             */
            this.dbconn = dbconn; 
        }

        private void LoadAddStudentControl(object sender, EventArgs e)
        {
            /*
             * This function contains all code that needs to be executed when the control is first loaded
             * 
             * You need to edit this code. 
             * Example: Population of Comboboxes and gridviews etc.
             * 
             */
            dbconn.changeProcedure("addStudent");
            dataGridView1.Columns.Add("Type", "Type");
            dataGridView1.Columns.Add("Number", "Number");
            dataGridView1.Columns.Add("T_ID", "T_ID");

            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].Width = 50;

            using (dbconn.SQLConnection = new SqlConnection(dbconn.Connectionstring))
            {
                try
                {
                    string query = "select * from StudentTypes";
                    SqlDataAdapter da = new SqlDataAdapter(query, dbconn.SQLConnection);
                    dbconn.SQLConnection.Open();
                    DataSet ds = new DataSet();
                    da.Fill(ds, "StudentType");
                    comboBox1.DisplayMember = "StudentType";
                    comboBox1.ValueMember = "ST_Id";
                    comboBox1.DataSource = ds.Tables["StudentType"];

                    query = "select * from TelType";
                    da = new SqlDataAdapter(query, dbconn.SQLConnection);
                    ds = new DataSet();
                    da.Fill(ds, "TelType");
                    comboBox2.DisplayMember = "TypeName";
                    comboBox2.ValueMember = "T_ID";
                    comboBox2.DataSource = ds.Tables["TelType"];

                }
                catch (Exception er)
                {
                    
                    MessageBox.Show(er.Message);
                }
                
            }

        }
        public void ResetAddStudentControl()
        {
            studentIDTextBox.Text = "";
            firstnameTextBox.Text = "";
            lastnameTextBox.Text = "";
            femaleRadioButton.Checked = false;
            maleRadioButton.Checked = false;
            zipCodeTextBox.Text = "";
            cityTextBox.Text = "";
            countryTextBox.Text = "";
            birthDatePicker.Value = DateTime.Now;
            numberTextBox1.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void addStudentButton_Click(object sender, EventArgs e)
        {
            if (dbconn.checkStudentsTablePK(studentIDTextBox.Text))
            {
                MessageBox.Show("There's already a student with that studentID registered! Please enter a unique and valid studentID.");
                return;
            }

            

            if (string.IsNullOrEmpty(studentIDTextBox.Text) || string.IsNullOrEmpty(firstnameTextBox.Text) || string.IsNullOrEmpty(lastnameTextBox.Text)
                || (maleRadioButton.Checked == false && femaleRadioButton.Checked == false))
            {
                MessageBox.Show("You need to enter information in the fields that are marked with an (*)");
                return;
            }

            dbconn.changeProcedure("addStudent");

            dbconn.SQLCmd.Parameters.Add("@studentID", SqlDbType.Char).Value = studentIDTextBox.Text;
            dbconn.SQLCmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = firstnameTextBox.Text;
            dbconn.SQLCmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = lastnameTextBox.Text;
            if(maleRadioButton.Checked==true)
                dbconn.SQLCmd.Parameters.Add("@gender", SqlDbType.Bit).Value = 1;
            else if(femaleRadioButton.Checked == true)
                dbconn.SQLCmd.Parameters.Add("@gender", SqlDbType.Bit).Value = 0;

            dbconn.SQLCmd.Parameters.Add("@streetAdress", SqlDbType.VarChar).Value = streetAdressTextBox.Text;
            if(string.IsNullOrEmpty(zipCodeTextBox.Text))
            {
                dbconn.SQLCmd.Parameters.Add("@zipCode", SqlDbType.Int).Value = 0;
            }
            else
            {
                dbconn.SQLCmd.Parameters.Add("@zipCode", SqlDbType.Int).Value = int.Parse(zipCodeTextBox.Text);
            }
                
            dbconn.SQLCmd.Parameters.Add("@city", SqlDbType.VarChar).Value = cityTextBox.Text;
            dbconn.SQLCmd.Parameters.Add("@country", SqlDbType.VarChar).Value = countryTextBox.Text;
            dbconn.SQLCmd.Parameters.Add("@birthdate", SqlDbType.Char).Value = birthDatePicker.Value.ToString("dd/MM-yyyy");

            if(comboBox1.SelectedItem != null)
            {
                DataRowView drv = comboBox1.SelectedItem as DataRowView;

                drv.Row["ST_Id"].ToString();
                dbconn.SQLCmd.Parameters.Add("@ST_Id", SqlDbType.Int).Value = drv.Row["ST_Id"].ToString();
            }
            

            
            dbconn.executeCommand();

            

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                dbconn.changeProcedure("addStudentPhoneNo");
                int index = comboBox2.FindString(item.Cells[0].Value.ToString());
                comboBox2.SelectedIndex = index;
                comboBox2.SelectedItem = comboBox2.Items[index];

                dbconn.SQLCmd.Parameters.Add("@phoneType", SqlDbType.Int).Value = (int)comboBox2.SelectedValue;
                dbconn.SQLCmd.Parameters.Add("@studentID", SqlDbType.Char).Value = studentIDTextBox.Text;
                dbconn.SQLCmd.Parameters.Add("@number", SqlDbType.BigInt).Value = Int32.Parse(item.Cells[1].Value.ToString());
                dbconn.executeCommand();
            }

            ResetAddStudentControl();

        }

        private void AddStudentGB_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DataRowView drv = comboBox1.SelectedItem as DataRowView;

                Debug.WriteLine("Item: " + drv.Row["StudentType"].ToString());
                Debug.WriteLine("Value: " + drv.Row["ST_Id"].ToString());
                Debug.WriteLine("Value: " + comboBox1.SelectedValue.ToString());
            }
        }


        /*ÄNDRA SÅ ATT DET ENBART FÅR FINNAS ETT UNIKT NUMMMER PER STUDENTID ISTÄLLET FÖR EN UNIK TYP*/
        private void addNumberButton_Click(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[1].Value.ToString().Equals(numberTextBox1.Text))
                {
                    MessageBox.Show("Can't have more than one type of number!");
                    return;
                }
            }
            dataGridView1.Rows.Add(comboBox2.Text.ToString(), numberTextBox1.Text.ToString());
        }

        private void numberTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)))

            {

                if (e.KeyChar != '\b') //allow the backspace key

                {

                    e.Handled = true;

                }

            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow item in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
            
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 1) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        
    }


}
