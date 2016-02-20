using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CslaContrib.UnitTests.ObjectCaching
{
  [Serializable]
  class TestCriteria : Csla.SingleCriteria<TestCachedInfo, int>
    {
        public TestCriteria(int id) : base(id) { }

        public override int GetHashCode()
        {
            return base.Value.GetHashCode();
        }
    }
}
