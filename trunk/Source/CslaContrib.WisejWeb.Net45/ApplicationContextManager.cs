//-----------------------------------------------------------------------
// <copyright file="ApplicationContextManager.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>Application context manager that uses
// Wisej.Base.ApplicationBase (aka WisejContext)</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Core;
using System.Web;
using Csla.Security;
using WisejContext = Wisej.Base.ApplicationBase;

namespace CslaContrib.WisejWeb
{
  /// <summary>
  /// Application context manager that uses Wisej.Base.ApplicationBase (aka WisejContext)
  /// to store context values.
  /// </summary>
  public class ApplicationContextManager : IContextManager
  {
    private const string _localContextName = "Csla.LocalContext";
    private const string _clientContextName = "Csla.ClientContext";
    private const string _globalContextName = "Csla.GlobalContext";

    private static string _sessionId;

    public ApplicationContextManager()
    {
      _sessionId = WisejContext.SessionId;
      WisejContext.Session.User = new UnauthenticatedPrincipal();
      WisejContext.Session.Items = new Dictionary<string, ContextDictionary>();
      SetLocalContext(new ContextDictionary());
      SetClientContext(new ContextDictionary());
      SetGlobalContext(new ContextDictionary());
    }

    /// <summary>
    /// Gets a value indicating whether this
    /// context manager is valid for use in
    /// the current environment.
    /// </summary>
    public bool IsValid
    {
      get
      {
        return _sessionId == WisejContext.SessionId;
      }
    }

    /// <summary>
    /// Gets the current principal.
    /// </summary>
    public System.Security.Principal.IPrincipal GetUser()
    {
      return WisejContext.Session.User;
    }

    /// <summary>
    /// Sets the current principal.
    /// </summary>
    /// <param name="principal">Principal object.</param>
    public void SetUser(System.Security.Principal.IPrincipal principal)
    {
      WisejContext.Session.User = principal;
    }

    /// <summary>
    /// Gets the local context.
    /// </summary>
    public ContextDictionary GetLocalContext()
    {
      return (ContextDictionary)WisejContext.Session.Items[_localContextName];
    }

    /// <summary>
    /// Sets the local context.
    /// </summary>
    /// <param name="localContext">Local context.</param>
    public void SetLocalContext(ContextDictionary localContext)
    {
      WisejContext.Session.Items[_localContextName] = localContext;
    }

    /// <summary>
    /// Gets the client context.
    /// </summary>
    public ContextDictionary GetClientContext()
    {
      return (ContextDictionary)WisejContext.Session.Items[_clientContextName];
    }

    /// <summary>
    /// Sets the client context.
    /// </summary>
    /// <param name="clientContext">Client context.</param>
    public void SetClientContext(ContextDictionary clientContext)
    {
      WisejContext.Session.Items[_clientContextName] = clientContext;
    }

    /// <summary>
    /// Gets the global context.
    /// </summary>
    public ContextDictionary GetGlobalContext()
    {
      return (ContextDictionary)WisejContext.Session.Items[_globalContextName];
    }

    /// <summary>
    /// Sets the global context.
    /// </summary>
    /// <param name="globalContext">Global context.</param>
    public void SetGlobalContext(ContextDictionary globalContext)
    {
      WisejContext.Session.Items[_globalContextName] = globalContext;
    }
  }
}