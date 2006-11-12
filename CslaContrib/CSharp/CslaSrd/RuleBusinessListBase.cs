using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using Csla.Core;

namespace CslaSrd
{
    [Serializable()]
    public abstract class SrdBusinessListBase<T, C> : Csla.BusinessListBase<T, C>
        where T : SrdBusinessListBase<T, C>
        where C : RuleBusinessBase<C>, IEditableBusinessObject
    {

        /// <summary>
        /// Provides a public method to tell whether any of the items in the collection has any broken validation rules.
        /// </summary>
        public bool IsBroken 
        {
            get
            {
                bool foundBroken = false;
                for (int i = 0; i < this.Count; i++)
                {
                    RuleBusinessBase<C> item = Items[i];
                    if (item.BrokenRules.Count > 0)
                    {
                        foundBroken = true;
                    }
                }
                return foundBroken;
            }
        }
        }


}
