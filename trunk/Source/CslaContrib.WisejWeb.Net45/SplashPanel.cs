using System;
using CslaContrib.WisejWeb.Properties;
using Wisej.Web;

namespace CslaContrib.WisejWeb
{
  /// <summary>
  /// Layout of the Splash Panel
  /// </summary>
  public enum SplashPanelLayout
  {
    /// <summary>
    /// Green horizontal bar
    /// </summary>
    Bar,

    /// <summary>
    /// Black and gray circle
    /// </summary>
    Circle
  }

  /// <summary>
  /// Use this control to give users feedback under heavy workload.
  /// </summary>
  public static class SplashPanel
  {
    private static Form _form;
    private static PictureBox _splashAnimation;
    private static TableLayoutPanel _splashInnerTable;
    private static Panel _splashOuterPanel;
    private static TableLayoutPanel _splashOuterTable;
    private static Label _splashStatusLabel;

    /// <summary>
    /// Shows the splash panel on the form.
    /// </summary>
    /// <param name="form">The form.</param>
    /// <remarks>
    /// Default text = "Please wait..." / "Vennligst vent..."
    /// Default SplashPanelLayout = Circle
    /// </remarks> 
    public static void Show(Form form)
    {
      Show(form, Resources.SplashPanel_PleaseWait);
    }

    /// <summary>
    /// Shows the splash panel on the form.
    /// </summary>
    /// <param name="form">The form.</param>
    /// <param name="text">The text.</param>
    /// <remarks>
    /// Default SplashPanelLayout = Circle
    /// </remarks> 
    public static void Show(Form form, string text)
    {
      Show(form, text, SplashPanelLayout.Circle);
    }

    /// <summary>
    /// Shows the splash panel on the form.
    /// </summary>
    /// <param name="form">The form.</param>
    /// <param name="text">The text.</param>
    /// <param name="splashPanelLayout">The splash panel layout.</param>
    public static void Show(Form form, string text, SplashPanelLayout splashPanelLayout)
    {
      if (_form != null)
      {
        return;
      }
      _form = form;
      if (String.IsNullOrEmpty(text))
      {
        text = Properties.Resources.SplashPanel_PleaseWait;
      }
      if (splashPanelLayout == SplashPanelLayout.Circle)
      {
        ShowPanelCircle(text);
      }
      else
      {
        ShowPanelBar(text);
      }
    }

    private static void ShowPanelBar(string text)
    {
      if (_form == null)
      {
        return;
      }
      _form.UseWaitCursor = true;

      _splashOuterPanel = new Panel();
      _splashOuterTable = new TableLayoutPanel();
      _splashInnerTable = new TableLayoutPanel();
      _splashStatusLabel = new Label();
      _splashAnimation = new PictureBox();

      _splashOuterPanel.SuspendLayout();
      _splashOuterTable.SuspendLayout();
      _splashInnerTable.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (_splashAnimation)).BeginInit();
      _form.SuspendLayout();

      var splashX = (_form.ClientRectangle.Width/2) - (262/2);
      var splashY = (_form.ClientRectangle.Height/2) - (117/2);

      // 
      // splashOuterPanel
      // 
      //_splashOuterPanel.BorderStyle = BorderStyle.FixedSingle;
      _splashOuterPanel.Controls.Add(_splashOuterTable);
      _splashOuterPanel.Location = new System.Drawing.Point(splashX, splashY);
      _splashOuterPanel.Name = "splashOuterPanel";
      _splashOuterPanel.Size = new System.Drawing.Size(262, 117);
      _splashOuterPanel.UseWaitCursor = true;

      // 
      // splashOuterTable
      // 
      //_splashOuterTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
      _splashOuterTable.ColumnCount = 1;
      _splashOuterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      _splashOuterTable.Controls.Add(_splashInnerTable, 0, 0);
      _splashOuterTable.Dock = DockStyle.Fill;
      _splashOuterTable.Location = new System.Drawing.Point(0, 0);
      _splashOuterTable.Name = "splashOuterTable";
      _splashOuterTable.RowCount = 1;
      _splashOuterTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      _splashOuterTable.Size = new System.Drawing.Size(260, 115);
      _splashOuterTable.UseWaitCursor = true;

