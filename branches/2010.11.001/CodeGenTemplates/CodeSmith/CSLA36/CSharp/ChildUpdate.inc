<% if(objInfo.IsGeneratedClass) {%>

<%=Indent(i)%>#region Data Access - Insert
<%=Indent(i)%>private void Child_Insert(<% if(objInfo.Parent.Length>0) {%><%=objInfo.Parent%> parent<% } %>)
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>OnInserting(<% if(objInfo.Parent.Length>0) {%>parent , <% } %>ref cancel);
<%=Indent(i)%>if (cancel) return;

<%}%>
<%=Indent(i)%>using (var mgr = Csla.Data.ContextManager<<%=objInfo.LinqDataContext%>>
<%=Indent(i+3)%>.GetManager(<%=objInfo.DbConnection%>))
<%=Indent(i++)%>{
<%=Indent(i)%>var data = new <%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>();

<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>OnMemberReading(data);
<%}%>
<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.HasDbColumn || prop.IsDbComputed) continue; %>	 
<%=Indent(i)%>data.<%=prop.DbColumnName%> = ReadProperty(<%=prop.PropertyInfoName%>);
<% } %>
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>OnMemberRead();
<%}%>

<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.InsertOnSubmit(data);
<%=Indent(i)%>mgr.DataContext.SubmitChanges();

<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.HasDbColumn || !prop.IsDbComputed) continue; %>
<%  if (prop.IsTimestamp) { %>        	 
<%=Indent(i)%><%=prop.MemberName%> = data.<%=prop.DbColumnName%>.ToArray();
<%  } else if(prop.Type != prop.NativeType) { %>
<%=Indent(i)%>LoadPropertyConvert(<%=prop.PropertyInfoName%>, data.<%=prop.DbColumnName%>);
<%  } else { %>
<%=Indent(i)%>LoadProperty(<%=prop.PropertyInfoName%>, data.<%=prop.DbColumnName%>);
<%  } %>
<% } %>
<%if(objInfo.ChildCollection.Count>0) {%>

<%=Indent(i)%>//Insert Child object(s)
<%=Indent(i)%>FieldManager.UpdateChildren(this);
<%}%>
<%=Indent(--i)%>}//using
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>OnInserted();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void OnInserting(<% if(objInfo.Parent.Length>0) {%><%=objInfo.Parent%> parent, <% } %>ref bool cancel);
<%=Indent(i)%>partial void OnInserted();
<%=Indent(i)%>partial void OnMemberReading(<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%> data);
<%=Indent(i)%>partial void OnMemberRead();
<%}%>
<%=Indent(i)%>#endregion //Data Access - Insert

<%=Indent(i)%>#region Data Access - Update
<%=Indent(i)%>private void Child_Update(<% if(objInfo.Parent.Length>0) {%><%=objInfo.Parent%> parent<% } %>)
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>OnUpdating(<% if(objInfo.Parent.Length>0) {%>parent, <% } %>ref cancel);
<%=Indent(i)%>if (cancel) return;

<%}%>
<%=Indent(i)%>using (var mgr = Csla.Data.ContextManager<<%=objInfo.LinqDataContext%>>
<%=Indent(i+3)%>.GetManager(<%=objInfo.DbConnection%>))
<%=Indent(i++)%>{
<%=Indent(i)%>var data = new <%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>() 
<%=Indent(i++)%>{
<% for(int j=0; j<objInfo.UniqueProperties.Count; j++) { 
	PropertyInfo prop = objInfo.UniqueProperties[j] as PropertyInfo; %>
<%=Indent(i)%><%=prop.DbColumnName%> = ReadProperty(<%=prop.PropertyInfoName%>)<%if(j<objInfo.UniqueProperties.Count-1){%>,<%}%>
<% } %>
<%=Indent(--i)%>};
<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.IsTimestamp) continue; %>	 
<%=Indent(i)%>data.<%=prop.DbColumnName%> = <%=prop.MemberName%>;
<% } %>

<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>OnMemberReading(data);
<%}%>
<%=Indent(i)%>if (IsSelfDirty)
<%=Indent(i++)%>{
<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.HasDbColumn || prop.IsDbComputed || prop.IsPrimaryKey) continue; %>	 
<%=Indent(i)%>data.<%=prop.DbColumnName%> = ReadProperty(<%=prop.PropertyInfoName%>);
<% } %>
<%if(objInfo.HasTimestamp) {%>
<%=Indent(i)%>data.PropertyChanged += delegate(object o, System.ComponentModel.PropertyChangedEventArgs e)
<%=Indent(i++)%>{
<% foreach(PropertyInfo prop in objInfo.Properties) {
	if (!prop.IsTimestamp) continue; %>	 
<%=Indent(i)%>if (e.PropertyName == "<%=prop.DbColumnName%>")
<%=Indent(i++)%>{
<%=Indent(i)%>var d = (<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>)o;
<%=Indent(i)%><%=prop.MemberName%> = d.<%=prop.DbColumnName%>.ToArray();
<%=Indent(--i)%>}
<% } %>
<%=Indent(--i)%>};
<%}%>
<%=Indent(i)%>mgr.DataContext.<%=objInfo.EntitySetName%>.Attach(data, true);
<%=Indent(--i)%>}
<%if(objInfo.ChildCollection.Count>0) {%>

<%=Indent(i)%>//Update Child object(s)
<%}%>
<%  foreach (PropertyInfo prop in objInfo.ChildCollection) { %>
<%=Indent(i)%>DataPortal.UpdateChild(ReadProperty(<%=prop.PropertyInfoName%>), this);
<% } %>
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>OnMemberRead();
<%}%>
<%=Indent(--i)%>}//using
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>OnUpdated();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void OnUpdating(<% if(objInfo.Parent.Length>0) {%><%=objInfo.Parent%> parent, <% } %>ref bool cancel);
<%=Indent(i)%>partial void OnUpdated();
<%}%>
<%=Indent(i)%>#endregion //Data Access - Update
<%}%>