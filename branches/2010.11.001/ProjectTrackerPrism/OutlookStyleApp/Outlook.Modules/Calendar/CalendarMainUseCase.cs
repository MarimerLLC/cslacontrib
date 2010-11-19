using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Regions;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace Outlook.Modules.Calendar
{
    /// <summary>
    ///     Use case that handles most 'main' calendar things like showing the main calendar and some interactions on that. 
    /// </summary>
    public class CalendarMainUseCase : ActiveAwareUseCaseController
    {
        private CalendarMainViewModel viewModel;

        public CalendarMainUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder, 
            // Get the factory that will create the viewmodel
            ObjectFactory<CalendarMainViewModel> calendarMainViewFactory): base(viewtoToRegionBinder)
        {
            // When the usecase get's activated for the first time
            AddInitializationMethods(
                // Create the viewmodel and assign it to this variable
                () => this.viewModel = calendarMainViewFactory.CreateInstance());

        }

        public override string Name
        {
            get { return "Calendar"; }
        }

        protected override void OnFirstActivation()
        {
            // Make sure the viewmodel will be added to the MainRegion when this use case
            // is activated and removed again when this usecase is deactivated.
            this.ViewToRegionBinder.Add("MainRegion", viewModel);
            // NB. in the CalendarModule, a visualization for this viewmodel is registered, so the region knows which
            // view to display for this viewmodel. 
        }



    }
}
