<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ProjectTrackerMvc.ViewModels.ProjectViewModel>" %>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm())
       {
           var project = Model.Project; %>

        <fieldset>
            <legend>Project</legend>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", project.Name) %>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Started">Started:</label>
                <%= Html.TextBox("Started", project.Started, new { @class="datepicker" })%>
                <%= Html.ValidationMessage("Started", "*") %>
            </p>
            <p>
                <label for="Ended">Ended:</label>
                <%= Html.TextBox("Ended", project.Ended, new { @class = "datepicker" })%>
                <%= Html.ValidationMessage("Ended", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextArea("Description", project.Description, new { @cols = "50", @rows = "3" })%>
                <%= Html.ValidationMessage("Description", "*") %>
            </p>
            
            <fieldset>
                <legend>Assigned Resources</legend>
                <% for (var i=0; i<project.Resources.Count; i++) {
                        var resource = project.Resources[i];
                %>
                <p>
                    <%= Html.Encode(resource.FullName) %>,
                    assigned as <%= Html.DropDownList(string.Format("Resources[{0}].Role", i), Model.GetRoleSelectList(resource.Role)) %>
                    on <%= Html.Encode(resource.Assigned) %>
                    &nbsp;&nbsp;
                    <%= Html.ActionLink("Remove", "RemoveResource", new { id=project.Id, resourceid=resource.ResourceId }) %>
                </p>
                <% } %>
                
                <% if (!project.IsNew) { %>
                <div>
                    <a id="assign-resource" href="javascript:void(0)" >Assign Resource</a>
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

    <div id="assign-resource-panel" title="Assign Resource">
        <% Html.RenderPartial("AssignResourceForm"); %>
    </div>

<script  type="text/javascript">
    $(document).ready(function() {
        var pnl = $("#assign-resource-panel").dialog({ autoOpen: false, modal: true, resizable: false });
        $("#assign-resource").click(function() { pnl.dialog("open"); });
        $("input.datepicker").datepicker();
    });
    
</script>
