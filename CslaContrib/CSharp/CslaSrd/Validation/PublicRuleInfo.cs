using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using CslaSrd.Properties;

namespace CslaSrd.Validation
{
    /// <summary>
    /// Contains publically available information about a given validation rule for a given object.
    /// </summary>
    [Serializable()]
    public class PublicRuleInfo : ReadOnlyBase<PublicRuleInfo>
    {

        #region Business Methods
        
        //TODO: add priority and severity information at some point.
        //private int      _priority        = 0;
        //private string[] _ruleParamNames  = { String.Empty, String.Empty };
        //private string[] _ruleParamValues = { String.Empty, String.Empty };
        private string    _ruleName        = String.Empty;
        private string    _ruleDescription = String.Empty;
        private string    _ruleProperty    = String.Empty;
        private ArrayList _ruleParamNames  = new ArrayList();
        private ArrayList _ruleParamValues = new ArrayList();


        /// <summary>
        /// The order in which this rule will be evaluated, relative to other rules on the same object, 
        /// with lower numbers being "sooner".
        /// Technical note: This information is not yet publically available as it is not exposed elsewhere.
        /// </summary>
        //public int Priority
        //{
        //    get { return _priority; }
        //}

        /// <summary>
        /// The name of the rule that has been assigned to this object.
        /// </summary>
        public string RuleName
        {
            get { return _ruleName; }
        }

        /// <summary>
        /// A brief explanation of the rule, suitable for presentation to a non-technical end-user, that explains how the rule would be broken.
        /// </summary>
        public string RuleDescription
        {
            get { return _ruleDescription; }
        }
        /// <summary>
        /// The object property that this rule is assigned to.
        /// </summary>
        public string RuleProperty
        {
            get { return _ruleProperty; }
        }
        /// <summary>
        /// An array of parameters that the rule needs in order to evaluate whether the rule is broken or not.
        /// </summary>
        public string[] RuleParamNames
        {
            get 
            { 
                string[] returnValue = new string[_ruleParamNames.Count];
                for (int i =0; 1<_ruleParamNames.Count; i++)
                {
                    returnValue[i] = _ruleParamNames[i].ToString();
                }
                return returnValue;
            }
        }
        /// <summary>
        /// An array of parameter values that the rule needs in order to evaluate whether the rule is broken or not.
        /// </summary>
        public string[] RuleParamValues
        {
            get 
            { 
                string[] returnValue = new string[_ruleParamValues.Count];
                for (int i =0; 1<_ruleParamValues.Count; i++)
                {
                    returnValue[i] = _ruleParamValues[i].ToString();
                }
                return returnValue;
            }
        }
        /// <summary>
        /// The name of the rule and the property it is assigned to (delimited by a / character) uniquely identify a given rule.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _ruleName + "/" + _ruleProperty;
        }

        public override string ToString()
        {
            return _ruleName + "/" + _ruleProperty;
        }

        #endregion

        #region Constructors

        private PublicRuleInfo()
        { /* require use of factory methods */ }

        /// <summary>
        /// Creates a new PublicRuleInfo definition based upon the data supplied.
        /// </summary>
        /// <param name="ruleStuff">The publically known data about the rule, formatted as follows:
        ///     rule://ruleName/propertyName?paramName1=paramValue1&paramName2=paramValue2
        /// </param>
        internal PublicRuleInfo(string ruleStuff)
        {
            //            rule://StringMaxLength/Name?maxLength=10&anotherParam=abc

            // I tried using System.Uri as a way to parse the incoming rule data.
            // Sadly, it corrupts some of the incoming data by changing it to all lowercase.
            // So, this is a less nifty, but more correct parsing of the data.

            
            // Trim off the initial string "rule://"
            ruleStuff = ruleStuff.Substring(7);

            // Separate the ruleName and propertyName from the parameter names and values.
            string[] tempList = ruleStuff.Split('?');
            // Separate the ruleName from the propertyName.
            string[] infoList = tempList[0].Split('/');
            _ruleName     = infoList[0];
            _ruleProperty = infoList[1];

            // Start producing the end-user description of the rule by getting the standard description text for the rule.
            // All standard description text entries in the resource file start with "rule" so that they sort together.
            // It is important that the resource key is an exact match to the ruleName (prefixed by "rule"), or the 
            // standard description text will not be found.
            _ruleDescription = Properties.Resources.ResourceManager.GetString("rule" + _ruleName);
            if (_ruleDescription == null || _ruleDescription == String.Empty)
            {
                // If no standard description is found, or if it is empty, default to the source data for the rule.
                _ruleDescription = "rule://" + ruleStuff;
            }
            // If the standard rule description text has a place-holder for the property name in its text, replace it with the property name here.
            // NICE-TO-HAVE: A way to look up property names in a reference file so that they too are language-specific to the user.
            _ruleDescription = _ruleDescription.Replace("{rulePropertyName}", _ruleProperty);

            // Parse thru the parameter name/value pairs and replace any parameter name placeholders in the standard
            // description text with the corresponding parameter value.
            string[] queryPairList;

            if (tempList.Length > 1)
            {
                queryPairList = tempList[1].Split('&');
                for (int i = 0; i < queryPairList.Length; i++)
                {
                    string[] temp = queryPairList[i].Split('=');
                    _ruleParamNames.Add(temp[0]);
                    _ruleParamValues.Add(temp[1]);
                    _ruleDescription = _ruleDescription.Replace("{" + temp[0].ToString() + "}", temp[1].ToString());
                }
            }
        }

        #endregion
    }
}

