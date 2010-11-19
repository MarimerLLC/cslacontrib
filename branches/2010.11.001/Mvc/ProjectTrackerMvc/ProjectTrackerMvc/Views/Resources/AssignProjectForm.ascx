<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ProjectTrackerMvc.ViewModels.ResourceViewModel>" %>

<% using (Html.BeginForm("AssignProject", "Resources")) { %>
    <fieldset>
        <%= Html.Hidden("Id", Model.Resource.Id) %>
        <p>
            <label for="Name">Name:</label>
            <%= Html.DropDownList("ProjectID", Model.GetProjectSelectList())%>
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