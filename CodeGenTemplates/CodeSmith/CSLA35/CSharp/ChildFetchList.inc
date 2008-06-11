<% if(objInfo.IsGeneratedClass) {%>
<%=Indent(i)%><%=objInfo.LocalMethodModifiers%> void Fetch(IEnumerable<<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>> dataSet)
<%=Indent(i++)%>{
<%if(objInfo.IsGeneratedPartial) {%>
<%=Indent(i)%>bool cancel = false;
<%=Indent(i)%>On<%=objInfo.Type%>Fetching(dataSet, ref cancel);
<%=Indent(i)%>if(cancel) return;

<%}%>
<%=Indent(i)%>RaiseListChangedEvents = false;
<% if(objInfo.IsReadOnly) {%>
<%=Indent(i)%>IsReadOnly = false;
<%}%>

<%=Indent(i)%>foreach (var data in dataSet)
<%=Indent(i+1)%>this.Add(<%=objInfo.Child%>.<%=objInfo.GetChildMethodName%>(data));

<% if(objInfo.IsReadOnly) {%>
<%=Indent(i)%>IsReadOnly = true;
<%}%>
<%=Indent(i)%>RaiseListChangedEvents = true;
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>On<%=objInfo.Type%>Fetched();
<%}%>
<%=Indent(--i)%>}
<%if(objInfo.IsGeneratedPartial) {%>

<%=Indent(i)%>partial void On<%=objInfo.Type%>Fetching(IEnumerable<<%=objInfo.DalNamespace%>.<%=objInfo.EntityName%>> dataSet, ref bool cancel);
<%=Indent(i)%>partial void On<%=objInfo.Type%>Fetched();
<%}%>
<%}%>