<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<ProjectTrackerMvc.ViewModels.ProjectViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Project - <%= Html.Encode(Model.Project.Name) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit <%= Html.Encode(Model.Project.Name) %></h2>
    
    <% Html.RenderPartial("ProjectForm"); %>

</asp:Content>
