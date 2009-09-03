using System;

namespace CslaContrib.Mvc
{
    public interface IModelInstantiator
    {
        object CallFactoryMethod(Type factoryType, Type returnType, string factoryMethod, object[] argumentValues);

        object CallFactoryMethod(string actionName, Type factoryType, Type returnType, object[] argumentValues);
    }
}