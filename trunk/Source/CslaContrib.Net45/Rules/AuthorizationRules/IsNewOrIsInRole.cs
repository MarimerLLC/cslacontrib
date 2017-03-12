// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsNewOrIsInRole.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Authorization rule for checking a Property can be accessed only if the object IsNew
//   or the user has any of the specified roles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using Csla;
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.AuthorizationRules
{
    /// <summary>
    /// Authorization rule for checking a Property can be accessed only if the object IsNew
    /// or the user has any of the specified roles.
    /// </summary>
    public class IsNewOrIsInRole : AuthorizationRule
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
        public IsNewOrIsInRole(AuthorizationActions action, IMemberInfo element, List<string> roles)
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
        public IsNewOrIsInRole(AuthorizationActions action, IMemberInfo element, params string[] roles)
            : base(action, element)
        {
            _roles = new List<string>(roles);
        }

        /// <summary>
        /// Rule implementation.
        /// </summary>
        /// <param name="context">Rule context.</param>
        protected override void Execute(AuthorizationContext context)
        {
            var target = (ITrackStatus) context.Target;
            var isNew = target.IsNew;

            if (!isNew)
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
            else
            {
                context.HasPermission = true;
            }
        }
    }
}
