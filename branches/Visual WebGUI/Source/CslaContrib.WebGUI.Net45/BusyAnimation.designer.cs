namespace CslaContrib.WebGUI
{
  partial class BusyAnimation
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.BusyProgressBar = new Gizmox.WebGUI.Forms.ProgressBar();
      this.ProgressTimer = new Gizmox.WebGUI.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // BusyProgressBar
      // 
      this.BusyProgressBar.Dock = Gizmox.WebGUI.Forms.DockStyle.Fill;
      this.BusyProgressBar.ForeColor = System.Drawing.Color.LawnGreen;
      this.BusyProgressBar.Location = new System.Drawing.Point(0, 0);
      this.BusyProgressBar.Maximum = 30;
      this.BusyProgressBar.Name = "BusyProgressBar";
      this.BusyProgressBar.Size = new System.Drawing.Size(100, 23);
      this.BusyProgressBar.Step = 1;
      this.BusyProgressBar.Style = Gizmox.WebGUI.Forms.ProgressBarStyle.Continuous;
      this.BusyProgressBar.TabIndex = 0;
      this.BusyProgressBar.Visible = false;
      // 
      // ProgressTimer
      // 
      this.ProgressTimer.Interval = 1;
      this.ProgressTimer.Tick += new System.EventHandler(this.ProgressTimer_Tick);
      // 
      // BusyAnimation
      // 
      //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      //this.AutoScaleMode = Gizmox.WebGUI.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.BusyProgressBar);
      this.Name = "BusyAnimation";
      this.Size = new System.Drawing.Size(100, 23);
      this.Load += new System.EventHandler(this.BusyAnimation_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private Gizmox.WebGUI.Forms.ProgressBar BusyProgressBar;
    private Gizmox.WebGUI.Forms.Timer ProgressTimer;
  }
}
