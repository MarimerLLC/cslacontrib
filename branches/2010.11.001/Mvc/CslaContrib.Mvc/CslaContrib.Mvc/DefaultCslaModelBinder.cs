using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Collections;
using System.Linq;

namespace CslaContrib.Mvc
{
    public class DefaultCslaModelBinder : DefaultModelBinder
    {
        protected readonly IModelInstantiator _instantiator;

        public DefaultCslaModelBinder() : this (new ModelInstantiator()) {  }

        public DefaultCslaModelBinder(IModelInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //if not csla object, let base method handle it
            return typeof(Csla.Core.IBusinessObject).IsAssignableFrom(bindingContext.ModelType) ? 
                    BindCslaModel(controllerContext, bindingContext) : 
                    base.BindModel(controllerContext, bindingContext);
        }
        
        protected virtual object BindCslaModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // BindComplexModel should be made overridable so the following code is more appropriate there.
            object model = bindingContext.Model ?? CreateCslaModel(controllerContext, bindingContext, bindingContext.ModelType);

            // We also need to repeat the fallback to empty prefix (ModelName) before continue
            bool usePrefix = true;
            if (!string.IsNullOrEmpty(bindingContext.ModelName) && !DictionaryHelpers.DoesAnyKeyHavePrefix(bindingContext.ValueProvider, bindingContext.ModelName))
            {
                if (!bindingContext.FallbackToEmptyPrefix) return null;
                usePrefix = false;
            }

            var newBindingContext = new ModelBindingContext()
                                        {
                                            Model = model,
                                            ModelName = usePrefix ? bindingContext.ModelName : string.Empty,
                                            ModelState = bindingContext.ModelState,
                                            ModelType = bindingContext.ModelType,
                                            PropertyFilter = bindingContext.PropertyFilter,
                                            ValueProvider = bindingContext.ValueProvider
                                        };


            if (typeof(Csla.Core.IEditableCollection).IsAssignableFrom((bindingContext.ModelType)))
                return BindCslaCollectionModel(controllerContext, newBindingContext);

            // bind IEditableBusinessObject
            BindProperties(controllerContext, newBindingContext);
            return model;
        }

        private void BindProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            PropertyDescriptorCollection properties = GetModelProperties(controllerContext, bindingContext);
            foreach (PropertyDescriptor property in properties)
            {
                BindProperty(controllerContext, bindingContext, property);
            }
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (typeof(Csla.Core.IEditableCollection).IsAssignableFrom((bindingContext.ModelType)))
                return;

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        private object BindCslaCollectionModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var collection = (IList)bindingContext.Model;

            for (int idx = 0; ; idx++)
            {
                var subIndexKey = CreateSubIndexName(bindingContext.ModelName, idx);

                // TODO: investigagte when sub index not exist but element model exist, if this model should be deleted
                // if no more value element to work with, exit.
                if(!DictionaryHelpers.DoesAnyKeyHavePrefix(bindingContext.ValueProvider, subIndexKey)) break;

                // TODO: investigate way to append new element in BindCslaCollectionModel
                // for now, we're done when no collection element to update
                if (idx >= collection.Count) break;

                object elementModel = collection[idx];
                var innerContext = new ModelBindingContext()
                                        {
                                            Model = elementModel,
                                            ModelName = subIndexKey,
                                            ModelState = bindingContext.ModelState,
                                            ModelType = elementModel.GetType(),
                                            PropertyFilter = bindingContext.PropertyFilter,
                                            ValueProvider = bindingContext.ValueProvider
                                        };

                BindProperties(controllerContext, innerContext);
            }
            return bindingContext.Model;
        }


        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return typeof(Csla.Core.IBusinessObject).IsAssignableFrom(bindingContext.ModelType) ? 
                CreateCslaModel(controllerContext, bindingContext, modelType) :
                base.CreateModel(controllerContext, bindingContext, modelType);
        }

        protected virtual object CreateCslaModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            //try to collect arguments within action method and make them arguments to factory method
            //ex: Edit(Guid id, Project project) => Project.GetProject(id)
            
            var argValues = new List<object>();
            foreach (var state in bindingContext.ModelState)
                argValues.Add(new[] { state.Value.Value.RawValue });
            
            string action = controllerContext.RouteData.GetRequiredString("action");

            //call factory method by convention
            return _instantiator.CallFactoryMethod(action, modelType, modelType, argValues.Count>0 ? argValues.ToArray(): null);
        }

    }
}
