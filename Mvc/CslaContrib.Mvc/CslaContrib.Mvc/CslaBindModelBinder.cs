using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CslaContrib.Mvc
{
    class CslaBindModelBinder : DefaultCslaModelBinder
    {
        public CslaBindAttribute BindCriteria { get; private set; }

        public CslaBindModelBinder(CslaBindAttribute bindCriteria) 
        {
            BindCriteria = bindCriteria;
        }

        public CslaBindModelBinder(CslaBindAttribute bindCriteria, IModelInstantiator instantiator) : base(instantiator)
        {
            BindCriteria = bindCriteria;
        }

        // add property filter described by BindAttribute, override prefix
        protected override object BindCslaModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (string.IsNullOrEmpty(BindCriteria.Include) && string.IsNullOrEmpty(BindCriteria.Exclude) && string.IsNullOrEmpty(BindCriteria.Prefix))
                return base.BindCslaModel(controllerContext, bindingContext);

            Predicate<string> propFilter = bindingContext.PropertyFilter;
            if (!string.IsNullOrEmpty(BindCriteria.Include) || !string.IsNullOrEmpty(BindCriteria.Exclude))
            {
                var bindAttr = new BindAttribute() { Exclude = BindCriteria.Exclude, Include = BindCriteria.Include };
                propFilter = (propName) => bindAttr.IsPropertyAllowed(propName)
                                                                && bindingContext.PropertyFilter(propName);
            }

            var newPrefix = BindCriteria.Prefix ?? bindingContext.ModelName;

            var newBindingContext = new ModelBindingContext()
                                        {
                                            Model = bindingContext.Model,
                                            ModelName = newPrefix,
                                            ModelState = bindingContext.ModelState,
                                            ModelType = bindingContext.ModelType,
                                            PropertyFilter = propFilter,
                                            ValueProvider = bindingContext.ValueProvider
                                        };

            return base.BindCslaModel(controllerContext, newBindingContext);
        }

        // use factory method described by BindAttribute
        protected override object CreateCslaModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (BindCriteria == null) 
                return base.CreateCslaModel(controllerContext, bindingContext, modelType);
            if(BindCriteria.FactoryType == null && string.IsNullOrEmpty(BindCriteria.Method) && string.IsNullOrEmpty(BindCriteria.Arguments))
                return base.CreateCslaModel(controllerContext, bindingContext, modelType);

            // use factoryType + factoryMethod (arguments) to find factory method
            var factoryType = BindCriteria.FactoryType ?? bindingContext.ModelType;
            var factoryMethod = BindCriteria.Method;

            var argNames = string.IsNullOrEmpty(BindCriteria.Arguments) ? null :
                            BindCriteria.Arguments.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var argValues = GetArgumentValues(bindingContext, argNames);

            if (string.IsNullOrEmpty(factoryMethod))
            {
                //factory method is not defined
                //pass action method where factory method will be located by finding best match based on convention
                string action = controllerContext.RouteData.GetRequiredString("action");
                return _instantiator.CallFactoryMethod(action, modelType, bindingContext.ModelType, argValues);
            }

            return _instantiator.CallFactoryMethod(factoryType, modelType, factoryMethod, argValues);
        }

        private object[] GetArgumentValues(ModelBindingContext bindingContext, string[] arguments)
        {
            if (arguments == null) return new object[0];

            var modelType = bindingContext.ModelType;
            var parValues = new object[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                var argName = arguments[i];

                //look for argument type first in model
                //when argument not found in model, just defaulted to type object
                var argType = modelType.GetPropertyType(argName) ?? typeof(object);

                object parValue = GetArgumentValue(bindingContext, argType, argName);

                if (parValue == null)
                    throw new ArgumentOutOfRangeException(string.Format(
"Unable to get value for arguments[{0}]: '{1}'. Suggestion: Name the argument according to form element's name or route parameter's name.", 
                        i, argName));

                parValues[i] = parValue;
            }
            return parValues;
        }

        private object GetArgumentValue(ModelBindingContext bindingContext, Type argumentType, string argumentName)
        {
            IModelBinder converter = ModelBinders.Binders.GetBinder(argumentType);

            //set model provider to null, so binder will only use ValueProvider which look from route data, query string, or form
            var argContext = new ModelBindingContext()
                                {
                                    ModelName = argumentName,
                                    ModelType = argumentType,
                                    ValueProvider = bindingContext.ValueProvider
                                };

            return converter.BindModel(null, argContext);
        }

    }
}
