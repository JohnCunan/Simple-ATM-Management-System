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
    public partial class LoginForm : Form
    {

        public LoginForm()
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //to close all background process
            System.Windows.Forms.Application.Exit();
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            RegisterForm frmReg = new RegisterForm();
            frmReg.Show();
            this.Hide();
        }

        //Read function in CRUD
        //Checks if the inputted account number and passsword exists in the database
        private bool verifyUser(string accNum, string pin)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.conString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblAccounts WHERE AccountNumber = '" + accNum + "' AND PIN = '" + pin + "'";
                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["Status"].ToString().Equals("1")) 
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("This account is disabled, please contact the administrator", "Notice");
                        return false;
                    }
                }
                else if (txtAccNum.Text.Equals("2000") && txtPIN.Text.Equals("2000"))
                {
                    AdminForm adminFrm = new AdminForm();
                    adminFrm.Show();

                    this.Hide();
                    return false;
                }
                else
                {
                    MessageBox.Show("Wrong username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        
        
        //Read function in CRUD
        //Gets the current balance
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (verifyUser(txtAccNum.Text, txtPIN.Text))
            {
                loginAccount();
            }
        }

        //Consumes any alphabetical letters
        private void txtAccNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        //Logins when pressed enter
        private void txtAccNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginAccount();
            }
        }

        private void txtPIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginAccount();
            }

        }

        //Checks if the users is verified
        //and declares is data to the variables at UserInfo class
        private void loginAccount()
        {
            if (verifyUser(txtAccNum.Text, txtPIN.Text))
            {
                //Retrieve account number
                //Declares the current account number to the userInfo class
                UserInfo.currentAccount = DataAccess.getAccountNumber(txtAccNum.Text);

                //Get full name from the retrieved account number 
                UserInfo.currentFullName = DataAccess.getFullName(UserInfo.currentAccount);

                //Get current balance of the logged account
                //Declares the current balance to the userInfo class
                UserInfo.currentBalance = Convert.ToInt32(DataAccess.getBalance(UserInfo.currentAccount));

                MainMenuForm frmMenu = new MainMenuForm();
                frmMenu.Show();
                this.Hide();
            }
        }

    }
}
