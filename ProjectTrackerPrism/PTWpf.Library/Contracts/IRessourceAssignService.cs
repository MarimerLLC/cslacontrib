using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectTracker.Library;

namespace PTWpf.Library.Contracts
{
    public interface IResourceAssignService
    {
        void Assign(Project resource);
    }
}
