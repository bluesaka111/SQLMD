using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using System.Diagnostics;
using datastructure;

namespace SQLMD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Location = new Point(Screen.FromControl(this).WorkingArea.Location.X + 3, Screen.FromControl(this).WorkingArea.Location.Y + 3);
        }

        #region MovableForm Zone

        Point oldP;

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Current = Cursors.SizeAll;
                oldP = e.Location;
            }
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Current = Cursors.SizeAll;
                Point newP = e.Location;
                int x = newP.X - oldP.X;
                int y = newP.Y - oldP.Y;

                this.Location = new Point(this.Location.X + x, this.Location.Y + y);
            }
        }

        private void toolStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Current = Cursors.Default;
                oldP = e.Location;
            }
        }

        #endregion

        #region PlaceHolder
        EventLogger.MainForm cmanager = new EventLogger.MainForm();
        private List<DataTable> datatables = new List<DataTable>();
        private List<datastructure.CStructure> connections = new List<datastructure.CStructure>();
        Stack<ListViewItem> tempstack = new Stack<ListViewItem>();
        DataTable dt = new DataTable();
        bool isMax = false;
        bool isMin = false;
        private int total = 0;
        private bool navDisable = false;
        private bool CMShutdown = false;
        private int tempi = -1;
        private int value1 = -1;
        #endregion

        public bool shutdownCM{
        	get{ return CMShutdown;}
        	set{ CMShutdown = value;}
        }

        private void Clear()
        {
        	this.Dispose();
        }

	private int test = 0;

        private void GetDatabaseNames(int torder)
        {
            try
            {
                connections[torder].sqlcon.Open();
                DataTable dt2 = connections[torder].sqlcon.GetSchema("Databases");
                connections[torder].sqlcon.Close(); // Close Immediately to prevent performance loss from server side
                foreach(DataRow dr in dt2.Rows)
                {
                	toolStripComboBox1.Items.Add(dr[0].ToString());
                }
                if(connections[torder].selected_db != null || connections[torder].selected_db != "")
                {
                	toolStripComboBox1.SelectedItem = connections[torder].selected_db;
                }
            }
            catch(Exception ex)
            {
            	cmanager.AddaConnection("Error during download database scheme from SQL instance", ex.Message);
            	connections[torder].sqlcon.Close();
            	Dialog.MainForm.DShowDialog("Error during download database scheme from SQL instance", ex.Message);
            }
        }

        private void GetTablesName(int torder, string dbname)
        {
            try
            {
                int order = customTabControl1.SelectedIndex;
            	SqlConnectionStringBuilder temp = new SqlConnectionStringBuilder(connections[order].connstring);
            	temp.InitialCatalog = dbname;
                
                connections[torder].connstring = temp.ConnectionString;
            	connections[torder].sqlcon.ConnectionString = temp.ConnectionString;
                connections[torder].sqlcon.Open();
                DataTable dt2 = connections[torder].sqlcon.GetSchema("Tables");
                connections[torder].sqlcon.Close(); // Close Immediately to prevent performance loss from server side
                foreach(DataRow dr in dt2.Rows)
                {
                    toolStripComboBox2.Items.Add(dr[2].ToString());
                }
                if(connections[torder].selected_tb != null || connections[torder].selected_tb != "")
                {
                	toolStripComboBox2.SelectedItem = connections[torder].selected_tb;
                }
            }
            catch(Exception ex)
            {
            	cmanager.AddaConnection("Error during download table scheme from SQL instance", ex.Message);
            	connections[torder].sqlcon.Close();
            	Dialog.MainForm.DShowDialog("Error during download table scheme from SQL instance", ex.Message);
            }
        }

        private void getRowsRange(int torder, string dbname, string table)
        {
        	try
            {
                int order = customTabControl1.SelectedIndex;
        		toolStripComboBox3.Items.Clear();
        		toolStripComboBox3.Items.Add("All records");
            	SqlConnectionStringBuilder temp = new SqlConnectionStringBuilder(connections[order].connstring);
            	temp.InitialCatalog = dbname;
                connections[torder].connstring = temp.ConnectionString;
            	connections[torder].sqlcon.ConnectionString = temp.ConnectionString;
                connections[torder].sqlcon.Open();
            	SqlCommand cmm = new SqlCommand();
            	cmm.Connection = connections[torder].sqlcon;
            	cmm.CommandText = "SELECT COUNT(*) FROM " + table;
                cmm.CommandType = CommandType.Text;
            	int totalRows = -1;
            	totalRows = (int)cmm.ExecuteScalar();
                int tempvalue = totalRows;
            	connections[torder].sqlcon.Close();
            	#region Its a mess!!
                int i = 0;
            	while(true)
                {
                    if (tempvalue < perPage && tempvalue == 0)
                    {
                        toolStripComboBox3.Items.Add((i * perPage).ToString() + " TO " + totalRows.ToString());
                        break;
                    }
                    if(tempvalue < perPage && tempvalue != 0)
                    {
                        toolStripComboBox3.Items.Add((i * perPage + 1).ToString() + " TO " + totalRows.ToString());
                        break;
                    }
                    else
                    {
                        toolStripComboBox3.Items.Add((i * perPage + 1).ToString() + " TO " + ((i + 1) * perPage).ToString());
                        i++;
                        tempvalue = tempvalue - perPage;
                    }
                }
                if(connections[torder].selected_page != 0)
                {
                    if((toolStripComboBox3.Items.Count - 1) >= connections[torder].selected_page)
                    {
                        toolStripComboBox3.SelectedIndex = connections[torder].selected_page;
                    }
                    else
                    {
                        toolStripComboBox3.SelectedIndex = 0;
                    }
                }
                else
                {
                    toolStripComboBox3.SelectedIndex = 0;
                }
            	#endregion
            }
            catch(Exception ex)
            {
            	cmanager.AddaConnection("Error during download table scheme from SQL instance", ex.Message);
            	connections[torder].sqlcon.Close();
            	Dialog.MainForm.DShowDialog("Error during download table scheme from SQL instance", ex.Message);
            }
        }
        
        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connections.Count <= 3)
            {
                ConnectionSetup.Form1 csetup = new ConnectionSetup.Form1();
                csetup.runFromApp = true;
                if (csetup.ShowDialog() == DialogResult.OK)
                {
                    int order = customTabControl1.SelectedIndex;
                    #region clearPreviousSelectedDatabaseTable
                    toolStripComboBox1.Items.Clear();
                    toolStripComboBox2.Items.Clear();
                    toolStripComboBox3.Items.Clear();
                    #endregion
                    datastructure.CStructure temp = new datastructure.CStructure();
                    #region Generate
                    temp.pos = total;
                    temp.sqlcon = csetup.dstructure.sqlcon;
                    temp.connstring = csetup.dstructure.connstring;
                    temp.coldName = null;
                    temp.conName = "DBO ID." + (total + 1).ToString();
                    #endregion
                    connections.Add(temp);
                    TabPage tp = new TabPage();
                    tp.Text = "DBO ID." + (total + 1).ToString();
                    customTabControl1.TabPages.Insert(total, tp);
                    Panel pn = new Panel();
                    pn.BackColor = Color.FromArgb(47, 47, 38);
                    pn.Name = "r_p" + total.ToString();
                    pn.Dock = DockStyle.Fill;
                    ListView lv = new ListView();
                    lv.Dock = DockStyle.Fill;
                    lv.BackColor = Color.FromArgb(107, 107, 118);
                    lv.ForeColor = Color.FromArgb(192, 192, 168);
                    lv.View = View.Details;
                    lv.Name = "r_l" + total.ToString();
                    lv.FullRowSelect = true;
                    lv.BorderStyle = BorderStyle.None;
                    lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                    lv.GridLines = false;
                    lv.SelectedIndexChanged += lv_SelectedIndexChanged;
                    lv.HideSelection = false;
                    lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                    lv.MultiSelect = true;
                    lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    lv.StateImageList = imageList1;
                    lv.LargeImageList = imageList2;
                    lv.SmallImageList = imageList1;
                    lv.BringToFront();
                    pn.Controls.Add(lv);
                    tp.Controls.Add(pn);
                    GetDatabaseNames(total);
                    order = total;
                    customTabControl1.SelectedIndex = total;
                    total++;
                }
                else
                {
                    Dialog.MainForm.DShowDialog("Error", "User cancelled");
                }
            }
            else
            {
                Dialog.MainForm.DShowDialog("Too many SQL connection", "You cant create new sql connection. Please close old sql connection before you create a new one.");
            }
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            numericUpDown1.Value = (lv.SelectedItems.Count >= 1 ? (lv.SelectedItems[0].Index + 1) : numericUpDown1.Value);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pausemenu pm = new Pausemenu();
            pm.runFromApp = true;
            if(pm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(pm.isShutdown == true)
                {
                    this.Close();
                }
                if(pm.isMaximize == true)
                {
                	this.WindowState = FormWindowState.Maximized;
                	isMax = pm.isMaximize;
                	if(pm.isMinimize == true)
                	{
                		isMin = pm.isMinimize;
                		this.WindowState = FormWindowState.Minimized;
                	}
                }
                if(pm.isMaximize == false)
                {
                	isMax = pm.isMaximize;
                	this.WindowState = FormWindowState.Normal;
                	if(pm.isMinimize == true)
                	{
                		isMin = pm.isMinimize;
                		this.WindowState = FormWindowState.Minimized;
                	}
                }
            }
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox cb = (ToolStripComboBox) sender;
            int order = customTabControl1.SelectedIndex;
            if(toolStripComboBox2.SelectedItem != null || toolStripComboBox2.SelectedIndex != -1)
            {
                connections[order].selected_tb = toolStripComboBox2.SelectedItem.ToString();
                getRowsRange(order, toolStripComboBox1.SelectedItem.ToString(), toolStripComboBox2.SelectedItem.ToString());
            }
        }

		private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabControl tc = (TabControl)sender;
			int order = tc.SelectedIndex;
			connections[order].selrow = Convert.ToInt32(numericUpDown1.Value);
            toolStripComboBox1.Items.Clear();
            toolStripComboBox2.Items.Clear();
            toolStripComboBox3.Items.Clear();
            GetDatabaseNames(order);
            if (connections[order].selected_db != null && connections[order].selected_tb != null)
            {
                if (Dialog.MainForm.DShowDialog(MessageBoxButtons.YesNo, "", "Do you want to update your records from SQL instance?") == System.Windows.Forms.DialogResult.Yes)
                {
                    reload_DataTable(toolStripButton1);
                }
            }
            if((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Items.Count == 0)
            {
                label3.Text = "of 0 records";
                numericUpDown1.Maximum = 0;
                numericUpDown1.Minimum = 0;
                numericUpDown1.Value = 0;
            }
            else
            {
                numericUpDown1.Maximum = connections[order].rows;
                label3.Text = "of " + connections[order].rows.ToString() + " records";
                numericUpDown1.Value = 1;
                numericUpDown1.Increment = 1;
                numericUpDown1.Minimum = 1;
            }
		}

        private static void autoResizeColumns(ListView lv)
        {
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListView.ColumnHeaderCollection cc = lv.Columns;
            for (int i = 0; i < cc.Count; i++)
            {
                int colWidth = TextRenderer.MeasureText(cc[i].Text, lv.Font).Width + 40;
                if (colWidth > cc[i].Width)
                {
                    cc[i].Width = colWidth;
                }
                else
                {
                    cc[i].Width += 40;
                }
            }
        }

        public void reload_DataTable(ToolStripButton bt)
        {
            currGroup = -1;
            int order = customTabControl1.SelectedIndex;
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Items.Clear();
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Columns.Clear();
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Groups.Clear();
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).GridLines = false;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connections[order].connstring);
            builder.InitialCatalog = toolStripComboBox1.SelectedItem.ToString();
            SqlCommand cm = new SqlCommand();
            cm.CommandType = CommandType.Text;
            cm.CommandText = "SELECT * FROM " + toolStripComboBox2.SelectedItem.ToString();
            cm.Connection = connections[order].sqlcon;
            try
            {
                dt.Reset();
                connections[order].sqlcon.Open();
                SqlDataAdapter data = new SqlDataAdapter();
                data.MissingMappingAction = MissingMappingAction.Passthrough;

                data.SelectCommand = cm;
                data.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                if (toolStripComboBox3.SelectedIndex == 0)
                {
                    tempi = 0;
                    data.Fill(dt);
                }
                else
                {
                    string[] spliters = { " TO " };
                    string[] datas = toolStripComboBox3.SelectedItem.ToString().Split(spliters, StringSplitOptions.None);
                    value1 = Convert.ToInt32(datas[0]) - 1;
                    tempi = value1;
                    data.Fill(value1, perPage, dt);
                }
                connections[order].rows = dt.Rows.Count;
                connections[order].sqlcon.Close();
                //Safety disable
                bt.Enabled = false;
                toolStripComboBox1.Enabled = false;
                toolStripComboBox2.Enabled = false;
                toolStripButton7.Enabled = true;
                customTabControl1.Enabled = false;
                //
                backgroundWorker1.RunWorkerAsync((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView));
            }
            catch (Exception ex)
            {
                cmanager.AddaConnection("Error during download database records from SQL instance", ex.Message);
                connections[order].sqlcon.Close();
                Dialog.MainForm.DShowDialog("Error during download database records from SQL instance", ex.Message);
                return;
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox cb = (ToolStripComboBox)sender;
            toolStripComboBox2.Items.Clear();
            toolStripComboBox3.Items.Clear();
            int order = customTabControl1.SelectedIndex;
            GetTablesName(order, toolStripComboBox1.SelectedItem.ToString());
            if(toolStripComboBox1.SelectedIndex != -1 || toolStripComboBox1.SelectedItem != null)
            {
                connections[order].selected_db = toolStripComboBox1.SelectedItem.ToString();
            }
            connections[order].selected_page = 0;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ToolStripButton bt = (ToolStripButton)sender;
            if (!backgroundWorker1.IsBusy && !backgroundWorker2.IsBusy)
            {
                if(connections.Count == 0)
                {
                    Dialog.MainForm.DShowDialog("Error", "No connection available.\nPlease create one before try this again");
                    return;
                }
            	try
            	{
                	reload_DataTable(bt);
                    if (dt.Rows.Count >= 1)
                    {
                        int order = customTabControl1.SelectedIndex;
                        (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).GridLines = true;
                    }
            	}
            	catch(Exception ex)
            	{
            		cmanager.AddaConnection("Unable to load database records and display due to error", ex.Message);
            		Dialog.MainForm.DShowDialog("Unable to load database records and display due to error",ex.Message);
            		if(backgroundWorker1.IsBusy)
            		{
            			backgroundWorker1.CancelAsync();
            		}
            		if(backgroundWorker2.IsBusy)
            		{
            			backgroundWorker2.CancelAsync();
            		}
                    return;
            	}
            }
        }

        bool byUser = false;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if(backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                byUser = true;
                return;
            }
            ListView lv = (e.Argument as ListView);
            if (dt.Columns.Count == 0)
            {
            	e.Cancel = true;
            	byUser = false;
            	return;
            }
            DataColumn[] prims = dt.PrimaryKey;
            int i = 0;
            datastructure.UselessColumnHeaderStructure temp = new UselessColumnHeaderStructure();
            ColumnHeader chg = new ColumnHeader();
            chg.Text = "Record Index";
            chg.ImageIndex = -1;
            chg.TextAlign = HorizontalAlignment.Left;
            backgroundWorker1.ReportProgress(0,chg);
            foreach (DataColumn dc in dt.Columns)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                temp.ch = new ColumnHeader();
                temp.ch.ImageIndex = -1;
                temp.ch.Text = (dc.Unique ? "[U]" : null) + (dc.ReadOnly ? "[R]" : null) + dc.ColumnName;
                temp.ch.Tag = (dc.Unique ? "isUnique" : null) + (dc.ReadOnly ? "isReadOnly" : null);
                //temp.ch.Width = -2;
                foreach (DataColumn dc2 in prims)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    if(prims.Length == 0)
                    {
                        break;
                    }
                    if (dc.ColumnName == dc2.ColumnName)
                    {
                        temp.ch.ImageIndex = 0;
                        temp.ch.Text = (dc.Unique ? "[U]" : null) + (dc.ReadOnly ? "[R]" : null) + dc.ColumnName;
                        temp.ch.Tag = temp.ch.Tag.ToString() + "isPrimaryKey";
                    }
                }
                temp.index = i;
                i++;
                backgroundWorker1.ReportProgress(0, temp.ch);
                Thread.Sleep(20);
            }
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        	int order = customTabControl1.SelectedIndex;
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Columns.Add(e.UserState as ColumnHeader);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        	int order = customTabControl1.SelectedIndex;
            backgroundWorker2.RunWorkerAsync((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView));
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if(backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                byUser = true;
                return;
            }
            if(backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
                byUser = true;
                return;
            }
            else
            {
            	Dialog.MainForm.DShowDialog("","The process already terminated");
                return;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ListView lv = (e.Argument as ListView);
            if(backgroundWorker2.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            if(dt.Rows.Count == 0)
            {
            	e.Cancel = true;
            	byUser = false;
            	return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (backgroundWorker2.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                ListViewItem item = new ListViewItem();
                item.UseItemStyleForSubItems = false;
                item.Text = tempi.ToString();
                item.BackColor = Color.FromArgb(37, 37, 38);
                item.ForeColor = Color.FromArgb(192, 192, 168);
                int nulls = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (backgroundWorker2.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    ListViewItem.ListViewSubItem sub = new ListViewItem.ListViewSubItem();
                    if(dr[i] == DBNull.Value)
                    {
                        nulls++;
                        sub.BackColor = Color.White;
                        sub.ForeColor = Color.Black;
                    }
                    sub.Text = (dr[i] == DBNull.Value ? "<<NULL>>" : dr[i].ToString());
                    item.SubItems.Add(sub);
                }
                if (nulls == dt.Columns.Count)
                {
                    item.UseItemStyleForSubItems = true;
                    item.BackColor = Color.FromArgb(27, 27, 28);
                    item.ForeColor = Color.FromArgb(192, 192, 168);
                    MessageBox.Show(nulls.ToString());
                    //continue;
                }
                backgroundWorker2.ReportProgress(0, item);
                tempi++;
                Thread.Sleep(5);
            }
            if (backgroundWorker2.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }
        int perPage = 200;
        int currGroup = -1;
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        	int order = customTabControl1.SelectedIndex;
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).BeginUpdate();
            if(toolStripComboBox3.SelectedIndex == 0)
            {
                if ((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Items.Count % perPage == 0)
                {
                    currGroup++;
                    ListViewGroup lvG = new ListViewGroup();
                    lvG.Header = "Page " + (currGroup + 1).ToString();
                    lvG.HeaderAlignment = HorizontalAlignment.Right;
                    lvG.Name = "p" + currGroup.ToString();
                    (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Groups.Insert(currGroup, lvG);
                }
            }
            else
            {
                if ((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Groups.Count == 0)
                {
                    ListViewGroup lvG = new ListViewGroup();
                    lvG.Header = "Page " + (toolStripComboBox3.SelectedIndex).ToString();
                    lvG.HeaderAlignment = HorizontalAlignment.Right;
                    lvG.Name = "p" + (toolStripComboBox3.SelectedIndex).ToString();
                    (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Groups.Insert(0, lvG);
                }
            }
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Items.Add(e.UserState as ListViewItem);
            if (toolStripComboBox3.SelectedIndex == 0)
            {
                (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Groups[currGroup].Items.Add(e.UserState as ListViewItem);
            }
            else
            {
                (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Groups[0].Items.Add(e.UserState as ListViewItem);
            }
            (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).EndUpdate();
            label3.Text = "of " + connections[order].rows.ToString() + " records";
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripButton6.Enabled = true;
            toolStripComboBox1.Enabled = true;
            toolStripComboBox2.Enabled = true;
            toolStripButton7.Enabled = false;
            customTabControl1.Enabled = true;
            int order = customTabControl1.SelectedIndex;
            if (e.Cancelled && !byUser && e.Error != null)
            {
                Dialog.MainForm.DShow("","Process terminated due to errors");
            }
            byUser = false;
            if(dt.Rows.Count >= 1)
            {
                numericUpDown1.Maximum = connections[order].rows;
                label3.Text = "of " + connections[order].rows.ToString() + " records";
                numericUpDown1.Value = 1;
            	numericUpDown1.Increment = 1;
                numericUpDown1.Minimum = 1;
            }
            else
            {
            	numericUpDown1.Maximum = 0;
            	numericUpDown1.Value = 0;
            	numericUpDown1.Increment = 0;
            	numericUpDown1.Minimum = 0;
                label3.Text = "of " + connections[order].rows.ToString() + " records";
            }
            autoResizeColumns((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(connections.Count == 0 && customTabControl1.TabPages.Count == 0)
            {
                navDisable = true;
            }
            else
            {
            	if(customTabControl1.TabPages.Count >= 1)
            	{
            		int order = customTabControl1.SelectedIndex;
	                if ((customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Items.Count >= 1)
	                {
	                    navDisable = false;
	                }
            	}
            }
        }

		void Form1_SizeChanged(object sender, EventArgs e)
		{
			if(this.WindowState != FormWindowState.Minimized)
			{
				if(isMax)
				{
					this.WindowState = FormWindowState.Maximized;
				}
				if(!isMax)
				{
					this.WindowState = FormWindowState.Normal;
				}
			}
		}
		
		void Form1_Load(object sender, EventArgs e)
		{
			cmanager.runFromApp = true;
			cmanager.Show();
			numericUpDown1.Maximum = 0;
            numericUpDown1.Value = 0;
            numericUpDown1.Increment = 0;
            numericUpDown1.Minimum = 0;
		}
		
		void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			connections.Clear();
			this.Dispose();
		}
		
		void ViewHelpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help_Screen.MainForm help = new Help_Screen.MainForm();
			help.runFromApp = true;
			help.Show();
		}
		void Utilities_bt_Click(object sender, EventArgs e)
		{
			Point screenPoint = editDBRecords_bt.PointToScreen(new Point(utilitiesDBRecords_bt.Left, utilitiesDBRecords_bt.Bottom));
			if (screenPoint.Y + contextMenuStrip1.Size.Height > Screen.PrimaryScreen.WorkingArea.Height) 
			{
			    contextMenuStrip1.Show(utilitiesDBRecords_bt, new Point(0, -contextMenuStrip1.Size.Height));
			} 
			else
			{
			    contextMenuStrip1.Show(utilitiesDBRecords_bt, new Point(0, utilitiesDBRecords_bt.Height));
  			}    
		}
		
		void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			contextMenuStrip1.Height = contextMenuStrip1.Items.Count * 22 + 4;
		}
		
		void NumericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			int order = customTabControl1.SelectedIndex;
            foreach (ListViewItem item in (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).SelectedItems)
            {
                item.Selected = false;
            }
            if (Convert.ToInt32(numericUpDown1.Value) - 1 != -1)
            {
                (customTabControl1.TabPages[order].Controls["r_p" + order.ToString()].Controls["r_l" + order.ToString()] as ListView).Items[Convert.ToInt32(numericUpDown1.Value) - 1].Selected = true;
                connections[order].selrow = Convert.ToInt32(numericUpDown1.Value);
            }
		}
		
		void InsertDBRecords_btEnter(object sender, EventArgs e)
		{
			Button bt = (Button)sender;
			
		}

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int order = customTabControl1.SelectedIndex;
            if(toolStripComboBox3.SelectedIndex != -1)
            {
                connections[order].selected_page = toolStripComboBox3.SelectedIndex;
            }
        }

        private void label3_TextChanged(object sender, EventArgs e)
        {
            label3.Width = TextRenderer.MeasureText(label3.Text, label3.Font).Width + 5;
        }
    }
}
