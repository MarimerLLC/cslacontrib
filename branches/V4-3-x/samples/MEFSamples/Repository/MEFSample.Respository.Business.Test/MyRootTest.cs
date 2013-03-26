using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Csla;
using MEFSample.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MEFSample.Respository.Business.Test
{
    
  [TestClass()]
  public class MyRootTest
  {
    private TestContext testContextInstance;
    public TestContext TestContext
    {
      get {return testContextInstance;}
      set {testContextInstance = value;}
    }

    [ClassInitialize()]
    public static void MyClassInitialize(TestContext testContext)
    {
      // set uo IoC container to use MyRootFakeData
      var container = new CompositionContainer();
      container.ComposeParts(new MyRootFakeData());
      CslaContrib.MEF.Ioc.InjectContainer(container);
    }

    [ClassCleanup()]
    public static void MyClassCleanup()
    {
      CslaContrib.MEF.Ioc.InjectContainer(null);
    }

    [TestMethod()]
    public void GetRootTest()
    {
      var root1 = MyRoot.GetRoot(1);

      Assert.AreEqual(1, root1.Id);
      Assert.AreEqual("Jonny", root1.Name);

      var root2 = MyRoot.GetRoot(2);
      Assert.AreEqual(2, root2.Id);
      Assert.AreEqual("Matt", root2.Name);
    }

    [TestMethod()]
    [ExpectedException(typeof(DataPortalException))]
    public void GetRootThrowsDataPortalException()
    {
      var root1 = MyRoot.GetRoot(999);
    }
  }
}
