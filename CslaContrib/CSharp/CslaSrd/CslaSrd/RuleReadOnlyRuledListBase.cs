using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Csla;
using Csla.Validation;
using CslaSrd;
using CslaSrd.Validation;


namespace CslaSrd
{
    [Serializable()]
    public abstract class RuleReadOnlyRuledListBase<T, C> : Csla.ReadOnlyRuledListBase<T, C>
        where T : RuleReadOnlyRuledListBase<T, C>
        where C : RuleReadOnlyRuledBase<C>, Csla.Core.IReadOnlyObject
    {
        public bool IsBroken
        {
            get
            {
                bool foundBroken = false;
                for (int i = 0; i < this.Count; i++)
                {
                    RuleReadOnlyRuledBase<C> item = Items[i];
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



