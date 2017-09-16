// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopIfNotIsExisting.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Shorcircuits rule processing if target object doesn't exist or was deleted.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.ShortCircuitingRules
{
  /// <summary>
  /// ShortCircuit rule processing if target is not an existing object (exists and is not deleted).
  /// </summary>
  public class StopIfNotIsExisting : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfNotIsExisting"/> class.
    /// </summary>
    /// <param name="property">The property.</param>
    public StopIfNotIsExisting(IPropertyInfo property) : base(property) { }

    /// <summary>
    /// Rule indicating whether the target object is an existing object (exists and is not deleted).
    /// Will always be silent and never set rule to broken.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(RuleContext context)
    {
      var target = (ITrackStatus)context.Target;
      if (target == null || target.IsDeleted)
      {
        // shortcurcuit as target object doesn't exist or was deleted (set stopProcessing)
        context.AddSuccessResult(true);
      }
    }
  }
}