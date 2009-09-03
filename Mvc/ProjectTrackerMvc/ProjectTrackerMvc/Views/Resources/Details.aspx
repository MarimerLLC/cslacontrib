<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<ProjectTrackerMvc.ViewModels.ResourceViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Resource Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Resource Detail</h2>

    <fieldset>
        <legend>Resource</legend>
        <h3><%= Html.Encode(Model.Resource.FullName) %></h3>

        <fieldset>
            <legend>Assignments</legend>
            <ul>
            <% for (var i=0; i<Model.Resource.Assignments.Count; i++) {
                   var project = Model.Resource.Assignments[i];
            %>
            <li>
                <%= Html.Encode(project.ProjectName) %>.
                Assigned as <%= Model.RoleList.GetItemByKey(project.Role).Value %>
                on <%= Html.Encode(project.Assigned) %>
            </li>
            <% } %>
            </ul>
            <% if(Model.Resource.Assignments.Count==0) { %>No project available<% } %>
        </fieldset>
    </fieldset>
    <p>
        <%=Html.ActionLink("Edit", "Edit", new { id=Model.Resource.Id }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

