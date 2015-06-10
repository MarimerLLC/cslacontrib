using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using MEFSample.Business.Repository;

namespace MEFSample.Respository.Business.Test
{
  [Export(typeof(IRootDataAccess))]
  public class MyRootFakeData : IRootDataAccess
  {

    public RootData Get(int id)
    {
      if (id == 1) return new RootData() { Id = 1, Name = "Jonny" };
      if (id == 2) return new RootData() { Id = 2, Name = "Matt" };
      if (id == 999) throw new System.Data.SyntaxErrorException("Test");

      return new RootData() { Id = id, Name = string.Format("Name {0}", id) };
    }
  }
}
