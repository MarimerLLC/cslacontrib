using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Csla;
using Csla.Reflection;
using Csla.Server;
using MEFSample.Business;
using MEFSample.Business.FactoryInterfaces;
using MEFSample.ObjectFactoryDAL.ChildInterfaces;
using MEFSample.ObjectFactoryDAL.DataEntitites;

namespace MEFSample.ObjectFactoryDAL
{
  [Export(typeof(ICustomerFactory))]
  public class CustomerFactory : ObjectFactory, ICustomerFactory
  {
    // use property injection 
    [Import(typeof(ICustomerInfoFactory))]
    public ICustomerInfoFactory MyCustomerInfoFactory { get; set; }


    // OR use Constructor injection
    //[ImportingConstructor]
    //public CustomerFactory(ICustomerInfoFactory customerInfoFactory)
    //{
    //  MyCustomerInfoFactory = customerInfoFactory;
    //}

    public object Fetch(string criteria)
    {
      var list = (CustomerList)MethodCaller.CreateInstance(typeof(CustomerList));


      // just sets up some mock data - could com from Xml,L2S, EF or othere sources unknown to the BO itself. 
      var customers = new[]
               {
                 new CustomerData {Id = 1, Name = "Baker, Jonathan"},
                 new CustomerData {Id = 2, Name = "Peterson, Peter"},
                 new CustomerData {Id = 3, Name = "Olsen, Egon"},
                 new CustomerData {Id = 4, Name = "Hansen, hans"}
               };

      list.RaiseListChangedEvents = false;
      this.SetIsReadOnly(list, false);

      // tranform to my lists child type
      list.AddRange(customers.Select(p => MyCustomerInfoFactory.GetCustomerInfo(p)));

      this.SetIsReadOnly(list, true);
      list.RaiseListChangedEvents = true;

      return list;
    }
  }
}
