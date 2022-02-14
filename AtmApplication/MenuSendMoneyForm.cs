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
    public partial class MenuSendMoneyForm : Form
    {
        public MenuSendMoneyForm()
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

        private void MenuSendMoneyForm_Load(object sender, EventArgs e)
        {
            lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int totalAmount = DataAccess.getBalance(UserInfo.currentAccount) - Convert.ToInt32(numUpDownSend.Value);

            DialogResult dialogResult = MessageBox.Show("Send " + Convert.ToInt32(numUpDownSend.Value) + ", to " + 
                txtAccNum.Text +"?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes && DataAccess.getBalance(UserInfo.currentAccount) > Convert.ToInt32(numUpDownSend.Value) && verifyAccNum(txtAccNum.Text))
            {
                using (SqlConnection connection = new SqlConnection(DataAccess.conString))
                {
                    connection.Open();

                    //Takes the money from the sender
                    string sendQuery = "UPDATE tblAccounts SET Balance = @sentBalance WHERE AccountNumber = @AccNumSender";
                    SqlCommand sendCommand = new SqlCommand(sendQuery, connection);

                    sendCommand.Parameters.AddWithValue("@AccNumSender", UserInfo.currentAccount);
                    sendCommand.Parameters.AddWithValue("@sentBalance", totalAmount);
                    sendCommand.ExecuteNonQuery();

                    //Receives money from the sender
                    string receiveQuery = "UPDATE tblAccounts SET Balance = Balance + @receivedBalance WHERE AccountNumber = @AccNumReceiver";
                    SqlCommand receiveCommand = new SqlCommand(receiveQuery, connection);

                    receiveCommand.Parameters.AddWithValue("@AccNumReceiver", Convert.ToInt32(txtAccNum.Text));
                    receiveCommand.Parameters.AddWithValue("@receivedBalance", Convert.ToInt32(numUpDownSend.Value));
                    receiveCommand.ExecuteNonQuery();

                    lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
                    MessageBox.Show(Convert.ToInt32(numUpDownSend.Value) + " was sent to " + txtAccNum.Text);
                }
            }
            else if (DataAccess.getBalance(UserInfo.currentAccount) < Convert.ToInt32(numUpDownSend.Value))
            {
                MessageBox.Show("You cannot send more than your balance", "Notice");
            }
        }

        //Checks if the account number to receive the money exists in the database
        private bool verifyAccNum(string accNum)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.conString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblAccounts WHERE AccountNumber = '" + accNum + "'";
                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["Status"].ToString().Equals("1"))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("The account is disabled, sending is cancelled.\nPlease contact the administrator", "Notice");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Account number does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
        }

    }
}
