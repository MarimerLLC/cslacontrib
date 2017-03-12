// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsOwnerOrIsInRole.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Authorization rule for checking a Property can be accessed only
//   by the user that created the object or the user has any of the specified roles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Csla;
using Csla.Core;
using Csla.Reflection;
using Csla.Rules;

namespace CslaContrib.Rules.AuthorizationRules
{
    /// <summary>
    /// Authorization rule for checking a Property can be accessed only
    /// by the user that created the object or the user has any of the specified roles.
    /// </summary>
    public class IsOwnerOrIsInRole : AuthorizationRule
    {
        private readonly List<string> _roles;
        private string _creatorProperty;
        private Func<int> _getCurrentUserDelegate;

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
        /// <param name="creatorProperty">Name of the property for the creator ID.</param>
        /// <param name="getCurrentUserDelegate">The Func delegate to get the current user ID.</param>
        /// <param name="roles">List of allowed roles.</param>
        public IsOwnerOrIsInRole(AuthorizationActions action, IMemberInfo element, string creatorProperty, Func<int> getCurrentUserDelegate, List<string> roles)
            : base(action, element)
        {
            _creatorProperty = creatorProperty;
            _getCurrentUserDelegate = getCurrentUserDelegate;
            _roles = roles;
        }

        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="action">Action this rule will enforce.</param>
        /// <param name="element">Member to be authorized.</param>
        /// <param name="creatorProperty">Name of the property for the creator ID.</param>
        /// <param name="getCurrentUserDelegate">The Func delegate to get the current user ID.</param>
        /// <param name="roles">List of allowed roles.</param>
        public IsOwnerOrIsInRole(AuthorizationActions action, IMemberInfo element, string creatorProperty, Func<int> getCurrentUserDelegate, params string[] roles)
            : base(action, element)
        {
            _creatorProperty = creatorProperty;
            _getCurrentUserDelegate = getCurrentUserDelegate;
            _roles = new List<string>(roles);
        }

        /// <summary>
        /// Rule implementation.
        /// </summary>
        /// <param name="context">Authorization context.</param>
        protected override void Execute(AuthorizationContext context)
        {
            var creatorID = (int) MethodCaller.CallPropertyGetter(context.Target, _creatorProperty);
            var currentUserID = _getCurrentUserDelegate.Invoke();
            if (currentUserID == creatorID)
                context.HasPermission = true;

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
