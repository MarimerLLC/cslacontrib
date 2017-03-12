// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsEmptyOrIsInRole.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Authorization rule for checking a Property can be accessed only it was empty
//   (at the time the edit started) or the user has any of the specified roles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using Csla;
using Csla.Core;
using Csla.Reflection;
using Csla.Rules;

namespace CslaContrib.Rules.AuthorizationRules
{
    /// <summary>
    /// Authorization rule for checking a Property can be accessed only it was empty
    /// (at the time the edit started) or the user has any of the specified roles.
    /// </summary>
    public class IsEmptyOrIsInRole : AuthorizationRule
    {
        private readonly List<string> _roles;

        /// <summary>
        /// Gets a value indicating whether the results
        /// of this rule can be cached at the business
        /// object level.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the result of this rule can be cached by the calling code; otherwise, <c>false</c>.
        /// </value>
        public new bool CacheResult
        {
            get { return false; }
        }

        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="action">Action this rule will enforce.</param>
        /// <param name="element">Member to be authorized.</param>
        /// <param name="roles">List of allowed roles.</param>
        public IsEmptyOrIsInRole(AuthorizationActions action, IMemberInfo element, List<string> roles)
            : base(action, element)
        {
            _roles = roles;
        }

        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="action">Action this rule will enforce.</param>
        /// <param name="element">Member to be authorized.</param>
        /// <param name="roles">List of allowed roles.</param>
        public IsEmptyOrIsInRole(AuthorizationActions action, IMemberInfo element, params string[] roles)
            : base(action, element)
        {
            _roles = new List<string>(roles);
        }

        /// <summary>
        /// Rule implementation.
        /// </summary>
        /// <param name="context">Authorization context.</param>
        protected override void Execute(AuthorizationContext context)
        {
            var isEmpty = false;

            var field = Element as IPropertyInfo;
            var smartField = Element as ISmartField;

            var target = (BusinessBase) context.Target;
            var value = MethodCaller.CallPropertyGetter(target, Element.Name);

            if (target.IsNew || value == null)
                isEmpty = true;
            else if (field != null)
            {
                if (value == field.DefaultValue)
                    isEmpty = true;
            }
            else if (smartField != null)
            {
                if (smartField.IsEmpty)
                    isEmpty = true;
            }

            if (!isEmpty)
                if (target.IsDirty)
                    isEmpty = true;
                else
                    isEmpty = string.IsNullOrEmpty(value.ToString());

            if (isEmpty)
                context.HasPermission = true;
            else
            {
                if (_roles.Count > 0)
                {
                    if (_roles.Any(item => ApplicationContext.User.IsInRole(item)))
                    {
                        context.HasPermission = true;
                    }
                }
                else
                {
                    // if no role specified, allow all roles
                    context.HasPermission = true;
                }
            }
        }
    }
}
