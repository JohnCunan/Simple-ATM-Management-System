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
    public partial class RegisterForm : Form
    {
        LoginForm frmLogin;
        public RegisterForm()
        {
            InitializeComponent();
            frmLogin = new LoginForm();
        }

        //Creates shadow on forms
        protected override CreateParams CreateParams
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
            DialogResult dialogResult = MessageBox.Show("Unsaved data will be lost\nWould you like to close?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                this.Dispose();
                frmLogin.Show();

                errorProvider.SetError(txtFNameReg, null);
                errorProvider.SetError(txtMNameReg, null);
                errorProvider.SetError(txtLNameReg, null);
                errorProvider.SetError(txtMobileNumReg, null);
                errorProvider.SetError(txtPin, null);
            }
        }

        //Generates account number
        private void RegisterForm_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            int randomAccNum = 20000000 + random.Next(0,10001);
            txtAccNumReg.Text = randomAccNum.ToString();
        }

        //Create function for CRUD
        //Inserts new data to database
        private void btnRegister_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Complete registration?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "INSERT INTO tblAccounts(AccountNumber ,FirstName, MiddleName, LastName, MobileNumber, DateOfBirth, PIN, Balance, Status)" +
                         "VALUES('" + Convert.ToInt32(txtAccNumReg.Text) + "','" + txtFNameReg.Text + "','" + txtMNameReg.Text + "','" + txtLNameReg.Text + "','" + txtMobileNumReg.Text + "','" + dtpDOBReg.Text + "','" + txtPin.Text + "','" + 0 + "','" + 0 + "')";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Registered Successfully!\n \nAccount Number: " + txtAccNumReg.Text +
                        "\nFirst Name: " + txtFNameReg.Text + "\nMidle Name: " + txtMobileNumReg.Text +
                        "\nLast Name: " + txtLNameReg.Text + "\nMobile Number: " + txtMobileNumReg.Text +
                        "\nPIN: " + txtPin.Text, "Notice");
                }

                txtAccNumReg.ResetText();
                txtFNameReg.ResetText();
                txtMNameReg.ResetText();
                txtLNameReg.ResetText();
                txtMobileNumReg.ResetText();
                txtPin.ResetText();

            }
        }

        //Validations
        private void txtFNameReg_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFNameReg.Text) || txtFNameReg.Text.Any(Char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtFNameReg, "First name cannot be empty or contain digits");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtFNameReg, null);
            }
        }

        private void txtMNameReg_Validating(object sender, CancelEventArgs e)
        {
            if (txtMNameReg.Text.Any(Char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtMNameReg, "Middle name contain digits");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMNameReg, null);
            }
        }

        private void txtLNameReg_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLNameReg.Text) || txtLNameReg.Text.Any(Char.IsDigit))
            {
                e.Cancel = true;
                errorProvider.SetError(txtLNameReg, "Last name cannot be empty or contain digits");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtLNameReg, null);
            }
        }

        private void txtMobileNumReg_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMobileNumReg.Text) || txtMobileNumReg.Text.Any(Char.IsLetter))
            {
                e.Cancel = true;
                errorProvider.SetError(txtMobileNumReg, "Mobile Number cannot be empty or contain letters");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMobileNumReg, null);
            }
        }

        private void txtPin_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPin.Text) || txtPin.Text.Any(Char.IsLetter))
            {
                e.Cancel = true;
                errorProvider.SetError(txtPin, "PIN cannot be empty or contain letters");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtPin, null);
            }
        }
    }
}
