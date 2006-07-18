<%=Indent(0, true)%>#Region " Data Access - Fetch "

<% if(objInfo.UseTransactionalAttribute) then %>
<%=Indent(i)%><Transactional(TransactionalTypes.<%=objInfo.TransactionType.ToString()%>)> _ 
<% end if %>
<%=Indent(i)%>Private Overloads Sub DataPortal_Fetch(byval criteria as FilterCriteria)
<% i+=1 %>
<%=Indent(i)%>RaiseListChangedEvents = false
<% if(objInfo.IsReadOnly) then%>
<%=Indent(i)%>IsReadOnly = false
<% end if %>

<%if(objInfo.UseAdoTransaction) then%>
<%=Indent(i)%>Dim tr as SqlTransaction 

<%end if%>
<%=Indent(i)%>Using cn as SqlConnection = new SqlConnection(<%=objInfo.DbConnection%>)
<% i+=1 %>
<%=Indent(i)%>cn.Open()

<% if(objInfo.UseAdoTransaction) then%>
<%=Indent(i)%>tr = cn.BeginTransaction()
<%=Indent(i)%>try
<% i+=1 %>
<% end if %>
<%=Indent(i)%>ExecuteFetch(<%if(objInfo.UseAdoTransaction) then%>tr<%else%>cn<%end if%>, criteria)
<% if(objInfo.UseAdoTransaction) then%>

<%=Indent(i)%>tr.Commit()
<% i-=1 %>
<%=Indent(i)%>catch
<% i+=1 %>
<%=Indent(i)%>tr.Rollback()
<%=Indent(i)%>throw
<% i-=1 %>
<%=Indent(i)%>End Try
<% end if %>
<% i-=1 %>
<%=Indent(i)%>End Using

<% if(objInfo.IsReadOnly) then%>
<%=Indent(i)%>IsReadOnly = true
<% end if %>
<%=Indent(i)%>RaiseListChangedEvents = true
<% i-=1 %>
<%=Indent(i)%>End Sub

<%=Indent(i)%><%=objInfo.LocalMethodModifiers%> Sub ExecuteFetch(<%if(objInfo.UseAdoTransaction) then%>Byval tr as SqlTransaction<%else%>Byval cn as SqlConnection<%end if%>, Byval criteria as FilterCriteria)
<% i+=1 %>
<%if(objInfo.IsGeneratedPartial) then%>
<%=Indent(i)%>if (not onExecuteFetch(<%if(objInfo.UseAdoTransaction) then%>tr<%else%>cn<%end if%>, criteria)) then return
<%end if %>
<%=Indent(i)%>Using cm as SqlCommand = <%if(objInfo.UseAdoTransaction) then%>tr.Connection<%else%>cn<%end if%>.CreateCommand()
<% i+=1 %>
<%if(objInfo.UseAdoTransaction) then%>
<%=Indent(i)%>cm.Transaction = tr
<%end if%>
<%=Indent(i)%>cm.CommandType = CommandType.StoredProcedure
<%=Indent(i)%>cm.CommandText = "<%=objInfo.FetchCommandText%>"
<%=GetFilterParameters(objInfo, i)%>

<%=Indent(i)%>Using dr as SafeDataReader = new SafeDataReader(cm.ExecuteReader())
<% i+=1 %>
<%=Indent(i)%>while (dr.Read())
<%=Indent(i+1)%>Me.Add(<%=objInfo.Child%><%if(objInfo.IsGeneratedBase) then%>(Of <%=GenericTypeChildParameter%>)<%end if%>.<%=objInfo.GetChildMethodName%>(dr))
<% i-=1 %>
<%=Indent(i)%>End While
<%if(objInfo.IsGeneratedPartial) then%>
<%=Indent(i)%>onExecuteFetchComplete(cm, criteria)
<%end if%>
<%=Indent(i)%>End Using
<% i-=1 %>
<%=Indent(i)%>End Using
<% i-=1 %>
<%=Indent(i)%>End Sub

<%=Indent(0)%>#End Region 'Data Access - Fetch
