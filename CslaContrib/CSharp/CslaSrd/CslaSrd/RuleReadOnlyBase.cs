using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using Csla.Validation;
using CslaSrd;
using CslaSrd.Validation;


namespace CslaSrd
{
    [Serializable()]
    public abstract class RuleReadOnlyBase<T> : Csla.ReadOnlyBase<T> where T : RuleReadOnlyBase<T>
    {

    }
}


