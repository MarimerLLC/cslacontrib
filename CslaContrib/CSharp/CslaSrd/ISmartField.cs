using System;
using System.Collections.Generic;
using System.Text;

namespace CslaSrd
{
    public interface ISmartField
    {
        /// <summary>
        /// A method, rather than a property, that equates to the IsEmpty property.
        /// </summary>
        /// <returns></returns>
        bool HasNullValue();
    }
}
