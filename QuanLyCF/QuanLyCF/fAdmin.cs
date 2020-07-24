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
    public partial class fAdmin : Form
    {
        public fAdmin()
        {
            InitializeComponent();
            load();
        }
        CafeEntities db = new CafeEntities();
        private void btViewBill_Click(object sender, EventArgs e)
        {
            loadBill();
        }
        void load()
        {
            loadBill();
            loadDrinks();
            loadCategory();
            loadtable();
            loadaccount();
        }
        void loadBill()
        {
            var result =
                from b in db.Bills
                from t in db.Tablecoffes
                where b.DateCheckIn <= dtpFromDate.Value && b.DateCheckOut <= dtpToDate.Value && b.status == 0 && t.id == b.idTable
                select new { TênBàn = t.name, TổngTiền = b.totalprice, NgàyVào = b.DateCheckIn, NgàyRa = b.DateCheckOut };
            dtgBill.DataSource = result.ToList();
        }
        void loadDrinks()
        {
            dtgDrinks.DataSource = db.Drinks.Select(c => new { c.id, c.idcategory, c.name, c.price }).ToList();
            cbDrinksCategory.DataSource = db.Categories.ToList();
            cbDrinksCategory.DisplayMember = "Name";
        }
        void loadCategory()
        {
            dtgCategory.DataSource = db.Categories.Select(c=>new { c.id,c.name}).ToList();
        }
        void loadtable()
        {
            dtgTable.DataSource = db.Tablecoffes.Select(c => new { c.id, c.name, c.status }).ToList();
        }
        void loadaccount()
        {
            dtgAccount.DataSource = db.Accounts.Select(c=>new { c.Username,c.DisplayName,c.PassWord,c.Type}).ToList();
        }
        #region drinkbutton
        private void btFindDrinks_Click(object sender, EventArgs e)
        {
            dtgDrinks.DataSource = db.Drinks.Where(c => c.name.Contains(tbFindDrinks.Text)).ToList();
        }
        private void btAddDrinks_Click(object sender, EventArgs e)
        {
            string a = cbDrinksCategory.Text;
            var takecategoryid = db.Categories.Where(c => c.name == a).SingleOrDefault();
            Drink drink = new Drink();
            drink.idcategory = takecategoryid.id;
            drink.name = tbDrinksName.Text;
            drink.price = Convert.ToDouble(nmdDrinksPrice.Value);
            db.Drinks.Add(drink);
            db.SaveChanges();
            MessageBox.Show("thêm thành công");
            loadDrinks();
        }

        private void btEditDrinks_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(tbDrinksID.Text);
            var takecategoryid = db.Drinks.Where(c => c.id == a).SingleOrDefault();
            Drink drink = db.Drinks.Where(c => c.id == a).SingleOrDefault();
            drink.idcategory = takecategoryid.idcategory;
            drink.name = tbDrinksName.Text;
            drink.price = Convert.ToDouble(nmdDrinksPrice.Value);
            db.SaveChanges();
            MessageBox.Show("Sửa thành công");
            loadDrinks();
        }

        private void btDeleteDrinks_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("bạn có muốn xoá món này không ???", "Thông Báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int a = Convert.ToInt32(tbDrinksID.Text);
                Drink drink = db.Drinks.Where(c => c.id == a).SingleOrDefault();
                db.Drinks.Remove(drink);
                db.SaveChanges();
                MessageBox.Show("Xoá thành công");
                loadDrinks();
            }
        }
        #endregion
        #region CategoriesButton
        private void btCategoryAdd_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            category.name = tbCategoryName.Text;
            db.Categories.Add(category);
            db.SaveChanges();
            MessageBox.Show("Thêm thành công");
            loadCategory();
        }

        private void btCategoryDelete_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(tbCategoryID.Text);
            Category category = db.Categories.Where(c => c.id == a).SingleOrDefault();
            db.Categories.Remove(category);
            db.SaveChanges();
            MessageBox.Show("Xoá thành công");
            loadCategory();
        }

        private void btCategoryEdit_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(tbCategoryID.Text);
            Category category = db.Categories.Where(c => c.id == a).SingleOrDefault();
            category.name = tbCategoryName.Text;
            db.SaveChanges();
            MessageBox.Show("Sửa thành công");
            loadCategory();
        }
        #endregion
        #region tablebutton
        private void btTableAdd_Click(object sender, EventArgs e)
        {
            Tablecoffe table = new Tablecoffe();
            table.name = tbTableName.Text;
            if (rdbtYes.Checked)
                table.status = 1;
            else table.status = 0;
            db.Tablecoffes.Add(table);
            db.SaveChanges();
            MessageBox.Show("Thêm thành công");
            loadtable();
        }

        private void btTableDelete_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(tbTableID.Text);
            Tablecoffe table = db.Tablecoffes.Where(c => c.id == a).SingleOrDefault();
            db.Tablecoffes.Remove(table);
            db.SaveChanges();
            MessageBox.Show("Xoá thành công");
            loadtable();
        }

        private void btTableEdit_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(tbTableID.Text);
            Tablecoffe table = db.Tablecoffes.Where(c => c.id == a).SingleOrDefault();
            table.name = tbTableName.Text;
            if (rdbtYes.Checked)
                table.status = 1;
            else table.status = 0;
            db.SaveChanges();
            MessageBox.Show("sửa thành công");
            loadtable();
        }
        #endregion
        private void dtgDrinks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbDrinksID.Text = dtgDrinks.CurrentRow.Cells[0].Value.ToString();
            tbDrinksName.Text = dtgDrinks.CurrentRow.Cells[2].Value.ToString();
            int a = Convert.ToInt32(tbDrinksID.Text);
            var takecategoryid = db.Drinks.Where(c => c.id == a).SingleOrDefault();
            cbDrinksCategory.SelectedItem = db.Categories.Where(c => c.id == takecategoryid.idcategory).SingleOrDefault();
            nmdDrinksPrice.Value = decimal.Parse(dtgDrinks.CurrentRow.Cells[3].Value.ToString());
        }

        private void dtgCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbCategoryID.Text = dtgCategory.CurrentRow.Cells[0].Value.ToString();
            tbCategoryName.Text = dtgCategory.CurrentRow.Cells[1].Value.ToString();
        }

        private void dtgTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbTableID.Text = dtgTable.CurrentRow.Cells[0].Value.ToString();
            tbTableName.Text = dtgTable.CurrentRow.Cells[1].Value.ToString();
            int a = Convert.ToInt32(tbTableID.Text);
            Tablecoffe table = db.Tablecoffes.Where(c => c.id == a).SingleOrDefault();
            if (table.status == 1)
                rdbtYes.Checked = true;
            else rdbtNo.Checked = true;
        }

        private void dtgAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbUserName.Text = dtgAccount.CurrentRow.Cells[0].Value.ToString();
            tbDisplayName.Text = dtgAccount.CurrentRow.Cells[1].Value.ToString();
            cbType.Value = decimal.Parse(dtgAccount.CurrentRow.Cells[3].Value.ToString());
            tbpassword.Text = dtgAccount.CurrentRow.Cells[2].Value.ToString();
        }

        private void btAccountAdd_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.Username = tbUserName.Text;
            account.DisplayName = tbDisplayName.Text;
            account.Type =Int32.Parse(cbType.Value.ToString());
            account.PassWord = tbpassword.Text;
            db.Accounts.Add(account);
            db.SaveChanges();
            MessageBox.Show("Thêm thành công");
            loadaccount();
        }

        private void btAccountDelete_Click(object sender, EventArgs e)
        {
            Account account = db.Accounts.Where(c => c.Username == tbUserName.Text).SingleOrDefault();
            db.Accounts.Remove(account);
            db.SaveChanges();
            MessageBox.Show("Xoá thành công");
            loadaccount();
        }

        private void btAccountEdit_Click(object sender, EventArgs e)
        {
            Account account = db.Accounts.Where(c => c.Username == tbUserName.Text).SingleOrDefault();
            account.DisplayName = tbDisplayName.Text;
            account.Type = Int32.Parse(cbType.Value.ToString());
            account.PassWord = tbpassword.Text;
            db.SaveChanges();
            MessageBox.Show("Sửa thành công");
            loadaccount();
        }
    }
}
