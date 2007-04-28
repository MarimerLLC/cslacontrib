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
    public abstract class RuleBusinessBase<T> : BusinessBase<T> where T : RuleBusinessBase<T> , Csla.Core.IEditableBusinessObject
    {
        /// <summary>
        /// Provides a collection of all validation rules on the object that are broken, i.e., in an invalid state.
        /// </summary>
        public BrokenRulesCollection BrokenRules
        {
            get
            {
                return BrokenRulesCollection;
            }
        }

        /// <summary>
        /// Provides a collection of all validation rules on the object. 
        /// </summary>
        public PublicRuleInfoList Rules
        {
            get
            {
                return PublicRuleInfoList.GetList(base.ValidationRules.GetRuleDescriptions());
            }
        }
    }

}
