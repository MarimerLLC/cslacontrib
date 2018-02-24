namespace MEFSample.Business.FactoryInterfaces
{
  public interface ICustomerFactory
  {
    object Fetch(string criteria);
  }
}