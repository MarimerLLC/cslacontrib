<%=Indent(0, True)%>#Region " Validation Rules "
<% If objInfo.IsUserBase OrElse objInfo.IsUserPartial OrElse objInfo.IsSingle OrElse objInfo.IsGeneratedBase OrElse objInfo.IsGeneratedClass Then %>
<% If objInfo.IsUserBase Then %>
<%=Indent(i, True)%>Protected Overrides Sub AddCustomRules()
<%=Indent(i+1)%>
<%=Indent(i+1)%>'add custom/non-generated rules here...
<%=Indent(i+1)%>
<%=Indent(i)%>End Sub
<% Else %>
<%=Indent(i, True)%><%=objInfo.LocalMethodModifiers%> Sub AddCustomRules()
<% If objInfo.IsGeneratedBase OrElse objInfo.IsUserPartial Then %>
<%=Indent(i+1)%>'hook for adding custom/non-generated rules
<% Else %>
<%=Indent(i+1)%>'add custom/non-generated rules here...
<% End If %>
<%=Indent(i)%>End Sub
<% End If %>
<% If objInfo.IsGeneratedClass Then %>
<%=Indent(i, True)%><%=objInfo.LocalMethodModifiers%> Sub AddCommonRules()
<%= GetCommonValidationRules(objInfo, i+1)%>
<%=Indent(i)%>End Sub
<%=Indent(i, True)%>Protected Overrides Sub AddBusinessRules()
<%=Indent(i+1)%>AddCommonRules()
<% If objInfo.IsGeneratedPartial Then %>
<%=Indent(i+1)%>OnAddCustomRules()
<% Else %>
<%=Indent(i+1)%>AddCustomRules()
<% End If %>
<%=Indent(i)%>End Sub
<% End If %>
<% End If %>
<%=Indent(0, True)%>#End Region 'Validation Rules