      // 
      // splashInnerTable
      // 
      _splashInnerTable.AutoSize = true;
      _splashInnerTable.BackgroundImage = Properties.Resources.AboutBack;
      _splashInnerTable.BackgroundImageLayout = ImageLayout.Stretch;
      _splashInnerTable.ColumnCount = 1;
      _splashInnerTable.ColumnStyles.Add(new ColumnStyle());
      _splashInnerTable.Controls.Add(_splashStatusLabel, 0, 0);
      _splashInnerTable.Controls.Add(_splashAnimation, 0, 1);
      _splashInnerTable.Dock = DockStyle.Fill;
      _splashInnerTable.ForeColor = System.Drawing.SystemColors.ControlText;
      _splashInnerTable.Location = new System.Drawing.Point(1, 1);
      _splashInnerTable.Margin = new Padding(0);
      _splashInnerTable.Name = "splashInnerTable";
      _splashInnerTable.RowCount = 2;
      _splashInnerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
      _splashInnerTable.RowStyles.Add(new RowStyle());
      _splashInnerTable.Size = new System.Drawing.Size(258, 113);
      _splashInnerTable.UseWaitCursor = true;

      // 
      // splashStatusLabel
      // 
      _splashStatusLabel.BackColor = System.Drawing.Color.Transparent;
      _splashStatusLabel.Dock = DockStyle.Fill;
      _splashStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      //_splashStatusLabel.ImeMode = ImeMode.NoControl;
      _splashStatusLabel.Location = new System.Drawing.Point(20, 0);
      _splashStatusLabel.Margin = new Padding(20, 0, 40, 0);
      _splashStatusLabel.Name = "splashStatusLabel";
      _splashStatusLabel.Size = new System.Drawing.Size(210, 60);
      if (text.Length > 0)
      {
        _splashStatusLabel.Text = text;
      }
      _splashStatusLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      _splashStatusLabel.UseWaitCursor = true;

      // 
      // splashAnimation
      // 
      _splashAnimation.Image = Properties.Resources.progressbar_green;
      //_splashAnimation.ImeMode = ImeMode.NoControl;
      _splashAnimation.Location = new System.Drawing.Point(20, 70);
      _splashAnimation.Margin = new Padding(20, 10, 0, 0);
      _splashAnimation.Name = "splashAnimation";
      _splashAnimation.Size = new System.Drawing.Size(214, 15);
      _splashAnimation.TabStop = false;
      _splashAnimation.UseWaitCursor = true;

