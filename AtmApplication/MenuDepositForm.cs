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
    public partial class MenuDepositForm : Form
    {
        public MenuDepositForm()
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
            this.Close();
        }

        private void MenuDepositForm_Load(object sender, EventArgs e)
        {
            lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
        }

        //Update function for CRUD
        //Deposits to balance
        private void btnDeposit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Deposit "+ Convert.ToInt32(numUpDownDeposit.Value) + ", in this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "UPDATE tblAccounts SET Balance = Balance + @depositedBalance WHERE AccountNumber = @AccNum";
                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@AccNum", UserInfo.currentAccount);
                        command.Parameters.AddWithValue("@depositedBalance", Convert.ToInt32(numUpDownDeposit.Value));
                        command.ExecuteNonQuery();
                    }

                    lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
                }
            }
        }
    }
}
