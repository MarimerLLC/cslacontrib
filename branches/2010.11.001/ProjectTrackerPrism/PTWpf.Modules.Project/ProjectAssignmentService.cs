using System;
using System.Windows;
using Microsoft.Practices.Composite.Events;
using ProjectTracker.Library;
using PTWpf.Library.Contracts;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Project.Modules;

public class ProjectAssignmentService : IProjectAssignService
{
    public ProjectAssignmentService(IEventAggregator eventAggregator)
    {
        eventAggregator.GetEvent<ProjectAssignmentServiceEvent>().Subscribe(Assign);
    }

    public void Assign(Resource resource)
    {
        ProjectSelect dlg = new ProjectSelect();
        if ((bool)dlg.ShowDialog())
        {
            Guid id = dlg.ProjectId;
            try
            {
                resource.Assignments.AssignTo(id);
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
}