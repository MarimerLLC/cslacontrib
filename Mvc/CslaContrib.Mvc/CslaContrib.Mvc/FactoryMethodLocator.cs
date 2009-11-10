using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CslaContrib.Mvc
{
    public class FactoryMethodLocator : IFactoryMethodLocator
    {
        public static readonly IList<PatternMap> PatternMappings = new List<PatternMap> {
             new PatternMap{ActionPattern = "create.*", MethodPatterns = new[] {"New.*"}}, 
             new PatternMap{ActionPattern = "add.*", MethodPatterns = new[] {"New.*"}},
             new PatternMap{ActionPattern = "edit.*", MethodPatterns = new[] {"Get.*"}},
             new PatternMap{ActionPattern = "detail.*", MethodPatterns = new[] {"Get.*"}},
             new PatternMap{ActionPattern = "update.*", MethodPatterns = new[] {"Get.*"}},
             new PatternMap{ActionPattern = "show.*", MethodPatterns = new[] {"Get.*"}},
             new PatternMap{ActionPattern = "index.*", MethodPatterns = new[] {"Get.*List", "Get.*Collection"}},
             new PatternMap{ActionPattern = "list.*", MethodPatterns = new[] {"Get.*List", "Get.*Collection"}}
         };

        public MethodInfo GetMethod(Type factoryType, Type returnType, string methodName, Type[] parameterTypes)
        {
            if (factoryType == null) 
                factoryType = returnType;
            if (parameterTypes == null) 
                parameterTypes = Type.EmptyTypes;

            //might use separate factory class
            BindingFlags flags = factoryType == returnType ? ftorMethodFlags : ftorFlags;

            var method = factoryType.GetMethod(methodName, flags, null, parameterTypes, null);

            if (method == null || method.ReturnType != returnType)
                return null;

            return method;
        }

        public MethodInfo GetMethod(Type factoryType, Type returnType, string methodName, object[] argumentValues)
        {
            var argTypes = argumentValues == null ? Type.EmptyTypes : argumentValues.GetTypes().ToArray();

            if (!argTypes.Any(t => t.IsArray))
                return GetMethod(factoryType, returnType, methodName, argTypes);

            //argument type contain object[] which indicate it contains unknown type
            //1. find matching methods given method name and number of arguments
            //2. determine which ones contain assignable arguments; also convert value
            var methods = FindMatchingMethods(factoryType, returnType, methodName, argTypes.Length);
            methods = FindMethodsWithMatchingArguments(methods, argumentValues);

            if (methods == null || methods.Length == 0)
                return null;

            if (methods.Length > 1)
            {
                var methodNames = Array.ConvertAll(methods, m => m.Name);
                var message = "Unable to find factory method. "
                              + string.Format("There are multiple factory methods found: {0}. ", string.Join(", ", methodNames))
                              + "Please use CslaBind attribute to specify the intended method.";
                throw new AmbiguousMatchException(message);
            }

            return methods[0];
        }

        public MethodInfo GetMethod(string actionName, Type factoryType, Type returnType, object[] argumentValues)
        {
            var argTypes = argumentValues == null ? Type.EmptyTypes : argumentValues.GetTypes().ToArray();

            //1. find matching methods with equal number of arguments based on action name
            //2. determine which ones contain assignable arguments; also convert value if value is unknown type
            var methods = FindMatchingMethods(actionName, factoryType, returnType, argTypes.Length);
            methods = FindMethodsWithMatchingArguments(methods, argumentValues);

            if (methods == null || methods.Length == 0)
                return null;

            if (methods.Length > 1)
            {
                var methodNames = Array.ConvertAll(methods, m => m.Name);
                var message = "Unable to find factory method by convention. "
                              + string.Format("There are multiple factory methods found: {0}. ", string.Join(", ", methodNames))
                              + "Please use CslaBind attribute to specify the intended method.";
                throw new AmbiguousMatchException(message);
            }
            return methods[0];
        }
        protected MethodInfo[] FindMatchingMethods(Type factoryType, Type returnType, string methodName, int parameterCount)
        {
            return (from m in AllFactoryMethods(factoryType, returnType, parameterCount)
                    where m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase)
                    select m).ToArray();
        }

        protected MethodInfo[] FindMatchingMethods(string actionName, Type factoryType, Type returnType, int parameterCount)
        {
            if (factoryType == null) 
                factoryType = returnType;

            return (from m in AllFactoryMethods(factoryType, returnType, parameterCount)
                    let patterns = from p in PatternMappings
                                   where Regex.Match(actionName, p.ActionPattern, RegexOptions.IgnoreCase).Success
                                   select p
                    where patterns.Any(mp =>
                                        mp.MethodPatterns.Any(p => 
                                            Regex.Match(m.Name, p, RegexOptions.IgnoreCase).Success))
                    select m).ToArray();
        }

        BindingFlags ftorFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        BindingFlags ftorMethodFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        protected IEnumerable<MethodInfo> AllFactoryMethods(Type factoryType, Type returnType, int parameterCount)
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

            //use object factory class
            return from m in factoryType.GetMethods(ftorFlags)
                   where m.GetParameters().Count() == parameterCount
                         && m.ReturnType == returnType
                   select m;
        }

        protected virtual MethodInfo[] FindMethodsWithMatchingArguments(MethodInfo[] methods, object[] argumentValues)
        {
            if (methods == null) return null;
            if(argumentValues == null) return methods;

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

                    if (argValue.GetType().IsArray)
                    {
                        //argument type is unknown, check if assignable
                        var paramType = param.ParameterType;
                        argValue = ((IList)argValue)[0];
                        object convertedValue;
                        if (TryConvertArgument(paramType, argValue, out convertedValue))
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

        protected virtual bool TryConvertArgument(Type type, object value, out object convertedValue)
        {
            if (type.IsAssignableFrom(value.GetType()))
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