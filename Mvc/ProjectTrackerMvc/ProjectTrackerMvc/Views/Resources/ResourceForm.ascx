<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ResourceViewModel>" %>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {
        var resource = Model.Resource;
           %>

        <fieldset>
            <legend>Resource</legend>
            <p>
                <label for="LastName">LastName:</label>
                <%= Html.TextBox("LastName", resource.LastName) %>
                <%= Html.ValidationMessage("LastName", "*") %>
            </p>
            <p>
                <label for="FirstName">FirstName:</label>
                <%= Html.TextBox("FirstName", resource.FirstName) %>
                <%= Html.ValidationMessage("FirstName", "*") %>
            </p>
            
            <fieldset>
                <legend>Assigned Projects</legend>
                <% for (var i=0; i<resource.Assignments.Count; i++) {
                        var project = resource.Assignments[i];
                %>
                <p>
                    <%= Html.Encode(project.ProjectName) %>.
                    Assigned as <%= Html.DropDownList(string.Format("Assignments[{0}].Role", i), Model.GetRoleSelectList(project.Role)) %>
                    on <%= Html.Encode(project.Assigned) %>.
                    &nbsp;&nbsp;
                    <%= Html.ActionLink("Remove", "RemoveAssignment", new { id=resource.Id, projectid=project.ProjectId }) %>
                </p>
                <% } %>
                
                <% if (!resource.IsNew) { %>
                <div>
                    <a id="assign-project" href="javascript:void(0)" >Assign Project</a>
                </div>
                
                <% } %>
            </fieldset>

            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

    <div id="assign-project-panel" title="Assign Project">
        <% Html.RenderPartial("AssignProjectForm"); %>
    </div>

<script  type="text/javascript">
    $(document).ready(function() {
        var pnl = $("#assign-project-panel").dialog({ autoOpen: false, modal: true, resizable: false, width:500 });
        $("#assign-project").click(function() { pnl.dialog("open"); });
    });
    
</script>

