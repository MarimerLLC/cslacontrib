// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopIfNotCanWrite.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Short circuit rule processing for this property if user is not allowed to edit this field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.ShortCircuitingRules
{
  /// <summary>
  /// ShortCircuit rule processing if user is not allowed to edit field.
  /// </summary>
  public class StopIfNotCanWrite : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfNotCanWrite"/> class.
    /// </summary>
    /// <param name="property">The property to check.</param>
    public StopIfNotCanWrite(IPropertyInfo property)
      : base(property)
    {
    }

    /// <summary>
    /// Rule indicating whether the user is authorized
    /// to change the property value.
    /// Will always be silent and never set rule to broken.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    /// <remarks>
    /// Combine this Rule with short-circuiting to
    /// prevent evaluation of other rules in the case
    /// that the user isn't allowed to change the value.
    /// </remarks>
    protected override void Execute(RuleContext context)
    {
      var business = context.Target as BusinessBase;
      if (business == null) return;

      if (!business.CanWriteProperty(context.Rule.PrimaryProperty))
      {
        // shortcurcuit as user cannot write to the property (set stopProcessing)
        context.AddSuccessResult(true);
      }
    }
  }
}