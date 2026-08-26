using DVLDBussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmUsersList : Form
    {

        static private DataTable _AllUsersTable = clsUsers.GetAllUsers();
        private DataTable _Usertable = _AllUsersTable.DefaultView.ToTable(true, "UserID", "PersonID", "FullName", "UserName", "IsActive");



        public frmUsersList()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(830, 550);
        }
        private void _RefreshUsersList() {

            _AllUsersTable = clsUsers.GetAllUsers();
            _Usertable = _AllUsersTable.DefaultView.ToTable(true, "UserID", "PersonID", "FullName", "UserName", "IsActive");

            dgvUsers.DataSource = _Usertable;
            cbFilterBy.SelectedIndex = 0;
            lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();

            if (dgvUsers.Rows.Count > 0) {

                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].HeaderText = "Person ID";
                dgvUsers.Columns[0].HeaderText = "Full Name";
                dgvUsers.Columns[0].HeaderText = "Username";
                dgvUsers.Columns[0].HeaderText = "Username";
            } 
        }
        private void frmUsersList_Load(object sender, EventArgs e)
        {

            _RefreshUsersList();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Still Not implemented", "Still Not implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Still Not implemented", "Still Not implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterText.Visible = (cbFilterBy.SelectedIndex != 0 && cbFilterBy.Text != "Is Active");
            cbIsActiveFilter.Visible = (cbFilterBy.Text == "Is Active");

            if (cbFilterBy.SelectedIndex != 0 && cbFilterBy.Text == "Is Active")
            {

                cbIsActiveFilter.Visible = false;
                cbIsActiveFilter.Visible = true;
                txtFilterText.Text = string.Empty;
                txtFilterText.Focus();
            }
            if (cbFilterBy.Text == "Is Active")
            {

                txtFilterText.Visible = false;
                cbIsActiveFilter.Visible = true;
                cbIsActiveFilter.Focus();
                cbIsActiveFilter.SelectedIndex = 0;
            }
        }
        private void txtFilterText_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {

                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "UserName":
                    FilterColumn = "UserName";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "";
                    break;
            }

            if (FilterColumn == "None" || txtFilterText.Text.Trim() == "")
            {

                _Usertable.DefaultView.RowFilter = "";
                lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID" || FilterColumn == "UserID")
                //in this case we deal with integer not string.
                _Usertable.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterText.Text.Trim());

            else if (FilterColumn == "UserName" || FilterColumn == "FullName")
                _Usertable.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterText.Text.Trim());


            lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();
        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditUser();
            frm.ShowDialog();
            _RefreshUsersList();
        }
        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditUser();
            frm.ShowDialog();
            _RefreshUsersList();
            frmUsersList_Load(null, null); // execute the instruction of load event: same function of RefreshUserList() in this case.
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete this User [{(int)dgvUsers.CurrentRow.Cells[0].Value}]", "Confirm Deleting", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {


                if (clsUsers.DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {

                    MessageBox.Show("Deleted Successfully.", "Succefull delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshUsersList();
                }
                else {

                    MessageBox.Show("Deleted not Successfully.", "Fail delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void cbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbIsActiveFilter.Text == "Yes")
                _Usertable.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", "1");
            else if (cbIsActiveFilter.Text == "No")
                _Usertable.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", "0");
            else
                _Usertable.DefaultView.RowFilter = "";
            lblNumOfRecords.Text = dgvUsers.Rows.Count.ToString();
        }

        private void txtFilterText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
