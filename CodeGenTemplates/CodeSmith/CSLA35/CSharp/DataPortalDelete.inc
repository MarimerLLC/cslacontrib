<%=Indent(i, true)%>#region Data Access - Delete
<% if(objInfo.UseTransactionalAttribute) {%>
<%=Indent(i)%>[Transactional(TransactionalTypes.<%=objInfo.TransactionType.ToString()%>)] 
<%}%>
<%=Indent(i)%>protected override void DataPortal_DeleteSelf()
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>On<%=objInfo.Type%>SelfDeleting(ref cancel);
<%=Indent(i)%>if (cancel) return;

<%}%>
<%=Indent(i)%>using (var mgr = ContextManager<<%=objInfo.LinqDataContext%>>
<%=Indent(i+3)%>.GetManager(<%=objInfo.DbConnection%>))
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>On<%=objInfo.Type%>MemberSelfDeleting();
<%}%>
<%if(objInfo.ChildCollection.Count>0) {%>
<%=Indent(i)%>//Delete Child object(s), which may contain concurrency check
<%}%>
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%><%=prop.Name%>.Clear();
<%=Indent(i)%>DataPortal.UpdateChild(ReadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>), this);
<% } %>
<%if(objInfo.ChildCollection.Count>0) {%><%=Indent(0)%><%}%>
<%=Indent(i)%>var data = new <%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>() 
<%=Indent(i++)%>{
<% for(int j=0; j<objInfo.UniqueProperties.Count; j++) { 
	PropertyInfo prop = objInfo.UniqueProperties[j] as PropertyInfo; %>
<%=Indent(i)%><%=prop.DbColumnName%> = ReadProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>)<%if(j<objInfo.UniqueProperties.Count-1){%>,<%}%>
<% } %>
<%=Indent(--i)%>};
<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.HasDbColumn || !prop.IsTimestamp) continue; %>	 
<%=Indent(i)%>data.<%=prop.DbColumnName%> = <%=prop.MemberName%>;
<% } %>
<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.Attach(data);

<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.DeleteOnSubmit(data);
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>On<%=objInfo.Type%>MemberSelfDeleted();
<%}%>

<%=Indent(i)%>mgr.DataContext.SubmitChanges();
<%=Indent(--i)%>}//using
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%>SetProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, <%=prop.DefaultValue%>);
<% } %>
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>On<%=objInfo.Type%>SelfDeleted();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void On<%=objInfo.Type%>SelfDeleting(ref bool cancel);
<%=Indent(i)%>partial void On<%=objInfo.Type%>SelfDeleted();
<%=Indent(i)%>partial void On<%=objInfo.Type%>MemberSelfDeleting();
<%=Indent(i)%>partial void On<%=objInfo.Type%>MemberSelfDeleted();
<%}%>

<% if(objInfo.UseTransactionalAttribute) {%>
<%=Indent(i)%>[Transactional(TransactionalTypes.<%=objInfo.TransactionType.ToString()%>)] 
<%}%>
<%=Indent(i)%>private void DataPortal_Delete(<%=GetFactoryDPDeclarationArguments(objInfo)%>)
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>On<%=objInfo.Type%>Deleting(criteria, ref cancel);
<%=Indent(i)%>if (cancel) return;

<%}%>
<%=Indent(i)%>using (var mgr = ContextManager<<%=objInfo.LinqDataContext%>>
<%=Indent(i+3)%>.GetManager(<%=objInfo.DbConnection%>))
<%=Indent(i++)%>{
<%=Indent(i)%>var data = mgr.DataContext.<%=objInfo.EntitySetName%>
<%=Indent(i+2)%>.Single(r => <% for(int j=0; j<objInfo.UniqueProperties.Count; j++) { 
	PropertyInfo prop = objInfo.UniqueProperties[j] as PropertyInfo; %><%if(j>0){%>
<%=Indent(i+4)%>&& <%}%>r.<%=prop.DbColumnName%> == criteria.<%if(objInfo.UniqueProperties.Count==1){%>Value<%}else{%><%=prop.Name%><%}%><% } %>);

<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>On<%=objInfo.Type%>MemberDeleting(data);
<%}%>
<%if(objInfo.ChildCollection.Count>0) {%>
<%=Indent(i)%>//Delete Child object(s)
<%}%>
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%>mgr.DataContext.<%=prop.DbRefTableName%>.DeleteAllOnSubmit(data.<%=prop.DbRefTableName%>);
<% } %>

<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.DeleteOnSubmit(data);
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>On<%=objInfo.Type%>MemberDeleted();
<%}%>

<%=Indent(i)%>mgr.DataContext.SubmitChanges();
<%=Indent(--i)%>}//using
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%>SetProperty<<%=prop.Type%>>(<%=prop.PropertyInfoName%>, <%=prop.DefaultValue%>);
<% } %>
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>On<%=objInfo.Type%>Deleted();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void On<%=objInfo.Type%>Deleting(<%=GetFactoryDPDeclarationArguments(objInfo, "ref bool cancel")%>);
<%=Indent(i)%>partial void On<%=objInfo.Type%>Deleted();
<%=Indent(i)%>partial void On<%=objInfo.Type%>MemberDeleting(<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%> data);
<%=Indent(i)%>partial void On<%=objInfo.Type%>MemberDeleted();
<%}%>
<%=Indent(i)%>#endregion //Data Access - Delete
