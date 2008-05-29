<% if(objInfo.IsGeneratedClass) {%>
<%=Indent(i)%>#region Data Access - Fetch
<% if(objInfo.UseTransactionalAttribute) {%>
<%=Indent(i)%>[Transactional(TransactionalTypes.<%=objInfo.TransactionType.ToString()%>)] 
<%}%>
<%=Indent(i)%>private void DataPortal_Fetch(<%=GetFactoryDPFilterDeclarationArguments(objInfo)%>)
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>On<%=objInfo.Type%>Fetching(criteria, ref cancel);
<%=Indent(i)%>if (cancel) return;

<%}%>
<%=Indent(i)%>RaiseListChangedEvents = false;
<% if(objInfo.IsReadOnly) {%>
<%=Indent(i)%>IsReadOnly = false;
<%}%>
<%=Indent(i)%>using (var mgr = ContextManager<<%=objInfo.LinqDataContext%>>
<%=Indent(i+3)%>.GetManager(<%=objInfo.DbConnection%>))
<%=Indent(i++)%>{
<%=Indent(i)%>var dataSet = from t in mgr.DataContext.<%=objInfo.EntitySetName%>
<%=Indent(i+2)%>where <% for(int j=0; j<objInfo.FilterProperties.Count; j++) { 
	PropertyInfo prop = objInfo.FilterProperties[j] as PropertyInfo; %><%if(j>0){%>
<%=Indent(i+3)%>&& <%}%>t.<%=prop.DbColumnName%> == criteria.<%if(objInfo.FilterProperties.Count==1){%>Value<%}else{%><%=prop.Name%><%}%><% } %>
<%=Indent(i+2)%>select t;

<%=Indent(i)%>RaiseListChangedEvents = false;

<%=Indent(i)%>foreach (var data in dataSet)
<%=Indent(i+1)%>this.Add(<%=objInfo.Child%>.<%=objInfo.GetChildMethodName%>(data));

<%=Indent(i)%>RaiseListChangedEvents = true;
<%=Indent(--i)%>}//using


<% if(objInfo.IsReadOnly) {%>
<%=Indent(i)%>IsReadOnly = true;
<%}%>
<%=Indent(i)%>RaiseListChangedEvents = true;
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>On<%=objInfo.Type%>Fetched();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void On<%=objInfo.Type%>Fetching(<%=GetFactoryDPFilterDeclarationArguments(objInfo, "ref bool cancel")%>);
<%=Indent(i)%>partial void On<%=objInfo.Type%>Fetched();
<%}%>
<%=Indent(i)%>#endregion //Data Access - Fetch
<%}%>