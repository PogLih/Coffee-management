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
    public partial class fAccount : Form
    {
        string acc;
        public fAccount(string acc)
        {
            this.acc = acc;
            InitializeComponent();
            changeAccount(acc);
        }
        CafeEntities db = new CafeEntities();
        void changeAccount(string acc)
        {
            Account account = db.Accounts.Where(c => c.Username == acc).SingleOrDefault();
            txbDisplayName.Text = account.DisplayName;
            txbNewPass.Text = null;
            txbPassWord.Text = null;
            txbReEnterPass.Text = null;
            txbUserName.Text = account.Username;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Account account = db.Accounts.Where(c => c.Username == acc).SingleOrDefault();
            account.DisplayName = txbDisplayName.Text;
            if (txbPassWord.Text == account.PassWord)
            {
                if (txbReEnterPass.Text == txbNewPass.Text)
                {
                    account.PassWord = txbNewPass.Text;
                }
                else
                {
                    MessageBox.Show("sai mật khẩu nhập lại");
                }
            }
            else MessageBox.Show("sai mật khẩu");
            db.SaveChanges();
            MessageBox.Show("cập nhật thành công");
        }
    }
}
