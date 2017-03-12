// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateNotInFuture.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Business rule for checking a date property is valid and not in the future.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Csla.Core;
using Csla.Rules;
using Csla.Rules.CommonRules;

namespace CslaContrib.Rules.DateRules
{
    /// <summary>
    /// Business rule for checking a date property is valid and not in the future.
    /// </summary>
    public class DateNotInFuture : CommonBusinessRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateNotInFuture"/> class.
        /// </summary>
        /// <param name="primaryProperty">Primary property for this rule.</param>
        public DateNotInFuture(IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties = new List<IPropertyInfo> {primaryProperty};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateNotInFuture"/> class.
        /// </summary>
        /// <param name="primaryProperty">Primary property for this rule.</param>
        /// <param name="message">The error message text.</param>
        public DateNotInFuture(IPropertyInfo primaryProperty, string message)
            : this(primaryProperty)
        {
            MessageText = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateNotInFuture"/> class.
        /// </summary>
        /// <param name="primaryProperty">Primary property for this rule.</param>
        /// <param name="messageDelegate">The error message function.</param>
        public DateNotInFuture(IPropertyInfo primaryProperty, Func<string> messageDelegate)
            : this(primaryProperty)
        {
            MessageDelegate = messageDelegate;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <returns>
        /// The error message.
        /// </returns>
        protected override string GetMessage()
        {
            return HasMessageDelegate ? base.GetMessage() : "{0} can't be in the future.";
        }

        /// <summary>
        /// Business rule implementation.
        /// </summary>
        /// <param name="context">Rule context object.</param>
        protected override void Execute(RuleContext context)
        {
            object value = context.InputPropertyValues[PrimaryProperty];
            if (Convert.ToDateTime(value) > DateTime.Now)
            {
                var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName);
                context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) {Severity = Severity});
                return;
            }

            try
            {
                if (Convert.ToDateTime(value) >= DateTime.MinValue)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                context.AddErrorResult(string.Format("{0}{1} isn't valid.",
                                                     ex.Message + Environment.NewLine,
                                                     PrimaryProperty.FriendlyName));
            }
        }
    }
}