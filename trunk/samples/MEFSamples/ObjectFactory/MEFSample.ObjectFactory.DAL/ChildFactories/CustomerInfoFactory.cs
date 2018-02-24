using System.ComponentModel.Composition;
using Csla.Reflection;
using Csla.Server;
using MEFSample.Business;
using MEFSample.ObjectFactoryDAL.ChildInterfaces;

namespace MEFSample.ObjectFactoryDAL.ChildFactories
{
  [Export(typeof(ICustomerInfoFactory))]
  public class CustomerInfoFactory : ObjectFactory, ICustomerInfoFactory
  {
    public CustomerInfo GetCustomerInfo(DataEntitites.CustomerData data)
    {
      var customer = (CustomerInfo) MethodCaller.CreateInstance(typeof(CustomerInfo));

      LoadProperty(customer, CustomerInfo.IdProperty, data.Id);
      LoadProperty(customer, CustomerInfo.NameProperty, data.Name);

      return customer;
    }
  }
}