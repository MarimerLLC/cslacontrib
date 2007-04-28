using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using Csla.Validation;
using Csla.Core;
using CslaSrd;

namespace CslaSrd.Validation
{
    /// <summary>
    /// Used to provide the list of validation rules on the object.
    /// </summary>
    [Serializable()]
    public class PublicRuleInfoList : ReadOnlyListBase<PublicRuleInfoList, PublicRuleInfo>
    {
        #region Authorization Rules

        /// <summary>
        /// Is the user authorized to see this information?
        /// </summary>
        /// <returns>Whether the user is authorized or not.</returns>
        public static bool CanGetObject()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Returns the text of all rule descriptions, each
        /// separated by a <see cref="Environment.NewLine" />.
        /// </summary>
        /// <returns>The text of all rule descriptions.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            bool first = true;
            foreach (PublicRuleInfo item in this)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    result.Append(Environment.NewLine);
                }
                result.Append(item.RuleDescription);
            }
            return result.ToString();
        }


        #region Factory Methods

        /// <summary>
        ///  Given an array of rule data as provided by ValidationRules.GetRuleDescriptions, return a collection of validation rules.
        /// </summary>
        /// <param name="ruleList">An array of rule data as provided by ValidationRules.GetRuleDescriptions.</param>
        /// <returns></returns>
        public static PublicRuleInfoList GetList(String[] ruleList)
        {
            PublicRuleInfoList list = new PublicRuleInfoList();
            list.IsReadOnly = false;
            list.RaiseListChangedEvents = false;
            for (int i = 0; i < ruleList.Length; i++)
            {
                list.Add(new PublicRuleInfo(ruleList[i]));
            }
            list.RaiseListChangedEvents = true;
            list.IsReadOnly = true;
            return list;
        }

        private PublicRuleInfoList()
        {
            this.AllowNew = true;
        }

        #endregion

    }

}

