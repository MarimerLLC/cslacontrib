// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopIfNotIsNew.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Shorcircuits rule processing if object is not new.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.ShortCircuitingRules
{
  /// <summary>
  /// ShortCircuit rule processing if target is not a new object
  /// </summary>
  public class StopIfNotIsNew : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfNotIsNew"/> class.
    /// </summary>
    /// <param name="property">The property.</param>
    public StopIfNotIsNew(IPropertyInfo property) : base(property) { }

    /// <summary>
    /// Rule indicating whether the target object is not new.
    /// Will always be silent and never set rule to broken.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(RuleContext context)
    {
      var target = (ITrackStatus)context.Target;
      if (!target.IsNew)
      {
        // shortcurcuit as target object isn't new (set stopProcessing)
        context.AddSuccessResult(true);
      }
    }
  }
}