<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ProjectTrackerMvc.ViewModels.ProjectViewModel>" %>

<% using (Html.BeginForm("AssignResource", "Projects")) { %>
    <fieldset>
        <%= Html.Hidden("Id", Model.Project.Id) %>
        <p>
            <label for="Name">Name:</label>
            <%= Html.DropDownList("ResourceID", Model.GetResourceSelectList())%>
        </p>
        <p>
            <label for="Role">Role:</label>
            <%= Html.DropDownList("Role", Model.GetRoleSelectList())%>
        </p>
        
    </fieldset>
    <p>
        <input type="submit" value="Assign" />
    </p>
<% } %>


