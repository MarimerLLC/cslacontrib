<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim IdLabel As System.Windows.Forms.Label
        Dim NameLabel As System.Windows.Forms.Label
        Dim SomeValueLabel As System.Windows.Forms.Label
        Me.RootBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.IdTextBox = New System.Windows.Forms.TextBox
        Me.NameTextBox = New System.Windows.Forms.TextBox
        Me.SomeValueTextBox = New System.Windows.Forms.TextBox
        Me.ChildrenBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ChildrenDataGridView = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GrandChildrenBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GrandChildrenDataGridView = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ErrorTreeView1 = New CslaErrorTreeView.ErrorTreeView
        IdLabel = New System.Windows.Forms.Label
        NameLabel = New System.Windows.Forms.Label
        SomeValueLabel = New System.Windows.Forms.Label
        CType(Me.RootBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChildrenBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChildrenDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrandChildrenBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrandChildrenDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IdLabel
        '
        IdLabel.AutoSize = True
        IdLabel.Location = New System.Drawing.Point(11, 10)
        IdLabel.Name = "IdLabel"
        IdLabel.Size = New System.Drawing.Size(19, 13)
        IdLabel.TabIndex = 1
        IdLabel.Text = "Id:"
        '
        'NameLabel
        '
        NameLabel.AutoSize = True
        NameLabel.Location = New System.Drawing.Point(11, 36)
        NameLabel.Name = "NameLabel"
        NameLabel.Size = New System.Drawing.Size(38, 13)
        NameLabel.TabIndex = 3
        NameLabel.Text = "Name:"
        '
        'SomeValueLabel
        '
        SomeValueLabel.AutoSize = True
        SomeValueLabel.Location = New System.Drawing.Point(11, 62)
        SomeValueLabel.Name = "SomeValueLabel"
        SomeValueLabel.Size = New System.Drawing.Size(67, 13)
        SomeValueLabel.TabIndex = 5
        SomeValueLabel.Text = "Some Value:"
        '
        'RootBindingSource
        '
        Me.RootBindingSource.DataSource = GetType(UsageSample.Root)
        '
        'IdTextBox
        '
        Me.IdTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RootBindingSource, "Id", True))
        Me.IdTextBox.Location = New System.Drawing.Point(84, 7)
        Me.IdTextBox.Name = "IdTextBox"
        Me.IdTextBox.Size = New System.Drawing.Size(100, 20)
        Me.IdTextBox.TabIndex = 2
        '
        'NameTextBox
        '
        Me.NameTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RootBindingSource, "Name", True))
        Me.NameTextBox.Location = New System.Drawing.Point(84, 33)
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.NameTextBox.TabIndex = 4
        '
        'SomeValueTextBox
        '
        Me.SomeValueTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RootBindingSource, "SomeValue", True))
        Me.SomeValueTextBox.Location = New System.Drawing.Point(84, 59)
        Me.SomeValueTextBox.Name = "SomeValueTextBox"
        Me.SomeValueTextBox.Size = New System.Drawing.Size(100, 20)
        Me.SomeValueTextBox.TabIndex = 6
        '
        'ChildrenBindingSource
        '
        Me.ChildrenBindingSource.DataMember = "Children"
        Me.ChildrenBindingSource.DataSource = Me.RootBindingSource
        '
        'ChildrenDataGridView
        '
        Me.ChildrenDataGridView.AutoGenerateColumns = False
        Me.ChildrenDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3})
        Me.ChildrenDataGridView.DataSource = Me.ChildrenBindingSource
        Me.ChildrenDataGridView.Location = New System.Drawing.Point(12, 85)
        Me.ChildrenDataGridView.Name = "ChildrenDataGridView"
        Me.ChildrenDataGridView.Size = New System.Drawing.Size(368, 109)
        Me.ChildrenDataGridView.TabIndex = 6
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Id"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Id"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "SomeValue"
        Me.DataGridViewTextBoxColumn3.HeaderText = "SomeValue"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'GrandChildrenBindingSource
        '
        Me.GrandChildrenBindingSource.DataMember = "GrandChildren"
        Me.GrandChildrenBindingSource.DataSource = Me.ChildrenBindingSource
        '
        'GrandChildrenDataGridView
        '
        Me.GrandChildrenDataGridView.AutoGenerateColumns = False
        Me.GrandChildrenDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6})
        Me.GrandChildrenDataGridView.DataSource = Me.GrandChildrenBindingSource
        Me.GrandChildrenDataGridView.Location = New System.Drawing.Point(386, 85)
        Me.GrandChildrenDataGridView.Name = "GrandChildrenDataGridView"
        Me.GrandChildrenDataGridView.Size = New System.Drawing.Size(370, 109)
        Me.GrandChildrenDataGridView.TabIndex = 7
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "Id"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Id"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "SomeValue"
        Me.DataGridViewTextBoxColumn6.HeaderText = "SomeValue"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'ErrorTreeView1
        '
        Me.ErrorTreeView1.Errors = Nothing
        Me.ErrorTreeView1.ImageIndex = 0
        Me.ErrorTreeView1.Location = New System.Drawing.Point(10, 200)
        Me.ErrorTreeView1.Name = "ErrorTreeView1"
        Me.ErrorTreeView1.SelectedImageIndex = 0
        Me.ErrorTreeView1.Size = New System.Drawing.Size(746, 232)
        Me.ErrorTreeView1.TabIndex = 8
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(793, 444)
        Me.Controls.Add(Me.ErrorTreeView1)
        Me.Controls.Add(Me.GrandChildrenDataGridView)
        Me.Controls.Add(Me.ChildrenDataGridView)
        Me.Controls.Add(IdLabel)
        Me.Controls.Add(Me.IdTextBox)
        Me.Controls.Add(NameLabel)
        Me.Controls.Add(Me.NameTextBox)
        Me.Controls.Add(SomeValueLabel)
        Me.Controls.Add(Me.SomeValueTextBox)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.RootBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChildrenBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChildrenDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrandChildrenBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrandChildrenDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RootBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents IdTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SomeValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ChildrenBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ChildrenDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GrandChildrenBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents GrandChildrenDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ErrorTreeView1 As CslaErrorTreeView.ErrorTreeView

End Class
