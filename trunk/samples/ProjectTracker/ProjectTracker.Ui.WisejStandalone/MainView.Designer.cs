namespace Wisej.Application
{
    partial class MainView
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
      this.loaderPanel = new System.Windows.Forms.Panel();
      this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
      this.isLoadingMessage = new System.Windows.Forms.Label();
      this.loaderPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // loaderPanel
      // 
      this.loaderPanel.Controls.Add(this.pictureBoxLogo);
      this.loaderPanel.Controls.Add(this.isLoadingMessage);
      this.loaderPanel.Location = new System.Drawing.Point(263, 199);
      this.loaderPanel.Name = "loaderPanel";
      this.loaderPanel.Size = new System.Drawing.Size(500, 220);
      this.loaderPanel.TabIndex = 1;
      // 
      // pictureBoxLogo
      // 
      this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
      this.pictureBoxLogo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBoxLogo.ErrorImage = global::Wisej.Application.Properties.Resources.CSLA_NET128;
      this.pictureBoxLogo.Image = global::Wisej.Application.Properties.Resources.CSLA_NET128;
      this.pictureBoxLogo.InitialImage = global::Wisej.Application.Properties.Resources.CSLA_NET128;
      this.pictureBoxLogo.Location = new System.Drawing.Point(0, 80);
      this.pictureBoxLogo.Name = "pictureBoxLogo";
      this.pictureBoxLogo.Size = new System.Drawing.Size(500, 140);
      this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.pictureBoxLogo.TabIndex = 0;
      this.pictureBoxLogo.TabStop = false;
      // 
      // isLoadingMessage
      // 
      this.isLoadingMessage.Dock = System.Windows.Forms.DockStyle.Top;
      this.isLoadingMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.isLoadingMessage.Location = new System.Drawing.Point(0, 0);
      this.isLoadingMessage.Name = "isLoadingMessage";
      this.isLoadingMessage.Size = new System.Drawing.Size(500, 80);
      this.isLoadingMessage.TabIndex = 1;
      this.isLoadingMessage.Text = "Project Tracker is loading...";
      this.isLoadingMessage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.isLoadingMessage.UseWaitCursor = true;
      // 
      // MainView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(1010, 578);
      this.Controls.Add(this.loaderPanel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainView";
      this.Text = "Project Tracker";
      this.Resize += new System.EventHandler(this.MainView_Resize);
      this.loaderPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel loaderPanel;
        private System.Windows.Forms.Label isLoadingMessage;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
    }
}

