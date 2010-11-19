using System;
using Microsoft.Practices.Composite.Presentation.Events;
using ProjectTracker.Library;
using ProjectTracker.Library.Admin;
using Csla.Wpf;

namespace PTWpf.Modules.ModuleEvents
{

    public class StatusbarMessageEvent : CompositePresentationEvent<string>
    {
    }

    public class NewResourceAddedEvent : CompositePresentationEvent<object>
    {
    }

    public class NewProjectAddedEvent : CompositePresentationEvent<object>
    {
    }

    public class ApplyAuthorizationEvent : CompositePresentationEvent<ObjectStatus>
    {
    }

    public class ProjectAssignmentServiceEvent : CompositePresentationEvent<Resource>
    {
    }
    public class ResourceAssignmentServiceEvent : CompositePresentationEvent<Project>
    {
    }
    public class OpenProjectByIdEvent : CompositePresentationEvent<Guid>
    {
    }
    public class OpenResourceByIdEvent : CompositePresentationEvent<int>
    {
    }
}
