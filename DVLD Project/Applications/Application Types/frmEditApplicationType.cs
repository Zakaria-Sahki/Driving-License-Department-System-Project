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
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationTypeID;
        private clsApplicationType ApplicationType;

        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {

            ApplicationType = clsApplicationType.GetApplicationTypeInfo(_ApplicationTypeID);

            if (ApplicationType != null) {

                lblApplicationTypeID.Text = ApplicationType.ID.ToString();
                txtApplicationTypeFees.Text = ApplicationType.Fees.ToString();
                txtApplicationTypeTitle.Text = ApplicationType.Title.ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren()) {

                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplicationType.Title = txtApplicationTypeTitle.Text.Trim();
            ApplicationType.Fees = Convert.ToSingle(txtApplicationTypeFees.Text.Trim());


            if (ApplicationType.Save())
            {

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {

                MessageBox.Show("Error: Data is not saved Successfully!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicationTypeTitle.Text.Trim()))
            {

                e.Cancel = true;
                txtApplicationTypeTitle.Focus();
                errorProvider1.SetError(txtApplicationTypeTitle, "Title Cannot be empty!");
            }
            else {

                errorProvider1.SetError(txtApplicationTypeTitle, null);
            }
        }
        private void txtApplicationTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicationTypeFees.Text.Trim()))
            {

                e.Cancel = true;
                txtApplicationTypeFees.Focus();
                errorProvider1.SetError(txtApplicationTypeFees, "Fees cannot be empty!");
            }
            else
            {

                errorProvider1.SetError(txtApplicationTypeFees, null);
            }

            if (!clsValidation.IsNumber(txtApplicationTypeFees.Text.Trim()))
            {

                e.Cancel = true;
                txtApplicationTypeFees.Focus();
                errorProvider1.SetError(txtApplicationTypeFees, "Invalid Number!");
            }
            else {

                errorProvider1.SetError(txtApplicationTypeFees, null);
            }
        }
    }
}
