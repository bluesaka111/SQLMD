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
    public partial class Pausemenu : Form
    {
        private Form temporate;
        public bool runFromApp = false;
        public bool isShutdown = false;
        public bool isMaximize = false;
        public bool isMinimize = false;
        public Pausemenu(Form term)
        {
            temporate = term;
        }

        public Pausemenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        	if(isMaximize == true)
        	{
        		isMaximize = false;
        	}
        	else
        	{
        		isMaximize = true;
        	}
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        	if(isMinimize == false)
        	{
        		isMinimize = true;
        	}
        	else
        	{
        		isMinimize = false;
        	}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isShutdown = true;
        }
        
		void Pausemenu_Load(object sender, EventArgs e)
		{
			button1.Text = (isMaximize == true ? "Restore normal size" : "Maximize size");
			this.Focus();
		}
		void Pausemenu_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Dispose();
		}
    }
}
