using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Csla;
using CslaContrib.MEF;
using MEFSample.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MEFSample.Respository.Business.Test
{
  [TestClass]
  public class MyRootTest
  {
    public TestContext TestContext { get; set; }

    [ClassInitialize]
    public static void MyClassInitialize(TestContext testContext)
    {
      // set uo IoC container to use MyRootFakeData
      var container = new CompositionContainer();
      container.ComposeParts(new MyRootFakeData());
      Ioc.InjectContainer(container);
    }

    [ClassCleanup]
    public static void MyClassCleanup()
    {
      Ioc.InjectContainer(null);
    }

    [TestMethod]
    public void GetRootTest()
    {
      var root1 = MyRoot.GetRoot(1);

      Assert.AreEqual(1, root1.Id);
      Assert.AreEqual("Jonny", root1.Name);

      var root2 = MyRoot.GetRoot(2);
      Assert.AreEqual(2, root2.Id);
      Assert.AreEqual("Matt", root2.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(DataPortalException))]
    public void GetRootThrowsDataPortalException()
    {
      var root1 = MyRoot.GetRoot(999);
    }
  }
}