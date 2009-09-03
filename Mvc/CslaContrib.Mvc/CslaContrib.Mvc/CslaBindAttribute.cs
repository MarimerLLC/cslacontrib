using System;
using System.Web.Mvc;

namespace CslaContrib.Mvc
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class CslaBindAttribute : CustomModelBinderAttribute
    {
        /// <summary>
        /// CSLA object factory to use when creating CSLA object binding for action argument.
        /// Provide the type when using object factory class instead of static methods.
        /// </summary>
        public Type FactoryType { get; set; }

        /// <summary>
        /// The factory method name for CSLA object
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Comma delimited names to be passed as arguments to the factory method.
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Comma delimited property names to be excluded when binding to action argument
        /// </summary>
        public string Exclude { get; set; }
        
        /// <summary>
        /// Comma delimited property names to be included when binding to action argument
        /// </summary>
        public string Include { get; set; }
        
        /// <summary>
        /// Prefix to use when binding to action argument
        /// </summary>
        public string Prefix { get; set; }

        public override IModelBinder GetBinder()
        {
            return new CslaBindModelBinder(this);
        }

    }
}
