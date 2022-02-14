using System;
using System.Drawing;
using System.Windows.Forms;

namespace AtmApplication
{
    public partial class MainMenuForm : Form
    {
        LoginForm frmLogin;
        public MainMenuForm()
        {
            InitializeComponent();
            frmLogin = new LoginForm();
            displayNameMainMenu();
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
            frmLogin.Show();
        }

        //Responsiveness for menu options
        private void pnlViewBalance_MouseEnter(object sender, EventArgs e)
        {
            lblViewBalance.Font = new Font(lblViewBalance.Font.Name, lblViewBalance.Font.SizeInPoints, FontStyle.Underline);
            pnlViewBalance.BackColor = Color.Navy;
        }

        private void pnlViewBalance_MouseLeave(object sender, EventArgs e)
        {
            lblViewBalance.Font = new Font(lblViewBalance.Font.Name, lblViewBalance.Font.SizeInPoints, FontStyle.Regular);
            pnlViewBalance.BackColor = Color.FromArgb(20, 83, 143);
        }

        private void pnlDeposit_MouseEnter(object sender, EventArgs e)
        {
            lblDeposit.Font = new Font(lblDeposit.Font.Name, lblDeposit.Font.SizeInPoints, FontStyle.Underline);
            pnlDeposit.BackColor = Color.Navy;
        }

        private void pnlDeposit_MouseLeave(object sender, EventArgs e)
        {
            lblDeposit.Font = new Font(lblDeposit.Font.Name, lblDeposit.Font.SizeInPoints, FontStyle.Regular);
            pnlDeposit.BackColor = Color.FromArgb(20, 83, 143);
        }

        private void pnlWithdraw_MouseEnter(object sender, EventArgs e)
        {
            lblWithdraw.Font = new Font(lblWithdraw.Font.Name, lblWithdraw.Font.SizeInPoints, FontStyle.Underline);
            pnlWithdraw.BackColor = Color.Navy;
        }

        private void pnlWithdraw_MouseLeave(object sender, EventArgs e)
        {
            lblWithdraw.Font = new Font(lblWithdraw.Font.Name, lblWithdraw.Font.SizeInPoints, FontStyle.Regular);
            pnlWithdraw.BackColor = Color.FromArgb(20, 83, 143);
        }

        private void nlViewAccount_MouseEnter(object sender, EventArgs e)
        {
            lblViewAccount.Font = new Font(lblViewAccount.Font.Name, lblViewAccount.Font.SizeInPoints, FontStyle.Underline);
            pnlViewAccount.BackColor = Color.Navy;
        }

        private void nlViewAccount_MouseLeave(object sender, EventArgs e)
        {
            lblViewAccount.Font = new Font(lblViewAccount.Font.Name, lblViewAccount.Font.SizeInPoints, FontStyle.Regular);
            pnlViewAccount.BackColor = Color.FromArgb(20, 83, 143);
        }

        private void pnlLogout_MouseEnter(object sender, EventArgs e)
        {
            lblLogout.Font = new Font(lblLogout.Font.Name, lblLogout.Font.SizeInPoints, FontStyle.Underline);
            pnlLogout.BackColor = Color.Navy;
        }

        private void pnlLogout_MouseLeave(object sender, EventArgs e)
        {
            lblLogout.Font = new Font(lblLogout.Font.Name, lblLogout.Font.SizeInPoints, FontStyle.Regular);
            pnlLogout.BackColor = Color.FromArgb(20, 83, 143);
        }

        //Menu options clicked
        private void pnlViewBalance_MouseClick(object sender, MouseEventArgs e)
        {
            MenuViewBalanceForm menuViewBalFrm = new MenuViewBalanceForm();
            menuViewBalFrm.ShowDialog();
        }

        private void pnlDeposit_MouseClick(object sender, MouseEventArgs e)
        {
            MenuDepositForm menuDepositFrm = new MenuDepositForm();
            menuDepositFrm.ShowDialog();
        }

        private void pnlWithdraw_MouseClick(object sender, MouseEventArgs e)
        {
            MenuWithdrawForm menuWithdrawFrm = new MenuWithdrawForm();
            menuWithdrawFrm.ShowDialog();
        }

        private void pnlViewAccount_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
            MenuViewAcountForm menuViewAccFrm = new MenuViewAcountForm();
            menuViewAccFrm.ShowDialog();
        }

        private void pnlLogout_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Log out from this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                frmLogin.Show();
            }
        }

        //Displays current user info
        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            lblAccNumMain.Text = UserInfo.currentAccount.ToString();
            displayNameMainMenu();
        }

        public void displayNameMainMenu()
        {
            lblAccNameMain.Text = DataAccess.getFullName(UserInfo.currentAccount);
        }


        //Additional feature: Send Money
        //Sends money to the entered Account number
        private void pnlSendMoney_MouseEnter(object sender, EventArgs e)
        {
            lblSendMoney.Font = new Font(lblSendMoney.Font.Name, lblSendMoney.Font.SizeInPoints, FontStyle.Underline);
            pnlSendMoney.BackColor = Color.Navy;
        }

        private void pnlSendMoney_MouseLeave(object sender, EventArgs e)
        {
            lblSendMoney.Font = new Font(lblSendMoney.Font.Name, lblSendMoney.Font.SizeInPoints, FontStyle.Regular);
            pnlSendMoney.BackColor = Color.FromArgb(20, 83, 143);
        }

        private void pnlSendMoney_MouseClick(object sender, MouseEventArgs e)
        {
            MenuSendMoneyForm sendFrm = new MenuSendMoneyForm();
            sendFrm.Show();
        }
    }
}
