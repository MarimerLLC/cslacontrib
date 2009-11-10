using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CslaContrib.Mvc
{
    internal class ModelInstantiator : IModelInstantiator
    {
        private IFactoryMethodLocator _locator;

        public ModelInstantiator() : this(null)
        { }

        public ModelInstantiator(IFactoryMethodLocator factoryMethodLocator)
        {
            _locator = factoryMethodLocator ?? new FactoryMethodLocator();
        }

        public object CallFactoryMethod(Type factoryType, Type returnType, string factoryMethod, object[] argumentValues)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType", "Must have return type to call a factory method");
            if (string.IsNullOrEmpty(factoryMethod))
                throw new ArgumentNullException("factoryMethod", "Must have method name to call a factory method");

            var method = _locator.GetMethod(factoryType, returnType, factoryMethod, argumentValues);

            if (method == null)
                throw new InvalidOperationException(
                    string.Format("Unable to locate CSLA factory method '{0}.{1}'", factoryType, factoryMethod));

            return CallFactoryMethod(factoryType, method, argumentValues);
        }

        public object CallFactoryMethod(string actionName, Type factoryType, Type returnType, object[] argumentValues)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType", "Must have return type to call a factory method");
            if (string.IsNullOrEmpty(actionName))
                throw new ArgumentNullException("actionName", "Must have action name to call a factory method");

            var method = _locator.GetMethod(actionName, factoryType, returnType, argumentValues);

            if (method == null)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to locate CSLA factory method of type '{0}' for action method '{1}'. "
                                + "Please use CslaBind attribute to specify the intended method.",
                                factoryType, actionName));
            }

            return CallFactoryMethod(factoryType, method, argumentValues);
        }

        public object CallFactoryMethod(Type factoryType, MethodInfo method, object[] argumentValues)
        {
            object obj = null;
            if (!method.IsStatic)
                obj = Activator.CreateInstance(factoryType);

            return method.Invoke(obj, argumentValues);
        }

    }
}