      // 
      // Add to form
      // 
      _form.Controls.Add(_splashOuterPanel);
      _splashOuterPanel.ResumeLayout(false);
      _splashOuterTable.ResumeLayout(false);
      _splashOuterTable.PerformLayout();
      _splashInnerTable.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize) (_splashAnimation)).EndInit();
      _form.ResumeLayout(false);

      _splashOuterPanel.BringToFront();
    }

    private static void ShowPanelCircle(string text)
    {
      if (_form == null)
      {
        return;
      }
      _form.UseWaitCursor = true;

      _splashOuterPanel = new Panel();
      _splashOuterTable = new TableLayoutPanel();
      _splashInnerTable = new TableLayoutPanel();
      _splashStatusLabel = new Label();
      _splashAnimation = new PictureBox();

      _splashOuterPanel.SuspendLayout();
      _splashOuterTable.SuspendLayout();
      _splashInnerTable.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (_splashAnimation)).BeginInit();
      _form.SuspendLayout();

      var splashX = (_form.ClientRectangle.Width/2) - (262/2);
      var splashY = (_form.ClientRectangle.Height/2) - (80/2);
      // 
      // splashOuterPanel
      //             
      //_splashOuterPanel.BorderStyle = BorderStyle.FixedSingle;
      _splashOuterPanel.Controls.Add(_splashOuterTable);
      _splashOuterPanel.Location = new System.Drawing.Point(splashX, splashY);
      _splashOuterPanel.Name = "splashOuterPanel";
      _splashOuterPanel.Size = new System.Drawing.Size(262, 80);
      _splashOuterPanel.UseWaitCursor = true;
      // 
      // splashOuterTable
      // 
      //_splashOuterTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
      _splashOuterTable.ColumnCount = 1;
      _splashOuterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      _splashOuterTable.Controls.Add(_splashInnerTable, 0, 0);
      _splashOuterTable.Dock = DockStyle.Fill;
      _splashOuterTable.Location = new System.Drawing.Point(0, 0);
      _splashOuterTable.Name = "splashOuterTable";
      _splashOuterTable.RowCount = 1;
      _splashOuterTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      _splashOuterTable.Size = new System.Drawing.Size(260, 78);
      _splashOuterTable.UseWaitCursor = true;

      // 
      // splashInnerTable
      // 
      _splashInnerTable.AutoSize = true;
      _splashInnerTable.BackgroundImage = Properties.Resources.AboutBack;
      _splashInnerTable.BackgroundImageLayout = ImageLayout.Stretch;
      _splashInnerTable.ColumnCount = 2;
      _splashInnerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
      _splashInnerTable.ColumnStyles.Add(new ColumnStyle());
      _splashInnerTable.Controls.Add(_splashStatusLabel, 1, 0);
      _splashInnerTable.Controls.Add(_splashAnimation, 0, 0);
      _splashInnerTable.Dock = DockStyle.Fill;
      _splashInnerTable.ForeColor = System.Drawing.SystemColors.ControlText;
      _splashInnerTable.Location = new System.Drawing.Point(1, 1);
      _splashInnerTable.Margin = new Padding(0);
      _splashInnerTable.Name = "splashInnerTable";
      _splashInnerTable.RowCount = 1;
      _splashInnerTable.RowStyles.Add(new RowStyle());
      _splashInnerTable.Size = new System.Drawing.Size(258, 76);
      _splashInnerTable.UseWaitCursor = true;

      // 
      // splashStatusLabel
      // 
      _splashStatusLabel.BackColor = System.Drawing.Color.Transparent;
      _splashStatusLabel.Dock = DockStyle.Fill;
      _splashStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      //_splashStatusLabel.ImeMode = ImeMode.NoControl;
      _splashStatusLabel.Location = new System.Drawing.Point(70, 0);
      _splashStatusLabel.Margin = new Padding(10, 0, 10, 0);
      _splashStatusLabel.Name = "splashStatusLabel";
      _splashStatusLabel.Size = new System.Drawing.Size(178, 76);
      if (text.Length > 0)
      {
        _splashStatusLabel.Text = text;
      }
      _splashStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      _splashStatusLabel.UseWaitCursor = true;
      // 
      // splashAnimation
      // 
      _splashAnimation.BackColor = System.Drawing.Color.Transparent;
      _splashAnimation.Image = Properties.Resources.progresscircle_black;
      //_splashAnimation.ImeMode = ImeMode.NoControl;
      _splashAnimation.Location = new System.Drawing.Point(20, 23);
      _splashAnimation.Margin = new Padding(20, 23, 0, 0);
      _splashAnimation.Name = "splashAnimation";
      _splashAnimation.Size = new System.Drawing.Size(31, 31);
      _splashAnimation.TabStop = false;
      _splashAnimation.UseWaitCursor = true;

      // 
      // Add to form
      //             
      _form.Controls.Add(_splashOuterPanel);
      _splashOuterPanel.ResumeLayout(false);
      _splashOuterTable.ResumeLayout(false);
      _splashOuterTable.PerformLayout();
      _splashInnerTable.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize) (_splashAnimation)).EndInit();
      _form.ResumeLayout(false);

      _splashOuterPanel.BringToFront();
    }

    /// <summary>
    /// Sets the status.
    /// </summary>
    /// <param name="text">The text.</param>
    public static void SetStatus(string text)
    {
      if (_form == null || _splashStatusLabel == null)
      {
        return;
      }
      _splashStatusLabel.Text = text;
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public static void Close()
    {
      if (_form == null)
      {
        return;
      }
      if (_splashOuterPanel != null)
      {
        if (_form.Controls.Contains(_splashOuterPanel))
        {
          _form.Controls.Remove(_splashOuterPanel);
          _splashOuterPanel.Dispose();
          _form.UseWaitCursor = false;
        }
      }
      _splashOuterPanel = null;
      _splashOuterTable = null;
      _splashInnerTable = null;
      _splashStatusLabel = null;
      _splashAnimation = null;

      _form = null;
    }
  } // static class SplashPanel
} // namespace CslaContrib.WisejWeb.Forms