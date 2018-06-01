using System;
using System.Timers;
using System.Windows.Forms;
namespace MYAPP_4
{
    class MyUnits
    {
        private ToolStripStatusLabel toolStripStatusLabel;

        public MyUnits(ToolStripStatusLabel toolStripStatusLabel)
        {
            this.toolStripStatusLabel = toolStripStatusLabel;

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(MyTimer);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        //返回当前时间字符串
        public void MyTimer(object o,ElapsedEventArgs args)
        {
            DateTime dt = DateTime.Now;

            toolStripStatusLabel.Text="当前时间："+ dt.ToString();
        }
    }
}
