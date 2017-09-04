// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopIfNotFieldExixts.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Shorcircuits rule processing if lazy loaded field is not initalized.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.ShortCircuitingRules
{
    /// <summary>
    /// Shorcircuits rule processing if lazy loaded field is not initalized (ie: not included in InputPropertyValues).
    /// </summary>
    public class StopIfNotFieldExists : BusinessRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopIfNotFieldExists"/> class.
        /// </summary>
        /// <param name="primaryProperty">Primary property for this rule.</param>
        public StopIfNotFieldExists(IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            if (InputProperties == null)
                InputProperties = new List<IPropertyInfo>();
            InputProperties.Add(primaryProperty);
        }

        /// <summary>
        /// Rule indicating whether the lazy loaded field is not initalized
		/// (ie: not included in InputPropertyValues).
        /// Will always be silent and never set rule to broken.
        /// </summary>
        /// <param name="context">Rule context object.</param>
        protected override void Execute(RuleContext context)
        {
            if (!context.InputPropertyValues.ContainsKey(PrimaryProperty))
            {
                // shortcurcuit as field isn't in InputPropertyValues (set stopProcessing)
                context.AddSuccessResult(true);
            }
        }
    }
}
