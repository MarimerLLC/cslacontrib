using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Composite.Regions;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;

namespace OutlookStyle.Infrastructure.ApplicationModel
{
    /// <summary>
    ///     The IApplicationModel provides an interface to the rest of the application on how the application interacts with use cases. 
    /// </summary>
    public interface IApplicationModel
    {
        /// <summary>
        /// Add a main usecase to the list of main usecases
        /// </summary>
        /// <param name="activeAwareUseCaseController"></param>
        void AddMainUseCase(IActiveAwareUseCaseController activeAwareUseCaseController);

        /// <summary>
        /// The list of main use cases
        /// </summary>
        IViewsCollection MainUseCases { get; }

        /// <summary>
        /// Activate a usecase. The usecase to activate should be in the command parameter
        /// </summary>
        ICommand ActivateUseCaseCommand { get; }

        /// <summary>
        /// Method that allows you to activate a usecase. 
        /// </summary>
        /// <param name="useCase"></param>
        void ActivateUseCase(IActiveAwareUseCaseController useCase);

        /// <summary>
        /// Show a usecase that's not a main usecase. Still work in progress
        /// </summary>
        /// <param name="useCase"></param>
        /// <returns></returns>
        void ShowUseCase(IActiveAwareUseCaseController useCase);

        /// <summary>
        /// Creates an object within a scoped regionmanager. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CreateObjectInScopedRegionManager<T>()
            where T : IRegionManagerAware;

    }
}