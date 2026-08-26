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
    public partial class uctrlUserInfo : UserControl
    {
        private clsUsers User;
        private int _UserID = -1;
        public int UserID
        {

            get { return _UserID; }
        }

        public uctrlUserInfo()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {

            _UserID = UserID;
            User = clsUsers.FindUser(_UserID);
            if (User == null)
            {

                _ResetPersonInfo();
                MessageBox.Show($"No User with UserID = [{UserID}].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FillUserInfo();
        }
        private void FillUserInfo()
        {

            uCPersonInfo1.LoadPersonInfo(User._PersonID);
            lblUserID.Text = User._UserID.ToString();
            lblUsername.Text = User._UserName;
            lblIsActive.Text = (User._IsActive == true) ? "Yes" : "No";
        }
        private void _ResetPersonInfo()
        {

            uCPersonInfo1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUsername.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
    }
}
