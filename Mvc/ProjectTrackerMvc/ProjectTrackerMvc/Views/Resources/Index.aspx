<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<ProjectTracker.Library.ResourceList>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List of Resources
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List of Resources</h2>

    <ul>
        <% string action = Csla.Security.AuthorizationRules.CanEditObject(typeof (ProjectTracker.Library.Resource)) ?
                            "Edit" : "Details";
           foreach (var resource in Model) { %>
        <li><h3><%= Html.ActionLink(resource.Name, action, new {id=resource.Id}) %></h3>

            <% if ( Csla.Security.AuthorizationRules.CanDeleteObject(typeof (ProjectTracker.Library.Resource))) { %> 
                <% using (Html.BeginForm("Delete", "Resources", new { id = resource.Id }, FormMethod.Post, new { @style="display:inline" })) { %>
                &nbsp;&nbsp;
                <a href="javascript:void(0)" style="display:none" class="delete">Delete</a>
                <noscript><input type="submit" value="Delete"/></noscript>
                <% } %>
            <% } %>
        </li>
        <% } %>
    </ul>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

<script  type="text/javascript">
    $(document).ready(function() {
        $("a.delete").click(function(e) {
            e.preventDefault(); 
            $(this).parent("form").submit();
        }).show();
    });
</script>

</asp:Content>

