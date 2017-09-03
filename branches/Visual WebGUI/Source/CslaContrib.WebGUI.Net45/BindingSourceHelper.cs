//-----------------------------------------------------------------------
// <copyright file="BindingSourceHelper.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Helper methods for dealing with BindingSource</summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Csla.Properties;
using Gizmox.WebGUI.Forms;

namespace CslaContrib.WebGUI
{
  /// <summary>
  /// Helper methods for dealing with BindingSource
  /// objects and data binding.
  /// </summary>
  public static class BindingSourceHelper
  {
    private static BindingSourceNode _rootSourceNode;

    /// <summary>
    /// Sets up BindingSourceNode objects for all
    /// BindingSource objects related to the provided
    /// root source.
    /// </summary>
    /// <param name="container">
    /// Container for the components.
    /// </param>
    /// <param name="rootSource">
    /// Root BindingSource object.
    /// </param>
    /// <returns></returns>
    public static BindingSourceNode InitializeBindingSourceTree(
      IContainer container, BindingSource rootSource)
    {
      if (rootSource == null)
        throw new ApplicationException(Resources.BindingSourceNotProvided);

      _rootSourceNode = new BindingSourceNode(rootSource);
      _rootSourceNode.Children.AddRange(GetChildBindingSources(container, rootSource, _rootSourceNode));

      return _rootSourceNode;
    }

    private static List<BindingSourceNode> GetChildBindingSources(
      IContainer container, BindingSource parent, BindingSourceNode parentNode)
    {
      List<BindingSourceNode> children = new List<BindingSourceNode>();

#if !WEBGUI
      foreach (System.ComponentModel.Component component in container.Components)
#else
      foreach (IComponent component in container.Components)
#endif
      {
        if (component is BindingSource)
        {
          BindingSource temp = component as BindingSource;
          if (temp.DataSource != null && temp.DataSource.Equals(parent))
          {
            BindingSourceNode childNode = new BindingSourceNode(temp);
            children.Add(childNode);
            childNode.Children.AddRange(GetChildBindingSources(container, temp, childNode));
            childNode.Parent = parentNode;
          }
        }
      }

      return children;
    }
  }
}