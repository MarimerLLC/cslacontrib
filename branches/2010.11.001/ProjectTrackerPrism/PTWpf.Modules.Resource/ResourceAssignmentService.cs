using System;
using System.Windows;
using Microsoft.Practices.Composite.Events;
using ProjectTracker.Library;
using PTWpf.Library.Contracts;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Modules.Resource;

namespace PTWpf.Modules.Resource
{

    public class ResourceAssignmentService : IResourceAssignService
    {
        public ResourceAssignmentService(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ResourceAssignmentServiceEvent>().Subscribe(Assign);
        }

        #region IResourceAssignService Member

        public void Assign(Project project)
        {
            ResourceSelect dlg = new ResourceSelect();
            if ((bool)dlg.ShowDialog())
            {
                int id = dlg.ResourceId;
                try
                {
                    project.Resources.Assign(id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                      ex.Message,
                      "Assignment error",
                      MessageBoxButton.OK,
                      MessageBoxImage.Information);
                }
            }
        }

        #endregion
    }
}