using System;
using System.Collections.Generic;
using System.Text;

namespace CslaSrd
{
    [Serializable()]
    public abstract class RuleEditableRootListBase<T> : Csla.EditableRootListBase<T> 
        where T : Csla.Core.IEditableBusinessObject, Csla.Core.ISavable
    {
    }
}
