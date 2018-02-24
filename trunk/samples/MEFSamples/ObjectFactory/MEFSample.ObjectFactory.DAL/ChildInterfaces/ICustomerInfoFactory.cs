using MEFSample.Business;
using MEFSample.ObjectFactoryDAL.DataEntitites;

namespace MEFSample.ObjectFactoryDAL.ChildInterfaces
{
  public interface ICustomerInfoFactory
  {
    CustomerInfo GetCustomerInfo(CustomerData data);
  }
}