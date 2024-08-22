using System;
using System.Configuration;
using System.Windows.Forms;

namespace Sunshine
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dgv.CellContentClick += dgv_CellContentClick;
            LoadData();
        }

        private void LoadData(string searchTerm = "", int page = 1)
        {
            var data = RemoteAccountService.GetRemoteAccounts(searchTerm, page);

            dgv.DataSource = data;

            // 重新定义列标题
            dgv.Columns["id"].Visible = false;
            dgv.Columns["name"].HeaderText = "远程名称";
            dgv.Columns["remark"].HeaderText = "备注";
            dgv.Columns["code"].HeaderText = "远程编号";
            dgv.Columns["password"].HeaderText = "密码";
            dgv.Columns["type"].HeaderText = "类型";

            // 添加删除列
            if (!dgv.Columns.Contains("delete"))
            {
                DataGridViewLinkColumn deleteColumn = new DataGridViewLinkColumn
                {
                    Name = "delete",
                    HeaderText = "删除",
                    Text = "删除",
                    UseColumnTextForLinkValue = true
                };
                dgv.Columns.Add(deleteColumn);
            }

            // 添加连接列
            if (!dgv.Columns.Contains("connect"))
            {
                DataGridViewLinkColumn connectColumn = new DataGridViewLinkColumn
                {
                    Name = "connect",
                    HeaderText = "连接",
                    Text = "连接",
                    UseColumnTextForLinkValue = true
                };
                dgv.Columns.Add(connectColumn);
            }
        }
        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // ReSharper disable once PossibleNullReferenceException
            if (e.ColumnIndex == dgv.Columns["delete"].Index && e.RowIndex >= 0)
            {
                var result = MessageBox.Show("确定要删除这条记录吗？", "确认删除", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var id = dgv.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    RemoteAccountService.DeleteRemoteAccount(id);
                    LoadData(); // 刷新数据
                }
            }
            // ReSharper disable once PossibleNullReferenceException
            else if (e.ColumnIndex == dgv.Columns["connect"].Index && e.RowIndex >= 0)
            {
                var code = dgv.Rows[e.RowIndex].Cells["code"].Value.ToString();
                var pwd = dgv.Rows[e.RowIndex].Cells["password"].Value.ToString();
                try
                {
                    Operator.RunSunshine(code, pwd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    var name = addForm.RemoteName;
                    var remark = addForm.Remark;
                    var code = addForm.Code;
                    var password = addForm.Password;
                    var type = addForm.Type;

                    RemoteAccountService.AddRemoteAccount(name, remark, code, password, type);
                    LoadData(); // 刷新数据
                }
            }
        }
    }
}
