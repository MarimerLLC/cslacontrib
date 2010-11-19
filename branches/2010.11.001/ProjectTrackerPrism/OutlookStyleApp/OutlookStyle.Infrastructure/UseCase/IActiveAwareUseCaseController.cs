using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation;

namespace OutlookStyle.Infrastructure.UseCase
{
    /// <summary>
    /// Interface for use cases that are ActiveAwareUseCaseControllers. 
    /// </summary>
    public interface IActiveAwareUseCaseController: IActiveAware
    {
        string Name
        { 
            get;
        }
    }
}