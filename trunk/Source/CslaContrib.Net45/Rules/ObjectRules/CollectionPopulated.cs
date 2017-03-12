// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionPolpulated.cs" company="Marimer LLC">
//   Copyright (c) Marimer LLC. All rights reserved. Website: http://www.lhotka.net/cslanet
// </copyright>
// <summary>
//   Object Business rule for checking a child collection has a least one item.
// </summary>
// <remarks>
//   If no child collections are specified, will check every child collection of the object.
//   Rule should run on client when a property is changed or when CheckRules is called.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Csla;
using Csla.Core;
using Csla.Rules;

namespace CslaContrib.Rules.ObjectRules
{
    /// <summary>
    /// Object Business rule for checking a child collection has a least one item.<br/>
    /// Rule should run on client when a property is changed or when CheckRules is called.
    /// </summary>
    /// <remarks>
    /// If no child collections are specified, will check every child collection of the object.
    /// </remarks>
    public class CollectionPopulated : CommonObjectRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPopulated"/> class.
        /// </summary>
        public CollectionPopulated()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPopulated"/> class.
        /// </summary>
        /// <param name="message">The error message text.</param>
        public CollectionPopulated(string message)
        {
            MessageText = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPopulated"/> class.
        /// </summary>
        /// <param name="messageDelegate">The error message function.</param>
        public CollectionPopulated(Func<string> messageDelegate)
        {
            MessageDelegate = messageDelegate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPopulated"/> class.
        /// </summary>
        /// <param name="childCollectionProperties">The child collections properties to check.</param>
        public CollectionPopulated(params IPropertyInfo[] childCollectionProperties)
        {
            if (InputProperties == null)
                InputProperties = new List<IPropertyInfo>();
            InputProperties.AddRange(childCollectionProperties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPopulated"/> class.
        /// </summary>
        /// <param name="message">The error message text.</param>
        /// <param name="childCollectionProperties">The child collections properties to check.</param>
        public CollectionPopulated(string message, params IPropertyInfo[] childCollectionProperties)
            : this(childCollectionProperties)
        {
            MessageText = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPopulated"/> class.
        /// </summary>
        /// <param name="messageDelegate">The error message function.</param>
        /// <param name="childCollectionProperties">The child collections properties to check.</param>
        public CollectionPopulated(Func<string> messageDelegate, params IPropertyInfo[] childCollectionProperties)
            : this(childCollectionProperties)
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
            return HasMessageDelegate ? base.GetMessage() : "The collection(s) - {0} - must have a at least one item.";
        }

        /// <summary>
        /// Business rule implementation.
        /// </summary>
        /// <param name="context">Rule context object.</param>
        protected override void Execute(RuleContext context)
        {
            var emptyCollections = new List<string>();

            if (context.InputPropertyValues.Count == 0)
            {
                var target = (BusinessBase) context.Target;
                foreach (var field in target.GetType().GetFields(
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic))
                {
                    var value = field.GetValue(target) as IPropertyInfo;
                    if (value != null)
                    {
                        if ((value.RelationshipType & RelationshipTypes.Child) == RelationshipTypes.Child)
                            emptyCollections.Add(value.FriendlyName);
                    }
                }
            }
            else
            {
                foreach (var field in context.InputPropertyValues)
                {
                    if (field.Value != null)
                    {
                        var childCollection = (IList) field.Value;
                        if (childCollection.Count == 0)
                            emptyCollections.Add(field.Key.FriendlyName);
                    }
                    else
                        emptyCollections.Add(field.Key.FriendlyName);
                }
            }

            var unpopulatedNames = String.Join(", ", emptyCollections);
            var errorMessage = string.Format(GetMessage(), unpopulatedNames);
            context.Results.Add(new RuleResult(RuleName, PrimaryProperty, errorMessage) { Severity = Severity });
        }
    }
}
