using System.ComponentModel.Composition;
using MEFSample.Business.Repository;

namespace MEFSample.Respository.DAL
{
  [Export(typeof(ICustomerDataAccess))]
  public class CustomerDataAccess : ICustomerDataAccess
  {
    public CustomerData[] Get(string filter)
    {
      return new[]
      {
        new CustomerData {Id = 1, Name = "Baker, Jonathan"},
        new CustomerData {Id = 2, Name = "Peterson, Peter"},
        new CustomerData {Id = 3, Name = "Olsen, Egon"},
        new CustomerData {Id = 4, Name = "Hansen, hans"}
      };
    }
  }
}