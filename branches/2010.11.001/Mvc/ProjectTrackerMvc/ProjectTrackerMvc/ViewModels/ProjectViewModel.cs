using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectTracker.Library;
using Csla.Security;

namespace ProjectTrackerMvc.ViewModels
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }

        public RoleList RoleList { get; set; }

        public ResourceList ResourceList { get; set; }

        public SelectList GetResourceSelectList()
        {
            return new SelectList(ResourceList, "ID", "Name");
        }

        public SelectList GetRoleSelectList()
        { 
            return new SelectList(RoleList, "Key", "Value");
        }

        public SelectList GetRoleSelectList(object selectedValue)
        {
            return new SelectList(RoleList, "Key", "Value", selectedValue);
        }

        public ProjectViewModel(Project project, RoleList roleList, ResourceList resourceList)
        {
            Project = project;
            RoleList = roleList;
            ResourceList = resourceList;
        }
    }
}
