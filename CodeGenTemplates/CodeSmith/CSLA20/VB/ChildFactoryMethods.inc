
#Region " Factory Methods "
<% If objInfo.IsUserBase Then %>
<%=Indent(i, True)%>Private Sub New() 
<%=Indent(i+1)%>MyBase.New()  ' require use of factory method  
<%=Indent(i)%>End Sub
<% End If %>
<% If objInfo.IsGeneratedClass Then %>
<% If Not objInfo.IsReadOnly Then %>
<%=Indent(i, True)%>Friend Shared Function <%=objInfo.NewMethodName%>(<%=GetFactoryNewDeclarationArguments(objInfo)%>) As <%=objInfo.Type%> 
<% i+=1 %>
<% If objInfo.IsGeneratedBase Then 'abstract-child cannot use constructor %>
<%=Indent(i)%>Dim child As <%=objInfo.Type%> = DataPortal.Create(Of <%=objInfo.Type%>)(<%If Not objInfo.HasIdentity AndAlso Not objInfo.IsGeneratedClass Then %>New Criteria(<%=GetFactoryNewCallArguments(objInfo)%>)<% End If %>)
<% If Not objInfo.IsCollection Then %>
<%=Indent(i)%>child.ValidationRules.CheckRules()
<% End If %>
<%=Indent(i)%>Return child
<% Else %>
<%=Indent(i)%>Return New <%=objInfo.Name%>(<%=GetFactoryNewCallArguments(objInfo)%>)
<% End If %>
<% i-=1 %>
<%=Indent(i)%>End Function
<% End If %>
<%=Indent(i, True)%>Friend Shared Function <%=objInfo.GetMethodName%>(ByVal dr As SafeDataReader) As <%=objInfo.Type%>
<% i+=1 %> 
<% If objInfo.IsGeneratedBase Then %>
<%=Indent(i)%>Dim child As <%=objInfo.Type%> = DataPortal.Create(Of <%=objInfo.Type%>)()
<%=Indent(i)%>child.Fetch(dr)
<%=Indent(i)%>Return child
<% Else %>
<%=Indent(i)%>Return New <%=objInfo.Name%>(dr)
<% End If %>
<% i-=1 %>
<%=Indent(i)%>End Function
<% If Not objInfo.IsGeneratedBase Then %>
<% If Not objInfo.IsReadOnly Then %>
<%=Indent(i, True)%><%=objInfo.MemberAccess%> Sub New(<%=GetFactoryNewDeclarationArguments(objInfo)%>)
<% i+=1 %>
<% If Not objInfo.IsCollection Then %>
<%=GetFactoryNewAssignments(objInfo, i)%>
<% End If %>
<%=Indent(i)%>MarkAsChild()
<% i-=1 %>
<%=Indent(i)%>End Sub
<% End If %>
<%=Indent(i, True)%>Private Sub New(ByVal dr As SafeDataReader)
<% i+=1 %>
<% If Not objInfo.IsReadOnly Then %>
<%=Indent(i)%>MarkAsChild()
<% End If %>
<%=Indent(i)%>Fetch(dr)
<% i-=1 %>
<%=Indent(i)%>End Sub
<% End If %><% End If %>

#End Region 
