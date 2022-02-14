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
    public partial class MenuViewAcountForm : Form
    {
        public MenuViewAcountForm()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= DsValue.CS_DropShadow;
                return cp;
            }
        }

        private void pbClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
            //Refreshes the name at the main menu if updated
            MainMenuForm mainFrm = new MainMenuForm();
            mainFrm.Show();
        }

        private void MenuViewAcountForm_Load(object sender, EventArgs e)
        {
            retriveAccountInfo();
        }

        //Enables textboxes for updating
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtFName.Enabled = true;
            txtMName.Enabled = true;
            txtLName.Enabled = true;
            txtMobileNum.Enabled = true;
            dtpDOB.Enabled = true;
            txtPIN.Enabled = true;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        //Update function for CRUD
        //Updates user information
        private void btnSave_Click(object sender, EventArgs e)
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

                    command.Parameters.AddWithValue("@AccNum", UserInfo.currentAccount);
                    command.Parameters.AddWithValue("@FName", txtFName.Text);
                    command.Parameters.AddWithValue("@MName", txtMName.Text);
                    command.Parameters.AddWithValue("@LName", txtLName.Text);
                    command.Parameters.AddWithValue("@MNumber", txtMobileNum.Text);
                    command.Parameters.AddWithValue("@DOB", dtpDOB.Text);
                    command.Parameters.AddWithValue("@PIN", txtPIN.Text);
                    command.ExecuteNonQuery();

                    retriveAccountInfo();
                    disableBtnsAndText();

                    MessageBox.Show("Information Saved and Updated!", "Notice");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Unsaved information will be removed, are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                disableBtnsAndText();
                retriveAccountInfo();
                errorProvider.SetError(txtFName, null);
                errorProvider.SetError(txtMName, null);
                errorProvider.SetError(txtLName, null);
                errorProvider.SetError(txtMobileNum, null);
                errorProvider.SetError(txtPIN, null);
            }
        }

        //Read function in CRUD
        //Retrieves data from the database
        //Refreshes textbox when there is update or when cancelled
        private void retriveAccountInfo()
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.conString))
            {
                connection.Open();

                string query = "SELECT AccountNumber, FirstName, MiddleName, LastName, MobileNumber, DateOfBirth, PIN " +
                    "FROM tblAccounts WHERE AccountNumber = @AccNum";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@AccNum", UserInfo.currentAccount);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    txtAccNum.Text = reader.GetValue(0).ToString();
                    txtFName.Text = reader.GetValue(1).ToString();
                    txtMName.Text = reader.GetValue(2).ToString();
                    txtLName.Text = reader.GetValue(3).ToString();
                    txtMobileNum.Text = reader.GetValue(4).ToString();
                    dtpDOB.Text = reader.GetValue(5).ToString();
                    txtPIN.Text = reader.GetValue(6).ToString();
                }
            }
        }

        private void disableBtnsAndText()
        {
            txtFName.Enabled = false;
            txtMName.Enabled = false;
            txtLName.Enabled = false;
            txtMobileNum.Enabled = false;
            dtpDOB.Enabled = false;
            txtPIN.Enabled = false;
            btnUpdate.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        //Validations
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

        private void txtMobileNum_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMobileNum.Text) || txtMobileNum.Text.Any(Char.IsLetter))
            {
                e.Cancel = true;
                errorProvider.SetError(txtMobileNum, "Mobile Number cannot be empty or contain letters");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMobileNum, null);
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

        private void btnDisable_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Disable this account?\nYou will be logged out, and need to contact the administrator to re-enable this account.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "UPDATE tblAccounts SET Status = " + 0 + "  WHERE AccountNumber = @AccNum";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@AccNum", Convert.ToInt32(txtAccNum.Text));
                    command.ExecuteNonQuery();

                    MessageBox.Show("Account: " + txtAccNum.Text + ", Disabled!", "Notice");

                    this.Close();

                    LoginForm frmLogin = new LoginForm();
                    frmLogin.Show();

                }
            }
        }

    }
}
