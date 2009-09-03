<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProjectTrackerMvc.ViewModels.ProjectViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Project Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Project Detail</h2>

    <fieldset>
        <legend>Project</legend>
        
            <h3><%= Html.Encode(Model.Project.Name) %></h3>
            <div>Project starts from <%=Html.Encode(Model.Project.Started) %>
            <% if(!string.IsNullOrEmpty(Model.Project.Ended)) { %>
                to <%= Html.Encode(Model.Project.Ended) %>
            <% } %>
            </div><br />
            <div>
                <%= Html.Encode(Model.Project.Description) %>
            </div>
            
            <fieldset>
                <legend>Resources</legend>
                <ul>
                <% for (var i=0; i<Model.Project.Resources.Count; i++) {
                       var resource = Model.Project.Resources[i];
                %>
                <li>
                    <%= Html.Encode(resource.FullName) %>,
                    assigned as <%= Model.RoleList.GetItemByKey(resource.Role).Value %>,
                    on <%= Html.Encode(resource.Assigned) %>
                </li>
                <% } %>
                </ul>
                <% if(Model.Project.Resources.Count==0) { %>No resource available<% } %>
            </fieldset>
    </fieldset>
    <p>
        <%=Html.ActionLink("Edit", "Edit", new { id=Model.Project.Id }) %> |
        <%=Html.ActionLink("Back to Project List", "Index") %>
    </p>

</asp:Content>

