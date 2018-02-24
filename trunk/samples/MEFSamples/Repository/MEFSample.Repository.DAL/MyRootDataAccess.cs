using System.ComponentModel.Composition;
using MEFSample.Business.Repository;

namespace MEFSample.Respository.DAL
{
  [Export(typeof(IRootDataAccess))]
  public class MyRootDataAccess : IRootDataAccess
  {
    public RootData Get(int id)
    {
      return new RootData {Id = id, Name = "Ole Olsen"};
    }
  }
}