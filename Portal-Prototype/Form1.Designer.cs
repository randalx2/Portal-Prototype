namespace Portal_Prototype
{
    partial class Form1
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
            this.lblListOfApps = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblListOfApps
            // 
            this.lblListOfApps.AutoSize = true;
            this.lblListOfApps.Location = new System.Drawing.Point(12, 9);
            this.lblListOfApps.Name = "lblListOfApps";
            this.lblListOfApps.Size = new System.Drawing.Size(137, 13);
            this.lblListOfApps.TabIndex = 0;
            this.lblListOfApps.Text = "Currently Open Applications";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 34);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(864, 238);
            this.listBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(383, 394);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 43);
            this.button1.TabIndex = 2;
            this.button1.Text = "SAVE APPS";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(726, 394);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 43);
            this.button2.TabIndex = 3;
            this.button2.Text = "RESTORE APPS";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 394);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(154, 43);
            this.button3.TabIndex = 4;
            this.button3.Text = "SHOW OPEN APPS";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(15, 327);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(152, 42);
            this.btnFilePath.TabIndex = 5;
            this.btnFilePath.Text = "Choose Save Directory";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(186, 342);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(102, 13);
            this.lblFilePath.TabIndex = 6;
            this.lblFilePath.Text = "Selected Save Path";
            // 
            // txtFilePath
            // 
            this.txtFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Portal_Prototype.Properties.Settings.Default, "holdpath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFilePath.Location = new System.Drawing.Point(309, 335);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(551, 20);
            this.txtFilePath.TabIndex = 7;
            this.txtFilePath.Text = global::Portal_Prototype.Properties.Settings.Default.holdpath;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 485);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.btnFilePath);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lblListOfApps);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblListOfApps;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnFilePath;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox txtFilePath;
    }
}

