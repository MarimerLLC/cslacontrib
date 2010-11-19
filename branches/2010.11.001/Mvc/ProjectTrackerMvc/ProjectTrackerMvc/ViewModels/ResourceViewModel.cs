using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectTracker.Library;
using System.Web.Mvc;

namespace ProjectTrackerMvc.ViewModels
{
    public class ResourceViewModel
    {
        public Resource Resource { get; set; }

        public RoleList RoleList { get; set; }
        
        public ProjectList ProjectList { get; set; }

        public SelectList GetProjectSelectList()
        {
            return new SelectList(ProjectList, "ID", "Name");
        }

        public SelectList GetRoleSelectList()
        {
            return new SelectList(RoleList, "Key", "Value");
        }

        public SelectList GetRoleSelectList(object selectedValue)
        {
            return new SelectList(RoleList, "Key", "Value", selectedValue);
        }

        public ResourceViewModel(Resource resource, RoleList roleList, ProjectList projectList)
        {
            Resource = resource;
            RoleList = roleList;
            ProjectList = projectList;
        }
    }
}
