using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCF
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        CafeEntities db = new CafeEntities();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var check = db.Accounts.Where(c => c.Username.Equals(txbUserName.Text) && c.PassWord.Equals(txbPassWord.Text)).FirstOrDefault();
            if (check != null)
            {
                this.Hide();
                fManager f = new fManager(check.Username,check.Type);
                f.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("Sai tài khoản hoặc mật khẩu !!!");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình không ?","Thông Báo",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
