using Wisej.Web;

namespace PTWisej
{
  partial class MainPage
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

    #region Wisej Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
      this.toolStrip1 = new Wisej.Web.ToolBar();
      this.ProjectsStripDropDownButton1 = new Wisej.Web.ToolBarButton();
      this.ResourcesToolStripDropDownButton = new Wisej.Web.ToolBarButton();
      this.AdminToolStripDropDownButton = new Wisej.Web.ToolBarButton();
      this.LoginToolStripLabel = new Wisej.Web.ToolBarButton();
      this.LoginToolStripButton = new Wisej.Web.ToolBarButton();
      this.DocumentsToolStripDropDownButton = new Wisej.Web.ToolBarButton();
      this.NewProjectToolStripMenuItem = new Wisej.Web.MenuItem();
      this.EditProjectToolStripMenuItem = new Wisej.Web.MenuItem();
      this.DeleteProjectToolStripMenuItem = new Wisej.Web.MenuItem();
      this.NewResourceToolStripMenuItem = new Wisej.Web.MenuItem();
      this.EditResourceToolStripMenuItem = new Wisej.Web.MenuItem();
      this.DeleteResourceToolStripMenuItem = new Wisej.Web.MenuItem();
      this.EditRolesToolStripMenuItem = new Wisej.Web.MenuItem();
      this.Panel1 = new Wisej.Web.Panel();
      this.StatusStrip1 = new Wisej.Web.StatusBar();
      this.StatusLabel = new Wisej.Web.StatusBarPanel();
      this.projectsMenu = new Wisej.Web.ContextMenu();
      this.resourcesMenu = new Wisej.Web.ContextMenu();
      this.adminMenu = new Wisej.Web.ContextMenu();
      this.documentsMenu = new Wisej.Web.ContextMenu();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Buttons.AddRange(new Wisej.Web.ToolBarButton[] {
            this.ProjectsStripDropDownButton1,
            this.ResourcesToolStripDropDownButton,
            this.AdminToolStripDropDownButton,
            this.LoginToolStripLabel,
            this.LoginToolStripButton,
            this.DocumentsToolStripDropDownButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(948, 26);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.TabStop = false;
      this.toolStrip1.TextAlign = Wisej.Web.ToolBarTextAlign.Right;
      // 
      // ProjectsStripDropDownButton1
      // 
      this.ProjectsStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("ProjectsStripDropDownButton1.Image")));
      this.ProjectsStripDropDownButton1.Name = "ProjectsStripDropDownButton1";
      this.ProjectsStripDropDownButton1.Text = "Projects";
      this.ProjectsStripDropDownButton1.DropDownMenu = this.projectsMenu;
      this.ProjectsStripDropDownButton1.Style = ToolBarButtonStyle.DropDownButton;
      // 
      // ResourcesToolStripDropDownButton
      // 
      this.ResourcesToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("ResourcesToolStripDropDownButton.Image")));
      this.ResourcesToolStripDropDownButton.Name = "ResourcesToolStripDropDownButton";
      this.ResourcesToolStripDropDownButton.Text = "Resources";
      this.ResourcesToolStripDropDownButton.DropDownMenu = this.resourcesMenu;
      this.ResourcesToolStripDropDownButton.Style = ToolBarButtonStyle.DropDownButton;
      // 
      // AdminToolStripDropDownButton
      // 
      this.AdminToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("AdminToolStripDropDownButton.Image")));
      this.AdminToolStripDropDownButton.Name = "AdminToolStripDropDownButton";
      this.AdminToolStripDropDownButton.Text = "Admin";
      this.AdminToolStripDropDownButton.DropDownMenu = this.adminMenu;
      this.AdminToolStripDropDownButton.Style = ToolBarButtonStyle.DropDownButton;
      // 
      // LoginToolStripLabel
      // 
      this.LoginToolStripLabel.Name = "LoginToolStripLabel";
      this.LoginToolStripLabel.Text = "Not logged in";
      // 
      // LoginToolStripButton
      // 
      this.LoginToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("LoginToolStripButton.Image")));
      this.LoginToolStripButton.Name = "LoginToolStripButton";
      this.LoginToolStripButton.Text = "Login";
      this.LoginToolStripButton.Click += new System.EventHandler(this.LoginToolStripButton_Click);
      // 
      // DocumentsToolStripDropDownButton
      // 
      this.DocumentsToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("DocumentsToolStripDropDownButton.Image")));
      this.DocumentsToolStripDropDownButton.Name = "DocumentsToolStripDropDownButton";
      this.DocumentsToolStripDropDownButton.Text = "Documents";
      this.DocumentsToolStripDropDownButton.DropDownMenu = this.documentsMenu;
      this.DocumentsToolStripDropDownButton.Style = ToolBarButtonStyle.DropDownButton;
      // 
      // NewProjectToolStripMenuItem
      // 
      this.NewProjectToolStripMenuItem.Index = 0;
      this.NewProjectToolStripMenuItem.Text = "New project";
      this.NewProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectToolStripMenuItem_Click);
      // 
      // EditProjectToolStripMenuItem
      // 
      this.EditProjectToolStripMenuItem.Index = 1;
      this.EditProjectToolStripMenuItem.Text = "Edit project";
      this.EditProjectToolStripMenuItem.Click += new System.EventHandler(this.EditProjectToolStripMenuItem_Click);
      // 
      // DeleteProjectToolStripMenuItem
      // 
      this.DeleteProjectToolStripMenuItem.Index = 2;
      this.DeleteProjectToolStripMenuItem.Text = "Delete project";
      this.DeleteProjectToolStripMenuItem.Click += new System.EventHandler(this.DeleteProjectToolStripMenuItem_Click);
      // 
      // NewResourceToolStripMenuItem
      // 
      this.NewResourceToolStripMenuItem.Index = -1;
      this.NewResourceToolStripMenuItem.Text = "New resource";
      this.NewResourceToolStripMenuItem.Click += new System.EventHandler(this.NewResourceToolStripMenuItem_Click);
      // 
      // EditResourceToolStripMenuItem
      // 
      this.EditResourceToolStripMenuItem.Index = -1;
      this.EditResourceToolStripMenuItem.Text = "Edit resource";
      this.EditResourceToolStripMenuItem.Click += new System.EventHandler(this.EditResourceToolStripMenuItem_Click);
      // 
      // DeleteResourceToolStripMenuItem
      // 
      this.DeleteResourceToolStripMenuItem.Index = -1;
      this.DeleteResourceToolStripMenuItem.Text = "Delete resource";
      this.DeleteResourceToolStripMenuItem.Click += new System.EventHandler(this.DeleteResourceToolStripMenuItem_Click);
      // 
      // EditRolesToolStripMenuItem
      // 
      this.EditRolesToolStripMenuItem.Index = -1;
      this.EditRolesToolStripMenuItem.Text = "Edit roles";
      this.EditRolesToolStripMenuItem.Click += new System.EventHandler(this.EditRolesToolStripMenuItem_Click);
      // 
      // Panel1
      // 
      this.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
      this.Panel1.Dock = Wisej.Web.DockStyle.Fill;
      this.Panel1.Location = new System.Drawing.Point(0, 26);
      this.Panel1.Name = "Panel1";
      this.Panel1.Size = new System.Drawing.Size(948, 438);
      this.Panel1.TabIndex = 2;
      // 
      // StatusStrip1
      // 
      this.StatusStrip1.Location = new System.Drawing.Point(0, 442);
      this.StatusStrip1.Name = "StatusStrip1";
      this.StatusStrip1.Panels.AddRange(new Wisej.Web.StatusBarPanel[] {
            this.StatusLabel});
      this.StatusStrip1.Size = new System.Drawing.Size(948, 22);
      this.StatusStrip1.TabIndex = 3;
      this.StatusStrip1.Text = "statusStrip1";
      // 
      // StatusLabel
      // 
      this.StatusLabel.Name = "StatusLabel";
      this.StatusLabel.Text = null;
      // 
      // projectsMenu
      // 
      this.projectsMenu.MenuItems.AddRange(new Wisej.Web.MenuItem[] {
            this.NewProjectToolStripMenuItem,
            this.EditProjectToolStripMenuItem,
            this.DeleteProjectToolStripMenuItem});
      // 
      // resourcesMenu
      // 
      this.resourcesMenu.MenuItems.AddRange(new Wisej.Web.MenuItem[] {
        this.NewResourceToolStripMenuItem,
        this.EditResourceToolStripMenuItem,
        this.DeleteResourceToolStripMenuItem});
      // 
      // adminMenu
      // 
      this.adminMenu.MenuItems.AddRange(new Wisej.Web.MenuItem[] {
        this.EditRolesToolStripMenuItem});
      // 
      // documentsMenu
      // 
      this.documentsMenu.MenuItems.AddRange(new Wisej.Web.MenuItem[] {});
      // 
      // MainPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
      this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
      this.Controls.Add(this.StatusStrip1);
      this.Controls.Add(this.Panel1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "MainPage";
      this.Size = new System.Drawing.Size(948, 464);
      this.Text = "Project Tracker";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal Wisej.Web.ToolBar toolStrip1;
    internal Wisej.Web.ToolBarButton ProjectsStripDropDownButton1;
    internal Wisej.Web.MenuItem NewProjectToolStripMenuItem;
    internal Wisej.Web.MenuItem EditProjectToolStripMenuItem;
    internal Wisej.Web.MenuItem DeleteProjectToolStripMenuItem;
    internal Wisej.Web.ToolBarButton LoginToolStripLabel;
    internal Wisej.Web.ToolBarButton LoginToolStripButton;
    internal Wisej.Web.ToolBarButton ResourcesToolStripDropDownButton;
    internal Wisej.Web.MenuItem NewResourceToolStripMenuItem;
    internal Wisej.Web.MenuItem EditResourceToolStripMenuItem;
    internal Wisej.Web.MenuItem DeleteResourceToolStripMenuItem;
    internal Wisej.Web.ToolBarButton AdminToolStripDropDownButton;
    internal Wisej.Web.MenuItem EditRolesToolStripMenuItem;
    internal Wisej.Web.ToolBarButton DocumentsToolStripDropDownButton;
    internal Wisej.Web.Panel Panel1;
    internal Wisej.Web.StatusBar StatusStrip1;
    internal Wisej.Web.StatusBarPanel StatusLabel;
    private Wisej.Web.ContextMenu projectsMenu;
    private Wisej.Web.ContextMenu resourcesMenu;
    private Wisej.Web.ContextMenu adminMenu;
    private Wisej.Web.ContextMenu documentsMenu;
  }
}