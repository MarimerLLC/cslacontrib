<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProjectTrackerMvc.ViewModels.ResourceViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit <%= Model.Resource.FullName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit <%= Model.Resource.FullName %></h2>
    
    <% Html.RenderPartial("ResourceForm"); %>

</asp:Content>
