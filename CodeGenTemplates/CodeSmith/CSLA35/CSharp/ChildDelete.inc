<% if(objInfo.IsGeneratedClass) {%>
<%=Indent(i)%>#region Data Access - DeleteSelf
<%=Indent(i)%>private void Child_DeleteSelf(System.Data.Linq.EntitySet<<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>> dataSet)
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>On<%=objInfo.Type%>SelfDeleting(dataSet, ref cancel);
<%=Indent(i)%>if (cancel) return;

<%}%>
<%=Indent(i)%>using (var mgr = Csla.Data.ContextManager<<%=objInfo.LinqDataContext%>>
<%=Indent(i+3)%>.GetManager(<%=objInfo.DbConnection%>))
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>On<%=objInfo.Type%>MemberSelfDeleting();
<%}%>
<%if(objInfo.ChildCollection.Count>0) {%>
<%=Indent(i)%>//Delete child object(s)
<%}%>
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%><%=prop.Name%>.Clear();
<%=Indent(i)%>DataPortal.UpdateChild(ReadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>), data.<%=prop.DbRefTableName%>);
<% } %>
<%=Indent(i)%>var data = new <%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>() 
<%=Indent(i++)%>{
<% for(int j=0; j<objInfo.UniqueProperties.Count; j++) { 
	PropertyInfo prop = objInfo.UniqueProperties[j] as PropertyInfo; %>
<%=Indent(i)%><%=prop.DbColumnName%> = ReadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>)<%if(j<objInfo.UniqueProperties.Count-1){%>,<%}%>
<% } %>
<%=Indent(--i)%>};
<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.IsTimestamp) continue; %>	 
<%=Indent(i)%>data.<%=prop.DbColumnName%> = <%=prop.MemberName%>;
<% } %>
<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.Attach(data);

<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.DeleteOnSubmit(data);
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>On<%=objInfo.Type%>MemberSelfDeleted();
<%}%>
<%=Indent(--i)%>}//using
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%>SetProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, <%=prop.DefaultValue%>);
<% } %>
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>On<%=objInfo.Type%>SelfDeleted();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>partial void On<%=objInfo.Type%>SelfDeleting(System.Data.Linq.EntitySet<<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>> dataSet, ref bool cancel);
<%=Indent(i)%>partial void On<%=objInfo.Type%>SelfDeleted();
<%=Indent(i)%>partial void On<%=objInfo.Type%>MemberSelfDeleting();
<%=Indent(i)%>partial void On<%=objInfo.Type%>MemberSelfDeleted();
<%}%>
<%=Indent(i)%>#endregion //Data Access - DeleteSelf
<%}%>