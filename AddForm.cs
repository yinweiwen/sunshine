using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunshine
{
    public partial class AddForm : Form
    {
        public string RemoteName => txtName.Text;
        public string Remark => txtRemark.Text;
        public string Code => txtCode.Text;
        public string Password => txtPassword.Text;
        public string Type => cmbType.SelectedItem.ToString();
        public AddForm()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            cmbType.Items.Add("向日葵");
            cmbType.Items.Add("ToDesk");
            cmbType.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RemoteName) || string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(Type))
            {
                MessageBox.Show("远程名称、远程编号和类型不能为空！");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
