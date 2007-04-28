using System;
using System.Collections.Generic;
using System.Text;

namespace CslaSrd
{
    [Serializable()]
    public abstract class RuleReadOnlyListBase<T, C> : Csla.ReadOnlyListBase<T, C>
        where T : RuleReadOnlyListBase<T, C>
        where C : RuleReadOnlyBase<C>, Csla.Core.IReadOnlyObject
    {
//        where C : RuleReadOnlyListBase<T, C>, Csla.Core.IReadOnlyObject
    }
}



