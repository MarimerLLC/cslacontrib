<% if(objInfo.IsGeneratedClass) {%>
<%=Indent(i)%>#region Data Access - Create
<%=Indent(i)%>private void Child_Create(<%=GetFactoryNewDeclarationArguments(objInfo)%>) 
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>On<%=objInfo.Type%>Creating(<%=GetFactoryNewCallArguments(objInfo, "ref cancel")%>);
<%=Indent(i)%>if (cancel) return;

<%}%>
<% if(!objInfo.IsCollection && !objInfo.HasIdentity) { 
   foreach(PropertyInfo prop in objInfo.UniqueProperties) { %>   
<%if(prop.Type == "Guid" && objInfo.HasObjectGeneratedKey) {%>
<%=Indent(i)%>LoadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, Guid.NewGuid());
<% } else { %>
<%=Indent(i)%>LoadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, <%=CsHelper.GetCamelCaseName(prop.Name)%>);
<%}%>
<% }} %>
<% if(!objInfo.IsReadOnly && !objInfo.IsCollection){ %>
<%=Indent(i)%>ValidationRules.CheckRules();
<% } %>
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>On<%=objInfo.Type%>Created();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void On<%=objInfo.Type%>Creating(<%=GetFactoryNewDeclarationArguments(objInfo, "ref bool cancel")%>);
<%=Indent(i)%>partial void On<%=objInfo.Type%>Created();
<%}%>
<%=Indent(i)%>#endregion //Data Access - Create
<%}%>