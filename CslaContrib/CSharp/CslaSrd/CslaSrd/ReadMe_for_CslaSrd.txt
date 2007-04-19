
NOTE:
I do not have team studio at work, and that's what I've been told to use to load this project up in CslaContrib.
It pretty regularly loses the connection information.
I've mistakenly placed the new version of the project in a sub-directory of the old project.
I have no idea how to fix that in CslaContrib.
So, the contents of this directory are the "real" CslaSrd project, not the directory above this.
Sorry for the confusion!


CslaSrd is a set of extensions to the Csla v2.1 framework.

We like the SmartDate class so much that we've added these classes:

SmartFloat
SmartBool
SmartDecimal
SmartInt16
SmartInt32
SmartInt64

All the above classes, except for SmartBool, work just like SmartDate.  SmartBool is more limited in capability, but works much the same.
All the above implement the ISmartField interface.
You might want to implement ISmartField in your own copy of SmartDate.  It will make writing generic validation rules libraries easier.

We've added SmartSafeDataReader, which is a subclass of SafeDataReader, in order to support the new "Smart" classes.
Use SmartSafeDataReader exactly like you would use SafeDataReader.

We've added a set of classes, RuleBusinessBase, RuleBusinessBaseList, etc.  These are subclasses of their Csla counterparts.
The intent is to add more business rule support into them, similar to what we have added to RuleBusinessBase and RuleBusinesBaseList.
Check out the BrokenRules and Rules properties in RuleBusinessBase.

We've also created a set of classes, PublicRuleInfo and PublicRuleInfoList.  These classes list all of the validation rules for a given business object.
They are used by RuleBusinessBase.  It makes a class self-documenting, because its Rules property lists all the validation rules for that object.

StdRules is our version of CommonRules.  The same rules in CommonRules are found here, but we've modified them to work with PublicRuleInfo.
The error messages for all the above classes are stored in Resources.resx.  We have also added several new validation rules:

        Rule: AOrBValue

        /// <summary>
        /// Implements a rule that only allows one field out of the specified set of fields to have a non-null value.
        /// Example: A gentleman can record his girlfriend's name, or his wife's name, but not both.
        /// </summary>
        

        Rule: SmartFieldRequired

        /// <summary>
        /// Rule ensuring a Smart... class value contains a non-null value.
        /// </summary>
        

        Rule: SmartDateRequired

        /// <summary>
        /// Rule ensuring a SmartDate class value contains a non-null value.
        /// </summary>
        
        Rule: MinMax
        /// <summary>
        /// The first field must not be greater than the second field.  Example: StartDate must not be greater than EndDate.
        /// </summary>
                        

Please note that we have changed the key structure for the messages to make them 100% consistent with the rule name.  This allows PublicRuleInfo to correctly find the appropriate message.
(The keys for the CommonRules had minor inconsistencies between the rule name and the rule error message key that made them unsuitable for our purpose.)
In addition, the rule error messages no longer have {0}, {1} subsitution markers in them.  Instead, we use {rulePropertyName} or the parameter name in {} braces.  Parameter name capitalization is camel-humped, with an initial lowercase letter.
In addition to making the messages suitable for use by PublicRuleInfo, it makes the messages self-documenting.

We have only supplied the English resource file, but it should be pretty easy to produce a translation.  Just use the English version for the substitution marker names, and the original CommonRules translation.

