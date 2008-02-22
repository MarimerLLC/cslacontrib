<% if(objInfo.IsGeneratedClass) {%>
<%=Indent(i)%>#region Data Access - Create
<%=Indent(i)%>[RunLocal]
<%if(objInfo.HasIdentity || objInfo.HasObjectGeneratedKey || objInfo.IsCollection) {%>
<%=Indent(i)%>protected override void DataPortal_Create()
<% } else { %>
<%=Indent(i)%>private void DataPortal_Create(<%=GetFactoryNewDPDeclarationArgutments(objInfo)%>)
<%}%>
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%if(objInfo.HasIdentity || objInfo.HasObjectGeneratedKey || objInfo.IsCollection) {%>
<%=Indent(i)%>On<%=objInfo.Type%>Creating(ref cancel);
<% } else { %>
<%=Indent(i)%>On<%=objInfo.Type%>Creating(criteria, ref cancel);
<%}%>
<%=Indent(i)%>if (cancel) return;

<%}%>
<% if(!objInfo.IsCollection && !objInfo.HasIdentity) { 
   foreach(PropertyInfo prop in objInfo.UniqueProperties) { %>   
<%if(prop.Type == "Guid" && objInfo.HasObjectGeneratedKey) {%>
<%=Indent(i)%>LoadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, Guid.NewGuid());
<% } else { %>
<%=Indent(i)%>LoadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, criteria.<%if(objInfo.UniqueProperties.Count==1){%>Value<%}else{%><%=prop.Name%><%}%>);
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

<%=Indent(i)%>partial void On<%=objInfo.Type%>Creating(<%=GetFactoryNewDPDeclarationArgutments(objInfo, "ref bool cancel")%>);
<%=Indent(i)%>partial void On<%=objInfo.Type%>Created();
<%}%>
<%=Indent(i)%>#endregion //Data Access - Create
<%}%>