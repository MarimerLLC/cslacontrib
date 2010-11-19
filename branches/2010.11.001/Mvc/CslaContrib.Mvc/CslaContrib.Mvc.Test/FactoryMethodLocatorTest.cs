using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace CslaContrib.Mvc.Test
{
    /// <summary>
    /// Summary description for ModelLocatorTest
    /// </summary>
    [TestClass]
    public class FactoryMethodLocatorTest
    {
        [TestMethod]
        public void WhenMethodNotExistShouldReturnNull()
        {
            var locator = new FactoryMethodLocator();
            MethodInfo m = locator.GetMethod(typeof(MyBO), typeof(MyBO), "MethodNotExist", new object[] {} );

            Assert.IsNull(m);
        }

        [TestMethod]
        public void ReturnsCorrectStaticFactoryMethod()
        {
            var locator = new FactoryMethodLocator();
            MethodInfo m = locator.GetMethod(typeof(MyBO), typeof(MyBO), "NewMyBO" , new object[] {});

            Assert.AreEqual(typeof(MyBO), m.DeclaringType);
            Assert.AreEqual(typeof(MyBO), m.ReturnType);
            Assert.IsTrue(m.IsStatic);
            Assert.AreEqual("NewMyBO", m.Name);
        }

        [TestMethod]
        public void ReturnsCorrectObjectFactoryMethod()
        {
            var locator = new FactoryMethodLocator();
            MethodInfo m = locator.GetMethod(typeof(MyFactory), typeof(MyBO), "NewMyBO", new object[] {});

            Assert.AreEqual(typeof(MyBO), m.ReturnType);
            Assert.IsFalse(m.IsStatic);
            Assert.AreEqual(typeof(MyFactory), m.DeclaringType);
            Assert.AreEqual("NewMyBO", m.Name);
        }

        //find factory method given action name, model type, parameter arguments, and/or factory type
        [TestMethod]
        public void CreateActionShouldReturnsNewMyBOMethod()
        {
            var locator = new FactoryMethodLocator();
            MethodInfo m = locator.GetMethod("create", typeof(MyBO), typeof(MyBO), (object[]) null);

            Assert.AreEqual(typeof(MyBO), m.DeclaringType);
            Assert.AreEqual(typeof(MyBO), m.ReturnType);
            Assert.IsTrue(m.IsStatic);
            Assert.AreEqual("NewMyBO", m.Name);
        }

        [TestMethod]
        public void EditActionWithSingleIntParameterReturnsGetMyBOMethod()
        {
            var locator = new FactoryMethodLocator();
            MethodInfo m = locator.GetMethod("edit", typeof(MyBO), typeof(MyBO), new object[] {1});

            Assert.AreEqual(typeof(MyBO), m.DeclaringType);
            Assert.AreEqual(typeof(MyBO), m.ReturnType);
            Assert.IsTrue(m.IsStatic);
            Assert.AreEqual("GetMyBO", m.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        public void EditActionWithSingleStringParameterReturnsAmbiguousException()
        {
            var locator = new FactoryMethodLocator();
            MethodInfo m = locator.GetMethod("edit", typeof(MyBO), typeof(MyBO), new object[] { "ricky" });
        }

        class MyBO : Csla.BusinessBase<MyBO>
        {
            public int ID { get; set; }

            public MyBO NotAFactoryMethod() { return this; }
            public static MyBO NewMyBO() { return new MyBO(); }
            public static MyBO GetMyBO(int id) { return new MyBO { ID = id }; }
            public static MyBO GetMyBOByX(string str) { return new MyBO { ID = int.Parse(str) }; }
            public static MyBO GetMyBOByY(string str) { return new MyBO { ID = int.Parse(str) }; }
        }

        class MyFactory
        {
            public MyBO NewMyBO() { return new MyBO(); }
            public MyBO GetMyBO(int id) { return new MyBO { ID = id }; }
        }

    }
}
