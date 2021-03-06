
<%=Indent(i)%>#region Factory Methods
<% if(objInfo.IsUserBase) {%>
<%=Indent(i)%>private <%=objInfo.Name%>() : base() { /* require use of factory method */ }

<% } %>
<% PropertyInfo idProp = null; 
	if(objInfo.HasIdentity) { 
		idProp = (PropertyInfo)objInfo.UniqueProperties[0]; 
		if(objInfo.IsGeneratedClass && !objInfo.IsCollection) { %>
<%=Indent(i)%>private static <%=idProp.Type%> _new<%=idProp.Name%> = -1;

<% }} %>
<% if(objInfo.IsGeneratedClass) {%>
<% if(!objInfo.IsReadOnly) {%>
<%=Indent(i)%>internal static <%=objInfo.Type%> <%=objInfo.NewMethodName%>(<%=GetFactoryNewDeclarationArguments(objInfo)%>)
<%=Indent(i++)%>{
<% if(objInfo.IsGeneratedBase) { //abstract-child cannot use constructor %>
<%=Indent(i)%><%=objInfo.Type%> child = DataPortal.Create<<%=objInfo.Type%>>(<% string cPars = GetFactoryNewCallArguments(objInfo); if(cPars.Length>0) {%>new Criteria(<%=cPars%>)<%}%>);
<% if(!objInfo.IsCollection) {%>
<% if(objInfo.HasIdentity) { %>
<%=Indent(i)%>child.<%=idProp.MemberName%> = _new<%=idProp.Name%>--;
<% } %>
<%=Indent(i)%>child.ValidationRules.CheckRules();
<% } %>
<%=Indent(i)%>return child;
<% } else { %>
<%=Indent(i)%>return new <%=objInfo.Name%>(<%=GetFactoryNewCallArguments(objInfo)%>);
<% } %>
<%=Indent(--i)%>}

<%} //if(!objInfo.IsReadOnly)%>
<%=Indent(i)%>internal static <%=objInfo.Type%> <%=objInfo.GetMethodName%>(SafeDataReader dr)
<%=Indent(i++)%>{
<% if(objInfo.IsGeneratedBase) { %>
<%=Indent(i)%><%=objInfo.Type%> child = DataPortal.Create<<%=objInfo.Type%>>();
<%=Indent(i)%>child.Fetch(dr);
<%=Indent(i)%>return child;
<% } else { %>
<%=Indent(i)%>return new <%=objInfo.Name%>(dr);
<% } %>
<%=Indent(--i)%>}

<% if(!objInfo.IsGeneratedBase) { %>
<% if(!objInfo.IsReadOnly) {%>
<%=Indent(i)%><%=objInfo.MemberAccess%> <%=objInfo.Name%>(<%=GetFactoryNewDeclarationArguments(objInfo)%>)
<%=Indent(i++)%>{
<% if(!objInfo.IsCollection) {%>
<% if(objInfo.HasIdentity) { %>
<%=Indent(i)%><%=idProp.MemberName%> = _new<%=idProp.Name%>--;
<% } %>
<%=GetFactoryNewAssignments(objInfo, i)%>
<%=Indent(i)%>ValidationRules.CheckRules();
<% } %>
<%=Indent(i)%>MarkAsChild();
<%=Indent(--i)%>}

<%} //if(!objInfo.IsReadOnly)%>
<%=Indent(i)%>private <%=objInfo.Name%>(SafeDataReader dr)
<%=Indent(i++)%>{
<% if(!objInfo.IsReadOnly) {%>
<%=Indent(i)%>MarkAsChild();
<% } %>
<%=Indent(i)%>Fetch(dr);
<%=Indent(--i)%>}
<% } %>
<% } //if(objInfo.IsGeneratedClass)%>
<%=Indent(i)%>#endregion //Factory Methods
