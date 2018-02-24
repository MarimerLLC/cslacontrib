using System.ComponentModel.Composition;
using System.Data;
using MEFSample.Business.Repository;

namespace MEFSample.Respository.Business.Test
{
  [Export(typeof(IRootDataAccess))]
  public class MyRootFakeData : IRootDataAccess
  {
    public RootData Get(int id)
    {
      if (id == 1) return new RootData {Id = 1, Name = "Jonny"};
      if (id == 2) return new RootData {Id = 2, Name = "Matt"};
      if (id == 999) throw new SyntaxErrorException("Test");

      return new RootData {Id = id, Name = string.Format("Name {0}", id)};
    }
  }
}