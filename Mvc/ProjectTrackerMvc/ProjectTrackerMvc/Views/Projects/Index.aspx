<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<ProjectTracker.Library.ProjectList>" 
%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List of Projects
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List of Projects</h2>
    
    <ul>
        <% string action = Csla.Security.AuthorizationRules.CanEditObject(typeof (ProjectTracker.Library.Project)) ?
                            "Edit" : "Details";
           foreach (var project in Model) { %>
        <li><h3><%= Html.ActionLink(project.Name, action, new {id=project.Id}) %></h3>

            <% if ( Csla.Security.AuthorizationRules.CanDeleteObject(typeof (ProjectTracker.Library.Project))) { %> 
                <% using (Html.BeginForm("Delete", "Projects", new { id = project.Id }, FormMethod.Post, new { @style="display:inline" })) { %>
                &nbsp;&nbsp;
                <a href="javascript:void(0)" style="display:none" class="delete">Delete</a>
                <noscript><input type="submit" value="Delete"/></noscript>
                <% } %>
            <% } %>
        </li>
        <% } %>
    </ul>

    <div>
        <%=Html.ActionLink("Create New Project", "Create")%>
    </div>

<script  type="text/javascript">
    $(document).ready(function() {
        $("a.delete").click(function(e) {
            e.preventDefault(); 
            $(this).parent("form").submit();
        }).show();
    });
</script>

</asp:Content>
