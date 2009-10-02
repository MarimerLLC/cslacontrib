namespace MyCslaSample
{
  partial class StatusBarExtenderDemo
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusBarExtenderDemo));
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.statusStripExtender1 = new MyCsla.Windows.StatusStripExtender();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // statusStrip1
      // 
      this.statusStripExtender1.SetAnimationLabel(this.statusStrip1, this.toolStripStatusLabel2);
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
      this.statusStrip1.Location = new System.Drawing.Point(0, 323);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.ShowItemToolTips = true;
      this.statusStrip1.Size = new System.Drawing.Size(504, 22);
      this.statusStripExtender1.SetStatusLabel(this.statusStrip1, this.toolStripStatusLabel1);
      this.statusStrip1.TabIndex = 0;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // statusStripExtender1
      // 
      this.statusStripExtender1.AnimationControl = this.toolStripStatusLabel2;
      this.statusStripExtender1.StatusControl = this.toolStripStatusLabel1;
      this.statusStripExtender1.StatusDefault = "Ready";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(444, 17);
      this.toolStripStatusLabel1.Spring = true;
      this.toolStripStatusLabel1.Text = "Ready";
      this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripStatusLabel2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel2.Image")));
      this.toolStripStatusLabel2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(45, 17);
      this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(456, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Start BackgroundWorker task, runs for 5 seconds, display new message";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(12, 41);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(456, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Set message (shown for 5 seconds then back to default message)";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(12, 70);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(456, 23);
      this.button3.TabIndex = 3;
      this.button3.Text = "Set static message (will not be reset)";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(12, 99);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new System.Drawing.Size(456, 210);
      this.textBox1.TabIndex = 4;
      this.textBox1.Text = resources.GetString("textBox1.Text");
      // 
      // StatusBarExtenderDemo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(504, 345);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.statusStrip1);
      this.Name = "StatusBarExtenderDemo";
      this.Text = "StatusBarDemo";
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip statusStrip1;
    private MyCsla.Windows.StatusStripExtender statusStripExtender1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.TextBox textBox1;
  }
}