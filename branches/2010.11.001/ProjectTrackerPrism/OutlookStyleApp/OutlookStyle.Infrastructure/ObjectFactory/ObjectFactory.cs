using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace OutlookStyle.Infrastructure
{
    /// <summary>
    /// Class that allows delayed creation of an object, but still makes it very clear that you have a dependency on that object type. 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ObjectFactory<TValue>  : IObjectFactory
        where TValue : class
    {
        private readonly IServiceLocator serviceLocator;
        private TValue value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactory&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator that's used to create the objects.</param>
        public ObjectFactory(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Create an instance of the specified class and retuyrns it. If the instance was already created, it will return that instance.  
        /// </summary>
        /// <returns></returns>
        public TValue CreateInstance()
        {
            if (this.value != null)
                return this.value;

            return this.value = this.serviceLocator.GetInstance<TValue>();
        }

        /// <summary>
        /// The value of the created object
        /// </summary>
        /// <value></value>
        public TValue Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OutlookStyle.Infrastructure.ObjectFactory&lt;TValue&gt;"/> to <see cref="TValue"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TValue(ObjectFactory<TValue> factory)
        {
            if (factory == null) 
                throw new ArgumentNullException("factory");

            return factory.value;
        }

        #region IObjectFactory Members

        // These methods have been implicitly implemented, they are needed for the non-generic interface, but not if you have a reference
        // to the direct type. 

        /// <summary>
        /// The value of the created object
        /// </summary>
        /// <value></value>
        object IObjectFactory.Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Create the specified object.
        /// </summary>
        /// <returns></returns>
        object IObjectFactory.CreateInstance()
        {
            return this.CreateInstance();
        }

        #endregion
    }
}