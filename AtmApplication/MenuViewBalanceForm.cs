using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtmApplication
{
    public partial class MenuViewBalanceForm : Form
    {
        public MenuViewBalanceForm()
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuViewBalanceForm_Load(object sender, EventArgs e)
        {
            lblCurrentBalance.Text = DataAccess.getBalance(UserInfo.currentAccount).ToString();
        }
    }
}
