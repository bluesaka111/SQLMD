using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLMD
{
    public partial class closeConnection : Form
    {
        private string pname = null;
        BackgroundWorker bgW = null;
        public closeConnection(BackgroundWorker bg, string processname)
        {
            InitializeComponent();
            pname = processname;
            bgW = bg;
            backgroundWorker1.RunWorkerAsync(bg);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgW2 = (e.Argument as BackgroundWorker);
            if(bgW2.IsBusy)
            {
                backgroundWorker1.ReportProgress(0, bgW2);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if((e.UserState as BackgroundWorker).IsBusy)
            {
                label4.Text = "Background Worker [" + pname + "] is busy";
                label4.BackColor = Color.LightGreen;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bgW.CancelAsync();
        }
    }
}
