using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CslaContrib.Mvc
{
    internal class ModelInstantiator : IModelInstantiator
    {

        public object CallFactoryMethod(Type factoryType, Type returnType, string factoryMethod, object[] argumentValues)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType", "Must have return type to call a factory method");
            if (string.IsNullOrEmpty(factoryMethod))
                throw new ArgumentNullException("factoryMethod", "Must have method name to call a factory method");

            var argTypes = argumentValues == null ? Type.EmptyTypes : argumentValues.GetTypes().ToArray();
            MethodInfo method = null;
            if(argTypes.Any(t=>t.IsArray))
            {
                //argument type contain object[], which indicates it contain unknown type
                //1. find methods with equal number of arguments
                //2. determine which ones contain assignable arguments; also convert value
                var methods = FindFactoryMethods(factoryType, factoryMethod, argTypes.Length, returnType);
                if(argumentValues != null)
                    methods = FindMethodsWithMatchingArguments(methods, argumentValues);

                if(methods!= null && methods.Length >1)
                {
                    var methodNames = Array.ConvertAll(methods, m => m.Name);
                    var message = "Unable to find factory method. "
                                  + string.Format("There are multiple factory methods found: {0}. ", string.Join(", ", methodNames))
                                  + "Please use CslaBind attribute to specify the intended method.";
                    throw new AmbiguousMatchException(message);                    
                }
                if(methods != null && methods.Length == 1)
                    method = methods[0];
            }
            else
            {
                method = GetFactoryMethod(factoryType, factoryMethod, argTypes, returnType);                
            }

            if (method == null)
                throw new InvalidOperationException(string.Format("Unable to locate CSLA factory method '{0}.{1}'", factoryType, factoryMethod));

            return CallFactoryMethod(factoryType, method, argumentValues);
        }

        public object CallFactoryMethod(string actionName, Type factoryType, Type returnType, object[] argumentValues)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType", "Must have return type to call a factory method");
            if (string.IsNullOrEmpty(actionName))
                throw new ArgumentNullException("actionName", "Must have action name to find then call a factory method");

            var argTypes = argumentValues == null ? Type.EmptyTypes : argumentValues.GetTypes().ToArray();

            //1. by convention, find methods with equal number of arguments based on action name
            //2. determine which ones contain assignable arguments; also convert value if value is unknown type
            var methods = FindFactoryMethodsByConvention(actionName, factoryType, argTypes.Length, returnType);
            if(argumentValues != null)
                methods = FindMethodsWithMatchingArguments(methods, argumentValues);

            if (methods == null || methods.Length == 0)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to locate CSLA factory method of type '{0}' for action method '{1}'. "
                                + "Please use CslaBind attribute to specify the intended method." ,
                                factoryType, actionName));
            }

            if (methods.Length > 1)
            {
                var methodNames = Array.ConvertAll(methods, m => m.Name);
                var message = "Unable to find factory method by convention. "
                              + string.Format("There are multiple factory methods found: {0}. ", string.Join(", ", methodNames))
                              + "Please use CslaBind attribute to specify the intended method.";
                throw new AmbiguousMatchException(message);
            }

            return CallFactoryMethod(factoryType, methods[0], argumentValues);
        }

        public object CallFactoryMethod(Type factoryType, MethodInfo method, object[] argumentValues)
        {
            object obj = null;
            if (!method.IsStatic)
                obj = Activator.CreateInstance(factoryType);

            return method.Invoke(obj, argumentValues);
        }

        BindingFlags ftorFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        BindingFlags ftorMethodFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private MethodInfo GetFactoryMethod(Type factoryType, string methodName, Type[] parameterTypes, Type returnType)
        {
            if (factoryType == null) factoryType = returnType;
            if (parameterTypes == null) parameterTypes = Type.EmptyTypes;

            //might use separate factory class
            BindingFlags flags = factoryType == returnType ? ftorMethodFlags : ftorFlags;

            var mtd = factoryType.GetMethod(methodName, flags, null, parameterTypes, null);
            if (mtd == null || mtd.ReturnType != returnType)
                return null;

            return mtd;
        }

        private MethodInfo[] FindFactoryMethods(Type factoryType, string methodName, int parameterCount, Type returnType)
        {
            return (from m in FindFactoryMethods(factoryType, parameterCount, returnType)
                   where m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase)
                   select m).ToArray();
        }

        private IEnumerable<MethodInfo> FindFactoryMethods(Type factoryType, int parameterCount, Type returnType)
        {
            if (factoryType == null || factoryType == returnType)
            {
                //use factory method
                return from m in returnType.GetMethods(ftorMethodFlags)
                        where m.GetParameters().Count() == parameterCount
                              && m.ReturnType == returnType
                              && m.IsStatic
                        select m;
            }

            //use separate factory class
            return from m in factoryType.GetMethods(ftorFlags)
                    where m.GetParameters().Count() == parameterCount
                          && m.ReturnType == returnType
                    select m;            
        }

        public static readonly IList<PatternMap> MapPatterns = new List<PatternMap> {
                                        new PatternMap{ActionPattern = "create.*", MethodPatterns = new[] {"New.*"}}, 
                                        new PatternMap{ActionPattern = "add.*", MethodPatterns = new[] {"New.*"}},
                                        new PatternMap{ActionPattern = "edit.*", MethodPatterns = new[] {"Get.*"}},
                                        new PatternMap{ActionPattern = "detail.*", MethodPatterns = new[] {"Get.*"}},
                                        new PatternMap{ActionPattern = "update.*", MethodPatterns = new[] {"Get.*"}},
                                        new PatternMap{ActionPattern = "show.*", MethodPatterns = new[] {"Get.*"}},
                                        new PatternMap{ActionPattern = "index.*", MethodPatterns = new[] {"Get.*List", "Get.*Collection"}},
                                        new PatternMap{ActionPattern = "list.*", MethodPatterns = new[] {"Get.*List", "Get.*Collection"}}
                                    };

        private MethodInfo[] FindFactoryMethodsByConvention(string actionName, Type factoryType, int parameterCount, Type returnType)
        {
            if (factoryType == null) factoryType = returnType;

            var patterns = from m in MapPatterns
                          where Regex.Match(actionName, m.ActionPattern, RegexOptions.IgnoreCase).Success
                          select m;

            var candidates = new List<MethodInfo>();

            foreach (MethodInfo method in FindFactoryMethods(factoryType, parameterCount, returnType))
            {
                var methodName = method.Name;
                var result = from a in patterns
                        where a.MethodPatterns.Any(p => Regex.Match(methodName, p, RegexOptions.IgnoreCase).Success)
                        select a;

                if (result.Count() == 0) continue;

                candidates.Add(method);
            }

            return candidates.ToArray();
        }

        private MethodInfo[] FindMethodsWithMatchingArguments(MethodInfo[] methods, object[] argumentValues)
        {
            if (methods == null) return null;

            var matches = new List<MethodInfo>();
            foreach (MethodInfo method in methods)
            {
                //make sure all argument types are match
                bool match = true;
                var pis = method.GetParameters();
                for (int i = 0; i < argumentValues.Length; i++)
                {
                    var param = pis[i];
                    var argValue = argumentValues[i];

                    if(argValue.GetType().IsArray)
                    {
                        //argument type is unknown, check if assignable
                        var paramType = param.ParameterType;
                        argValue = ((IList)argValue)[0];
                        object convertedValue;
                        if(TryConvertArgument(paramType, argValue, out convertedValue))
                        {
                            argumentValues[i] = convertedValue;
                            continue;
                        }
                        //not match
                        match = false;
                        break;
                    }

                    if (!param.ParameterType.IsAssignableFrom(argValue.GetType()))
                    {
                        match = false;
                        break;
                    }
                }
                if (match) 
                    matches.Add(method);
            }
            return matches.ToArray();            
        }

        private bool TryConvertArgument(Type type, object value, out object convertedValue)
        {
            if(type.IsAssignableFrom(value.GetType()))
            {
                convertedValue = value;
                return true;
            }
            var converter = TypeDescriptor.GetConverter(value);
            if (converter != null && converter.CanConvertTo(type))
            {
                //convert argument value
                convertedValue = converter.ConvertTo(value, type);
                return true;
            }
            converter = TypeDescriptor.GetConverter(type);
            if (converter != null && converter.CanConvertFrom(value.GetType()))
            {
                try
                {
                    //convert argument value
                    convertedValue = converter.ConvertFrom(value);
                    return true;
                }
                catch { /* not convertible, continue..*/ }
            }
            try
            {
                convertedValue = Convert.ChangeType(value, type);
                return true;
            }
            catch { /* not convertible, continue..*/ }

            // no possible conversion
            convertedValue = null;
            return false;
        }
    }
}
