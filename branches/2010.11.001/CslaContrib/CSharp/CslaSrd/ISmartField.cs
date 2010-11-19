using System;
using System.Collections.Generic;
using System.Text;

namespace CslaSrd
{
    /// <summary>
    /// This interface is used make it easier to write generic rules against the various Smart datatype classes.
    /// All the Smart classes (except SmartDate) implement this interface.
    /// </summary>
    public interface ISmartField
    {
        /// <summary>
        /// A method, rather than a property, that equates to the IsEmpty property.  
        /// (Properties cannot be specified in an interface, neither can static methods.)
        /// </summary>
        /// <returns></returns>
        bool HasNullValue();
    }
}
