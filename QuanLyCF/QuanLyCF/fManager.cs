using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCF
{
    public partial class fManager : Form
    {
        string acc;
        int type;
        public fManager(string acc,int type)
        {
            this.acc = acc;
            this.type = type;
            InitializeComponent();
            loadcategory();
            loadswtichtable();
            loadtable();
            checkAdmin(type);
        }
        CafeEntities db = new CafeEntities();
        #region void
        void insertBill(int id)
        {
            Bill bill = new Bill();
            bill.DateCheckIn = DateTime.Now;
            bill.DateCheckOut = DateTime.Now;
            bill.idTable = id;
            bill.status = 1;
            db.Bills.Add(bill);
            db.SaveChanges();
        }
        int UncheckBill(int id)
        {
            Bill kq = db.Bills.Where(c => c.idTable == id && c.status == 1).SingleOrDefault();
            try
            {
                int idtable = kq.id;
                if (idtable != 0)
                    return idtable;
            }
            catch (Exception)
            {
            }
            return -1;
        }
        void loadswtichtable()
        {
            cbSwitchTable.DataSource = db.Tablecoffes.ToList();
            cbSwitchTable.DisplayMember = "name";
            cbSwitchTable.ValueMember = "id";
        }
        void insertBillInfo(int idbill, int iddrinks, int count)
        {
            try
            {
                BillInfo kq = db.BillInfoes.Where(c => c.idBill == idbill && c.idDrinks == iddrinks).SingleOrDefault();
                int drinkscount = kq.CountItem;
                int billinfoexit = kq.id;
                int newcount = 0;
                if (billinfoexit > 0)//nếu đã có item đó thì tăng thêm hoặc  xoá đi
                {
                    newcount = drinkscount + count;
                    if (newcount > 0)
                    {
                        kq.CountItem = drinkscount + count;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.BillInfoes.Remove(kq);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                BillInfo billinfo = new BillInfo();
                billinfo.idBill = idbill;
                billinfo.idDrinks = iddrinks;
                billinfo.CountItem = count;
                db.BillInfoes.Add(billinfo);
                db.SaveChanges();
            }

        }
        int MaxIdBill()
        {
            try
            {
                return (int)db.Bills.Max(c => c.id);
            }
            catch
            {

                return 1;
            }
        }
        void loadcategory()
        {
            cbCategory.DataSource = db.Categories.ToList();
            cbCategory.DisplayMember = "name";
        }
        void loaddrinks(int id)
        {
            cbDrinks.DataSource = db.Drinks.Where(c => c.idcategory == id).ToList();
            cbDrinks.DisplayMember = "name";
        }
        void showbill(int id)
        {
            float totalPrice = 0;
            var result = from a in db.BillInfoes
                         from b in db.Bills
                         from c in db.Drinks
                         where b.idTable == id && a.idDrinks == c.id && a.idBill == b.id && b.status == 1
                         select new { c.name, a.CountItem, c.price };
            lsvBill.Items.Clear();
            foreach (var item in result)//add item vào listview
            {
                ListViewItem lvitem = new ListViewItem();
                lvitem.Text = item.name.ToString();
                lvitem.SubItems.Add(item.CountItem.ToString());
                lvitem.SubItems.Add(item.price.ToString());
                lvitem.SubItems.Add((item.price * item.CountItem).ToString());
                totalPrice += (float)(item.price * item.CountItem);
                lsvBill.Items.Add(lvitem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            //Thread.CurrentThread.CurrentCulture = culture; dùng để chuyển đổi đơn vị tiền tệ
            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }
        void loadtable()
        {
            flpTable.Controls.Clear();
            List<Tablecoffe> table = db.Tablecoffes.ToList();
            foreach (Tablecoffe item in table)//Thêm button hiển thị bàn
            {
                if (UncheckBill(item.id) == -1)
                    item.status = 1;
                else item.status = 0;
                db.SaveChanges();
                Button btn = new Button();
                btn.Width = 100;
                btn.Height = 100;
                string a;
                if (item.status == 0)
                    a = "Có người";
                else a = "Không có người";
                btn.Text = item.name + Environment.NewLine + a;
                btn.Name = item.name;
                btn.Click += Btn_Click;
                btn.Tag = item;//chứa dữ liệu của button đó 
                if (item.status == 0)
                {
                    btn.BackColor = Color.Aqua;
                }
                else btn.BackColor = Color.Green;
                flpTable.Controls.Add(btn);
            }
        }
        void checkAdmin(int type)
        {
            adminToolStripMenuItem.Enabled = type == 0;
        }
        #endregion
        #region event
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Tablecoffe).id;//lấy dữ liệu từ button để sử dụng
            lsvBill.Tag = (sender as Button).Tag;
            showbill(tableID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.id;
            loaddrinks(id);
        }        
        private void btnCheckOut_Click(object sender, EventArgs e)//tính tiền
        {
            Tablecoffe table = lsvBill.Tag as Tablecoffe;
            int idbill = UncheckBill(table.id);
            double totalprice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            if(idbill!=-1)
            {
                if(MessageBox.Show(string.Format("Bạn có muốn thanh toán hoá đơn cho {0}\nTổng tiền là {1},000",table.name,totalprice),"Thông báo",MessageBoxButtons.OKCancel)==DialogResult.OK)
                {
                    Bill bill = db.Bills.Where(c => c.id == idbill).SingleOrDefault();
                    bill.status = 0;
                    bill.DateCheckOut = DateTime.Now;
                    bill.totalprice = (int)totalprice;
                    table.status = 1;
                    db.SaveChanges();
                    showbill(table.id);
                    loadtable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            Tablecoffe table = lsvBill.Tag as Tablecoffe;
            int idbill = UncheckBill(table.id);
            int tableswitch = int.Parse(cbSwitchTable.SelectedValue.ToString());
            int idbillswitch = UncheckBill(tableswitch);
            List <BillInfo> listbillinfo = db.BillInfoes.Where(c => c.idBill == idbill).ToList();
            if (idbillswitch==-1)
            {
                insertBill(tableswitch);
                foreach (BillInfo item in listbillinfo)
                {
                    insertBillInfo(MaxIdBill(),item.idDrinks,item.CountItem);
                }
            }
            else
            {
                foreach (BillInfo item in listbillinfo)
                {
                    insertBillInfo(idbillswitch, item.idDrinks, item.CountItem);
                }
            }
            Bill bill = db.Bills.Where(c => c.id == idbill).SingleOrDefault();
            bill.status = 0;
            db.SaveChanges();
            showbill(table.id);
            loadtable();
        }
        private void btnAddFood_Click(object sender, EventArgs e)//thêm item
        {
            Tablecoffe table = lsvBill.Tag as Tablecoffe;
            int idBill = UncheckBill(table.id);
            int drinksID = (cbDrinks.SelectedItem as Drink).id;
            int count = (int)nmDrinksCount.Value;
            if (idBill == -1)//nếu bàn chưa có người thì add vào
            {
                insertBill(table.id);
                insertBillInfo(MaxIdBill(), drinksID, count);
                table.status = 1;
                db.SaveChanges();
            }
            else//thêm item vào bàn có người
            {
                insertBillInfo(idBill, drinksID, count);
            }
            showbill(table.id);
            loadtable();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccount f = new fAccount(acc);
            f.ShowDialog();
        }
        #endregion
    }
}
