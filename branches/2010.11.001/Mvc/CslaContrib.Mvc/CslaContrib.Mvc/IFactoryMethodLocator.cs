using System;
using System.Reflection;

namespace CslaContrib.Mvc
{
    public interface IFactoryMethodLocator
    {
        MethodInfo GetMethod(Type factoryType, Type returnType, string methodName, Type[] parameterTypes);

        MethodInfo GetMethod(Type factoryType, Type returnType, string methodName, object[] argumentValues);

        MethodInfo GetMethod(string actionName, Type factoryType, Type returnType, object[] argumentValues);
    }
}