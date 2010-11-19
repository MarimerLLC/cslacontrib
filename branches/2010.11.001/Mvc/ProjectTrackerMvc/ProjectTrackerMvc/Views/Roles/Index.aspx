<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProjectTracker.Library.Admin.Roles>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Project Roles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Project Roles</h2>

    <ul id="role-list">
    <% foreach (var item in Model) { %>
        <li><h3 style="display:inline;"><%=Html.Encode(item.Name) %></h3><input type="hidden" value="<%=item.Id %>" />
        <% if ( Csla.Security.AuthorizationRules.CanEditObject(typeof (ProjectTracker.Library.Admin.Roles))) { %> 
            &nbsp;&nbsp;<a class="edit" href="javascript:void(0)" >Edit</a>
        <% } %>
        <% if ( Csla.Security.AuthorizationRules.CanDeleteObject(typeof (ProjectTracker.Library.Admin.Roles))) { %> 
            &nbsp;&nbsp;<a class="delete" href="javascript:void(0)" >Delete</a>
        <% } %>
        </li>   
    
    <% } %>
    </ul>

    <% if ( Csla.Security.AuthorizationRules.CanCreateObject(typeof (ProjectTracker.Library.Admin.Roles))) { %> 
    <p>
        <a id="create-new" href="javascript:void(0)" >Create New</a>
    </p>

    <div id="new-role" title="New Role" class="dialog">
        <% using(Html.BeginForm("Create", "Roles")) { %>
        <p>
            <label for="Name">Name:</label>
            <input type="text" id="new-role-name" name="Name" />
        </p>
        <p>
            <input id="create-role" type="submit" value="Create" />
        </p>
        <% } %>
    </div>
    <% } %>

    <div id="edit-role" title="Edit Role" class="dialog">
        <% using(Html.BeginForm("Edit", "Roles")) { %>
        <p>
            <label for="Name">Name:</label>
            <input type="text" id="role-name" name="Name" />
            <input type="hidden" id="role-id" name="id" />
        </p>
        <p>
            <input id="save-role" type="submit" value="Save" />
        </p>
        <% } %>
    </div>
    
    
<script type="text/html" id="item-template">
    <li><h3 style="display:inline;"></h3><input type="hidden" value="" />
    <% if ( Csla.Security.AuthorizationRules.CanEditObject(typeof (ProjectTracker.Library.Admin.Roles))) { %> 
        &nbsp;&nbsp;<a class="edit" href="javascript:void(0)" >Edit</a>
    <% } %>
    <% if ( Csla.Security.AuthorizationRules.CanDeleteObject(typeof (ProjectTracker.Library.Admin.Roles))) { %> 
        &nbsp;&nbsp;<a class="delete" href="javascript:void(0)" >Delete</a>
    <% } %>
    </li>   
</script>

<script  type="text/javascript">

    $(document).ready(function() {
        var editpnl = $("#edit-role"),
            newpnl = $("#new-role");

        $("div.dialog").dialog({ 
                            autoOpen: false, modal: true, resizable: false,
                            open: function() { $(this).find("input:text:first").focus(); }
                        })
                        .find("input:text").keypress(function(e) {
                            if (e.which == 13) $(this).parent("form").trigger("submit");
                        });

        $("form", editpnl).submit(function(e) {
            e.preventDefault();
            $.post(this.action, $(this).serialize(),
                        function(result) {
                            if (result.Success) {
                                editpnl.dialog("close");
                                $("input:hidden[value='" + result.Data.Id + "']", "#role-list").prev("h3").text(result.Data.Name);
                            }
                            else {
                                alert(result.Messages);
                            }
                        }
                );
        });

        $("a.edit").live("click", function() {
            $("#role-id").val($(this).prev("input:hidden").val());
            $("#role-name").val($(this).prev().prev("h3").text());
            editpnl.dialog("open");
        });

        var tmplt = document.getElementById("item-template").innerHTML;

        $("form", newpnl).submit(function(e) {
            e.preventDefault();
            $.post(this.action, $(this).serialize(),
                        function(result) {
                            if (result.Success) {
                                var item = $(tmplt).appendTo("#role-list");
                                $("h3", item).text(result.Data.Name);
                                $("input", item).val(result.Data.Id);

                                newpnl.dialog("close");
                            }
                            else {
                                alert(result.Messages);
                            }
                        }
                );
        });

        $("#create-new").click(function() {
            newpnl.dialog("open").find("input:text").val("");
        });

        $("a.delete").live("click", function() {
            var item = $(this).parent("li");
            $.post('<%=Url.Action("Delete", "Roles")%>', { id: $("input:hidden", item).val() },
                    function(result) {
                        if (result.Success) {
                            item.remove();
                        }
                        else {
                            alert(result.Messages);
                        }
                    }
            );
        });
    })
    .ajaxError(function(e, xhr) {
        var resp = xhr.responseText;
        if (resp && resp.charAt(0) == "{") {
            var err = JSON.parse();
            alert(err.Message);
        }
        else alert(resp);
    });

    $.ajaxSetup({
        dataFilter: function(data) {
            var r = JSON.parse(data);
            if (r.hasOwnProperty("d")) return r.d;
            return r;
        }
    });

</script>

</asp:Content>

