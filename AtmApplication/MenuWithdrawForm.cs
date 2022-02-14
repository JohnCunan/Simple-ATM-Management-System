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
    public partial class MenuWithdrawForm : Form
    {
        public MenuWithdrawForm()
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

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuWithdrawForm_Load(object sender, EventArgs e)
        {
            lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Withdraw " + Convert.ToInt32(numUpDownWithdraw.Value) + ", from this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (dialogResult == DialogResult.Yes && DataAccess.getBalance(UserInfo.currentAccount) > Convert.ToInt32(numUpDownWithdraw.Value))
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    string query = "UPDATE tblAccounts SET Balance = Balance - @withdrawedBalance WHERE AccountNumber = @AccNum";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@AccNum", UserInfo.currentAccount);
                    command.Parameters.AddWithValue("@withdrawedBalance", Convert.ToInt32(numUpDownWithdraw.Value));
                    command.ExecuteNonQuery();

                    lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
                }
            }
            else if (DataAccess.getBalance(UserInfo.currentAccount) < Convert.ToInt32(numUpDownWithdraw.Value))
            {
                MessageBox.Show("You cannot withdraw more than your balance", "Notice");
            }
        }
    }
}
