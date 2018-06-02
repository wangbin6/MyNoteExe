using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;

/**
* 文本编辑器 
*/
namespace MYAPP_4
{
    class Program : Form
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        ///

        //输入区
        private TextBox textBox;
        //打开的文本文件的绝对路径
        private String openedFilePath = "";
        //是否已保存当前文档，默认已保存
        private Boolean isSaved = true;
        //提示是否保存的弹窗
        SimpleDialogBox isSavedDialog;
        static Program program;

        [STAThread]
        static void Main(string[] args)
        {
            //实例化窗口类
            program = new Program();

            //处理拖动txt文件到exe处理
            if (args.Length >= 1)
            {
                //args[0];//文件路径
                program.OpenFileOnDragExe(args[0]);
            }

            //文本文本窗体居中屏幕显示
            program.StartPosition = FormStartPosition.CenterScreen;

            //打开窗口
            Application.Run(program);
        }

        public Program()
        {
            //解决[error]：线程间操作无效:从不是创建控件 的线程访问它
            CheckForIllegalCrossThreadCalls = false;

            Text = "小新文本编辑器";
            Icon = Resources.Resource1.MainIcon;//窗口图标
            Size = new Size(500, 400);
            //Icon = new Icon();

            //文件菜单
            MenuItem miOpen = new MenuItem("打开", new EventHandler(MenuFileOpenOnClick), Shortcut.CtrlO);

            MenuItem miSave = new MenuItem("保存", new EventHandler(MenuFileSaveOnClick), Shortcut.CtrlS);

            MenuItem miSaveAs = new MenuItem("另存为", new EventHandler(MenuFileSaveAsOnClick));

            MenuItem miDash = new MenuItem("-");

            MenuItem miExit = new MenuItem("退出", new EventHandler(MenuFileExitOnClick));

            miExit.DefaultItem = true;

            MenuItem miFile = new MenuItem("文件(&F)", new MenuItem[] { miOpen, miSave, miSaveAs, miDash, miExit });

            //编辑菜单
            MenuItem miSelectAll = new MenuItem("全选", new EventHandler(MenuFileSelectAllOnClick), Shortcut.CtrlA);

            MenuItem miCut = new MenuItem("剪切", new EventHandler(MenuFileCutOnClick), Shortcut.CtrlX);

            MenuItem miCopy = new MenuItem("复制", new EventHandler(MenuFileCopyOnClick), Shortcut.CtrlC);

            MenuItem miPaste = new MenuItem("粘贴", new EventHandler(MenuFilePasteOnClick), Shortcut.CtrlV);

            MenuItem miEdit = new MenuItem("编辑(&E)", new MenuItem[] { miCut, miCopy, miPaste, miSelectAll });

            //格式
            MenuItem miFont = new MenuItem("字体...(&F)", new EventHandler(MenuFontOnClick));
            MenuItem miFormat = new MenuItem("格式(&O)", new MenuItem[] { miFont });

            //帮助菜单
            MenuItem miAbout = new MenuItem("关于", new EventHandler(MenuFileAboutOnClick));

            MenuItem miHelp = new MenuItem("帮助(&H)", new MenuItem[] { miAbout });

            Menu = new MainMenu(new MenuItem[] { miFile, miEdit, miFormat, miHelp });

            //文本输入编辑区域
            textBox = new TextBox();
            textBox.Parent = this;
            textBox.Dock = DockStyle.Fill;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Both;
            textBox.AcceptsTab = true;
            textBox.Font = new Font(FontFamily.GenericSansSerif,16, FontStyle.Regular);
            textBox.TextChanged += new EventHandler(TextChangedEvent);
            textBox.AllowDrop = true;
            textBox.DragDrop += new DragEventHandler(TextBox_DragDrop);

            //状态栏
            StatusStrip statusStrip = new StatusStrip();

            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.GripStyle = ToolStripGripStyle.Visible;
            statusStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            //statusStrip.Location = new Point(0, 50);
            statusStrip.Name = "statusStrip";
            statusStrip.ShowItemToolTips = true;
            //statusStrip.Size = new Size(100, 30);
            statusStrip.SizingGrip = true;
            statusStrip.Stretch = false;
            statusStrip.TabIndex = 0;

            ToolStripStatusLabel toolStripStatusLabel1 = new ToolStripStatusLabel();
            ToolStripStatusLabel toolStripStatusLabel2 = new ToolStripStatusLabel();

            toolStripStatusLabel1.Text = "状态栏";
            toolStripStatusLabel1.BorderSides = ToolStripStatusLabelBorderSides.None;
            toolStripStatusLabel1.Spring = true;

            toolStripStatusLabel2.Text = "当前时间：" + DateTime.Now.ToString();

            //toolStripStatusLabel1.BorderSides = ToolStripStatusLabelBorderSides.All;
            //toolStripStatusLabel2.BorderSides = ToolStripStatusLabelBorderSides.All;
            //toolStripStatusLabel1.BorderStyle = Border3DStyle.Sunken;
            //toolStripStatusLabel2.BorderStyle = Border3DStyle.Sunken;

            MyUnits myUnits = new MyUnits(toolStripStatusLabel2);//公共函数类

            toolStripStatusLabel2.Alignment = ToolStripItemAlignment.Right;

            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1,toolStripStatusLabel2 });

            Controls.Add(textBox);
            Controls.Add(statusStrip);

        }

        private void TextBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textBox.Text = files[0];
            }
        }

        /// <summary>
        /// 打开通过拖拽txt文件到exe图标中的文件
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        public void OpenFileOnDragExe(string FilePath)
        {
            StreamReader streamReader = new StreamReader(FilePath, Encoding.UTF8);
            textBox.Text = streamReader.ReadToEnd();
            textBox.Select(textBox.TextLength, 0);

            streamReader.Close();//关闭文件读取流

            //设置文本保存标识为已保存
            isSaved = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
        }

        //文本编辑区内容change事件
        public void TextChangedEvent(object o,EventArgs e)
        {
            //设置文本保存标识为未保存
            isSaved = false;

            TextBox textBox = (TextBox)o;
            //如果textbox为空则不用保存的
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                isSaved = true;
            }
        }

        //打开文件
        public void MenuFileOpenOnClick(object o, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "txt files(*.txt)|*.txt";//All files(*.*)|*.*|
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openedFilePath = openFileDialog.FileName.ToString();//打开文件的绝对路径

                StreamReader streamReader = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
                textBox.Text = streamReader.ReadToEnd();
                textBox.Select(textBox.TextLength, 0);

                streamReader.Close();//关闭文件读取流

                //设置文本保存标识为已保存
                isSaved = true;
            }
        }

        //文件保存
        public void MenuFileSaveOnClick(object o, EventArgs e)
        {
            if (textBox.Text == "") return;

            if (openedFilePath != "")
            {
                //保存打打开的文件中
                string text = textBox.Text;
                StreamWriter stringWriter = new StreamWriter(openedFilePath,false);
                stringWriter.WriteLine(text);//写入
                stringWriter.Close();
            }
            else
            {
                //新建文件并保存
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = @"文本文件|*.txt*";
                String text = textBox.Text;//待保存的文本

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    openedFilePath = saveFileDialog.FileName;

                    StreamWriter streamWriter = new StreamWriter(openedFilePath, true);
                    streamWriter.WriteLine(text);
                    streamWriter.Close();
                }
                else
                {
                    //设置文本保存标识为未保存
                    isSaved = false;
                    return;
                }
            }

            //设置文本保存标识为已保存
            isSaved = true;
        }

        //另存为
        public void MenuFileSaveAsOnClick(object o, EventArgs e)
        {
            //输入区不为空
            if (textBox.Text != "")
            {
                //另存为
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = @"文本文件|*.txt*";
                String text = textBox.Text;//待保存的文本

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    openedFilePath = saveFileDialog.FileName;

                    StreamWriter streamWriter = new StreamWriter(openedFilePath, true);
                    streamWriter.WriteLine(text);
                    streamWriter.Close();
                }
            }
            else
            {

            }
        }

        //退出软件
        public void MenuFileExitOnClick(object o, EventArgs e)
        {
            if (isSaved)
            {
                System.Environment.Exit(0);
            }
            else
            {
                isSavedDialog = new SimpleDialogBox();
                isSavedDialog.StartPosition = FormStartPosition.CenterParent;
                String tip = "您是否要将改变的内容保存到" + openedFilePath + "文件中？";
                isSavedDialog.SaveTextBtnDialog(tip);

                isSavedDialog.ShowDialog();
            }
        }

        //剪切
        public void MenuFileCutOnClick(object o, EventArgs e)
        {
            
        }

        //复制
        public void MenuFileCopyOnClick(object o, EventArgs e)
        {
            //选中的内容不为空
            if (textBox.SelectedText != "")
            {
                Clipboard.SetDataObject(textBox.SelectedText);
            }
            else
            {

            }
        }

        //粘贴
        public void MenuFilePasteOnClick(object o, EventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                textBox.AppendText((String)dataObject.GetData(DataFormats.Text));
            }
            else
            {

            }
        }

        public void MenuFileAboutOnClick(object o, EventArgs e)
        {
            SimpleDialogBox simpleDialogBox = new SimpleDialogBox();

            //初始化对话框信息
            simpleDialogBox.SetText("关于");
            simpleDialogBox.SetSize(new Size(200,100));
            simpleDialogBox.SetMinimizeBox(false);
            simpleDialogBox.SetMaximizeBox(false);
            simpleDialogBox.SetControlBox(true);
            simpleDialogBox.SetShowInTaskbar(false);
            simpleDialogBox.SetFormBorderStyle(FormBorderStyle.Fixed3D);

            //相对于父亲窗口居中
            simpleDialogBox.StartPosition = FormStartPosition.CenterParent;

            //生成版本信息
            simpleDialogBox.AboutLabelDialog();

            //显示对话框
            simpleDialogBox.ShowDialog();
        }

        //全选
        public void MenuFileSelectAllOnClick(object o,EventArgs e)
        {
            textBox.SelectAll();
        }

        public void MenuFontOnClick(object o,EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Font = fontDialog.Font;
            }
            
        }

        /// <summary>
        /// 重写OnFormClosing方法，提示保存
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //文件没有保存弹出保存提示窗口
            if (!isSaved)
            {
                MenuFileExitOnClick(null, null);

                e.Cancel = true;//取消关闭窗口事件
            }
        }

        /// <summary>
        /// 获取该主窗体Form对象
        /// </summary>
        /// <returns></returns>
        public static Form GetMainForm()
        {
            return program;
        }
    }
}
