using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEFSample.Business.Repository
{
  public interface ICustomerDataAccess
  {
    CustomerData[] Get(string filter);
  }

  public class CustomerData
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
