// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopIfIsNew.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Shorcircuits rule processing if object is new.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.ShortCircuitingRules
{
    /// <summary>
    /// Shorcircuits rule processing if object is new.
    /// </summary>
    public class StopIfIsNew : PropertyRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopIfIsNew"/> class.
        /// </summary>
        /// <param name="primaryProperty">
        /// The primary property.
        /// </param>
        public StopIfIsNew(IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
        }

        /// <summary>
        /// Rule indicating whether the target object is new.
        /// Will always be silent and never set rule to broken.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void Execute(RuleContext context)
        {
            var target = (ITrackStatus) context.Target;
            if (target.IsNew)
            {
                // shortcurcuit as target object is new (set stopProcessing)
                context.AddSuccessResult(true);
            }
        }
    }
}
