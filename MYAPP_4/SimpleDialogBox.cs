using System;
using System.Windows.Forms;
using System.Drawing;

namespace MYAPP_4
{
    /// <summary>
    /// 自定义弹出对话框类
    /// </summary>
    class SimpleDialogBox:Form
    {
        public SimpleDialogBox()
        {
            /*初始化默认参数*/
            Visible = false;
            Text = "对话框";
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = true;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Size = new Size(300, 200);
        }

        /// <summary>
        /// 设置对话框标题
        /// </summary>
        /// <param name="text">标题</param>
        public void SetText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 设置是否显示窗口关闭按钮
        /// </summary>
        /// <param name="flag"></param>
        public void SetControlBox(bool flag)
        {
            ControlBox = flag;
        }

        /// <summary>
        /// 设置是否显示窗口最大化按钮
        /// </summary>
        /// <param name="flag"></param>
        public void SetMaximizeBox(bool flag)
        {
            MaximizeBox = flag;
        }

        /// <summary>
        /// 设置是否显示窗口最小化按钮
        /// </summary>
        /// <param name="flag"></param>
        public void SetMinimizeBox(bool flag)
        {
            MinimizeBox = flag;
        }

        /// <summary>
        /// 设置弹窗是否在任务栏图标
        /// </summary>
        /// <param name="flag"></param>
        public void SetShowInTaskbar(bool flag)
        {
            ShowInTaskbar = flag;
        }

        /// <summary>
        /// 设置弹窗边框样式
        /// </summary>
        public void SetFormBorderStyle(FormBorderStyle formBorderStyle)
        {
            FormBorderStyle = formBorderStyle;
        }
        
        /// <summary>
        /// 设置对话框大小
        /// </summary>
        public void SetSize(Size size)
        {
            Size = size;
        }

        /// <summary>
        /// 版本提示对话框信息
        /// </summary>
        public void AboutLabelDialog()
        {
            Label label1 = new Label();
            label1.Text = "应用名称：简易文本编辑器\n版本号：1.0.0";
            label1.Top = 20;
            label1.Left = 5;
            label1.Parent = this;
            label1.Size = new Size(ClientRectangle.Width, ClientRectangle.Height);
        }

        /// <summary>
        /// 关闭软件前未保存文本提示保存对话框
        /// </summary>
        /// <param name="tip">保存提示语</param>
        public void SaveTextBtnDialog(String tip)
        {
            Label label = new Label();
            label.Text = tip;
            label.Parent = this;
            label.Height = ClientRectangle.Height;
            label.Width = ClientRectangle.Width;
            label.TextAlign = ContentAlignment.TopLeft;
            label.TabIndex = 0;

            label.Font = new Font("微软雅黑", 15, FontStyle.Regular);

            Panel panel = new Panel();
            panel.Parent = this;
            panel.Height = 50;
            panel.Dock = DockStyle.Bottom;
            panel.BackColor = ColorTranslator.FromHtml("#FFF");
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Visible = true;
            panel.BringToFront();//panel显示最上层

            panel.Controls.Clear();

            Button saveBtn = new Button();
            saveBtn.Parent = panel;
            saveBtn.Name = "SaveTxt";
            saveBtn.Text = "保存";
            saveBtn.Size = new Size(80, 2 * Font.Height);
            saveBtn.Visible = true;
            saveBtn.Font = new Font("微軟雅黑", 13, FontStyle.Regular);
            saveBtn.Location = new Point(15, 7);
            saveBtn.Click += new EventHandler(CloseOnSaveTextBtn);

            Button noSaveBtn = new Button();
            noSaveBtn.Parent = panel;
            noSaveBtn.Name = "noSaveTxt";
            noSaveBtn.Text = "不保存";
            noSaveBtn.Size = new Size(80, 2 * Font.Height);
            noSaveBtn.Visible = true;
            noSaveBtn.Font = new Font("微軟雅黑", 13, FontStyle.Regular);
            noSaveBtn.Location = new Point(100, 7);
            noSaveBtn.Click += new EventHandler(CloseOnNoSaveTextBtn);

            Button cancelBtn = new Button();
            cancelBtn.Parent = panel;
            cancelBtn.Name = "cancelTxt";
            cancelBtn.Text = "取消";
            cancelBtn.Size = new Size(80, 2 * Font.Height);
            cancelBtn.Visible = true;
            cancelBtn.Font = new Font("微軟雅黑", 13, FontStyle.Regular);
            cancelBtn.Location = new Point(190, 7);
            cancelBtn.Click += new EventHandler(CloseOnCancelTextBtn);

            panel.Controls.Add(saveBtn);
        }

        /// <summary>
        /// 關閉前保存文本
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void CloseOnSaveTextBtn(object o,EventArgs e)
        {
            Program a = (Program)Program.GetMainForm();
            a.MenuFileSaveOnClick(o,e);
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 关闭前不保存文本
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void CloseOnNoSaveTextBtn(object o, EventArgs e)
        {
            System.Environment.Exit(0);
        }


        /// <summary>
        /// 关闭前取消
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void CloseOnCancelTextBtn(object o, EventArgs e)
        {
            this.Close();
        }
    }
}
