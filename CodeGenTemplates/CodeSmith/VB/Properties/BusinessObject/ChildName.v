
    ' The variable referenced in this property is defined in the CSLAHelper20.vb file
  <Category(" Business Object"), _
  Description("The Member Name of the Business Object's Child.")> _
  Public Property ChildName() As String
    Get
      Return m_ChildName
    End Get
    Set(ByVal Value As String)
      m_ChildName = Value
    End Set
  End Property
