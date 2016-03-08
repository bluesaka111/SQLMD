namespace SQLMD
{
    partial class Pausemenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.panel2 = new System.Windows.Forms.Panel();
        	this.button3 = new System.Windows.Forms.Button();
        	this.button2 = new System.Windows.Forms.Button();
        	this.button1 = new System.Windows.Forms.Button();
        	this.panel1.SuspendLayout();
        	this.panel2.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// panel1
        	// 
        	this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
        	this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        	this.panel1.Controls.Add(this.panel2);
        	this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(168)))));
        	this.panel1.Location = new System.Drawing.Point(1, 1);
        	this.panel1.Margin = new System.Windows.Forms.Padding(0);
        	this.panel1.Name = "panel1";
        	this.panel1.Padding = new System.Windows.Forms.Padding(2);
        	this.panel1.Size = new System.Drawing.Size(318, 107);
        	this.panel1.TabIndex = 0;
        	// 
        	// panel2
        	// 
        	this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(48)))));
        	this.panel2.Controls.Add(this.button3);
        	this.panel2.Controls.Add(this.button2);
        	this.panel2.Controls.Add(this.button1);
        	this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.panel2.Location = new System.Drawing.Point(2, 2);
        	this.panel2.Margin = new System.Windows.Forms.Padding(2);
        	this.panel2.Name = "panel2";
        	this.panel2.Padding = new System.Windows.Forms.Padding(8);
        	this.panel2.Size = new System.Drawing.Size(312, 101);
        	this.panel2.TabIndex = 0;
        	// 
        	// button3
        	// 
        	this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
        	this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.button3.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button3.Location = new System.Drawing.Point(8, 70);
        	this.button3.Margin = new System.Windows.Forms.Padding(4);
        	this.button3.Name = "button3";
        	this.button3.Size = new System.Drawing.Size(296, 23);
        	this.button3.TabIndex = 3;
        	this.button3.Text = "Close entire program";
        	this.button3.UseVisualStyleBackColor = false;
        	this.button3.Click += new System.EventHandler(this.button3_Click);
        	// 
        	// button2
        	// 
        	this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
        	this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button2.Location = new System.Drawing.Point(8, 39);
        	this.button2.Margin = new System.Windows.Forms.Padding(4);
        	this.button2.Name = "button2";
        	this.button2.Size = new System.Drawing.Size(296, 23);
        	this.button2.TabIndex = 2;
        	this.button2.Text = "Minimize main window";
        	this.button2.UseVisualStyleBackColor = false;
        	this.button2.Click += new System.EventHandler(this.button2_Click);
        	// 
        	// button1
        	// 
        	this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
        	this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.button1.Dock = System.Windows.Forms.DockStyle.Top;
        	this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button1.Location = new System.Drawing.Point(8, 8);
        	this.button1.Margin = new System.Windows.Forms.Padding(4);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(296, 23);
        	this.button1.TabIndex = 1;
        	this.button1.Text = "Maximize main window";
        	this.button1.UseVisualStyleBackColor = false;
        	this.button1.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// Pausemenu
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(320, 109);
        	this.Controls.Add(this.panel1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        	this.Name = "Pausemenu";
        	this.Padding = new System.Windows.Forms.Padding(1);
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        	this.Text = "Pausemenu";
        	this.TopMost = true;
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Pausemenu_FormClosing);
        	this.Load += new System.EventHandler(this.Pausemenu_Load);
        	this.panel1.ResumeLayout(false);
        	this.panel2.ResumeLayout(false);
        	this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}