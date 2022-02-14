using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AtmApplication
{
    public partial class AdminForm : Form
    {
        SqlDataAdapter adapter;
        public AdminForm()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams //Creates shadow on forms
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= DsValue.CS_DropShadow;
                return cp;
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            getData();

            LoginForm loginFrm = new LoginForm();
            loginFrm.Show();
            this.Close();
        }

        //Shows data to the data grid view at the start of the admin panel form
        private void AdminForm_Load(object sender, EventArgs e)
        {
            showData();
        }

        //Update function to CRUD
        //Updates any data changes to the selected row
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Update and save account information?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "UPDATE tblAccounts SET FirstName = @FName, MiddleName = @MName, LastName = @LName, MobileNumber = @MNumber, DateOfBirth = @DOB, PIN = @PIN " +
                            "WHERE AccountNumber = @AccNum";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@AccNum", Convert.ToInt32(txtAccNum.Text));
                    command.Parameters.AddWithValue("@FName", txtFName.Text);
                    command.Parameters.AddWithValue("@MName", txtMName.Text);
                    command.Parameters.AddWithValue("@LName", txtLName.Text);
                    command.Parameters.AddWithValue("@MNumber", txtMobileNumber.Text);
                    command.Parameters.AddWithValue("@DOB", dtpDOB.Text);
                    command.Parameters.AddWithValue("@PIN", txtPIN.Text);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Information Saved and Updated!", "Notice");

                    showData();
                }
            }
        }

        //Update function in CRUD
        //Sets the selected row's status to 2
        private void btnDisable_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Disable this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "UPDATE tblAccounts SET Status = "+ 0 +"  WHERE AccountNumber = @AccNum";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@AccNum", Convert.ToInt32(txtAccNum.Text));
                    command.ExecuteNonQuery();

                    MessageBox.Show("Account: " + txtAccNum.Text + ", Disabled!", "Notice");

                    showData();
                }
            }
        }

        //Update function in CRUD
        //Sets the selected row's status to 1 
        private void btnEnable_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Enable this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "UPDATE tblAccounts SET Status = " + 1 + "  WHERE AccountNumber = @AccNum";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@AccNum", Convert.ToInt32(txtAccNum.Text));
                    command.ExecuteNonQuery();

                    MessageBox.Show("Account: " + txtAccNum.Text + ", Enabled!", "Notice");

                    showData();
                }
            }
        }

        //Delete function in CRUD
        //Permanently deletes a selected row from the database
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Permanently delete this account?\nThis cannot be undone", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "DELETE FROM tblAccounts  WHERE AccountNumber = @AccNum";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@AccNum", Convert.ToInt32(txtAccNum.Text));
                    command.ExecuteNonQuery();

                    MessageBox.Show("Account: " + txtAccNum.Text + ", Permanently Deleted!", "Notice");

                    showData();
                }
            }
        }

        private void dgvAccountList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            getData();
        }

        //Read function in CRUD
        //Retrieves data from database and displays on data grid view
        private void showData()
        {
            adapter = new SqlDataAdapter("SELECT * FROM tblAccounts ORDER BY LastName", DataAccess.conString);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvAccountList.DataSource = table;
        }

        private void getData()
        {
            txtAccNum.Text = dgvAccountList.SelectedRows[0].Cells[0].Value.ToString();
            txtFName.Text = dgvAccountList.SelectedRows[0].Cells[1].Value.ToString();
            txtMName.Text = dgvAccountList.SelectedRows[0].Cells[2].Value.ToString();
            txtLName.Text = dgvAccountList.SelectedRows[0].Cells[3].Value.ToString();
            txtMobileNumber.Text = dgvAccountList.SelectedRows[0].Cells[4].Value.ToString();
            dtpDOB.Text = dgvAccountList.SelectedRows[0].Cells[5].Value.ToString();
            txtPIN.Text = dgvAccountList.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void txtFName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFName.Text) || txtFName.Text.Any(Char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtFName, "First name cannot be empty or contain digits");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtFName, null);
            }
        }

        private void txtMName_Validating(object sender, CancelEventArgs e)
        {
            if (txtMName.Text.Any(Char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtMName, "Middle name contain digits");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMName, null);
            }
        }

        private void txtLName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLName.Text) || txtLName.Text.Any(Char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtLName, "Last name cannot be empty or contain digits");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtLName, null);
            }
        }

        private void txtMobileNumber_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMobileNumber.Text) || txtMobileNumber.Text.Any(Char.IsLetter))
            {
                e.Cancel = true;
                errorProvider.SetError(txtMobileNumber, "Mobile Number cannot be empty or contain letters");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMobileNumber, null);
            }
        }

        private void txtPIN_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPIN.Text) || txtPIN.Text.Any(Char.IsLetter))
            {
                e.Cancel = true;
                errorProvider.SetError(txtPIN, "PIN cannot be empty or contain letters");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtPIN, null);
            }
        }
    }
}
