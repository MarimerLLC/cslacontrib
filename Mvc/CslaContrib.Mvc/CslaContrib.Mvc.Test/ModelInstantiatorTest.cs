using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CslaContrib.Mvc.Test
{
    /// <summary>
    /// Summary description for ModelInstantiatorTest
    /// </summary>
    [TestClass]
    public class ModelInstantiatorTest
    {

        [TestMethod]
        public void MustAtLeastPassReturnTypeAndFactoryMethod()
        {
            var obj = new ModelInstantiator().CallFactoryMethod(null, typeof(MyBO), "NewMyBO", null);
            Assert.IsInstanceOfType(obj, typeof(MyBO));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingWithoutReturnTypeArgumentThrowsArgumentNullException()
        {
            new ModelInstantiator().CallFactoryMethod(null, null, "NewMyBO", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCallingWithoutFactoryMethodArgumentThrowsArgumentNullException()
        {
            new ModelInstantiator().CallFactoryMethod(null, typeof(MyBO), "", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenMethodIsNotFactoryMethodThrowsInvalidOperationException()
        {
            new ModelInstantiator().CallFactoryMethod(typeof(MyBO), typeof(MyBO), "NotAFactoryMethod", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenFactoryMethodNotExistThrowsInvalidOperationException()
        {
            new ModelInstantiator().CallFactoryMethod(typeof(MyBO), typeof(MyBO), "MethodNotExist", null);
        }

        [TestMethod]
        public void CanCallFactoryMethodWithoutArgument()
        {
            var obj = new ModelInstantiator().CallFactoryMethod(null, typeof(MyBO), "NewMyBO", null);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(0, ((MyBO)obj).ID);
        }

        [TestMethod]
        public void CanCallMethodWithinFactoryClassWithoutArgument()
        {
            var fType = typeof (MyFactory);
            var oType = typeof(MyBO);
            var mtd = "NewMyBO";
            object[] argValues = {};

            var obj = new ModelInstantiator().CallFactoryMethod(fType, oType, mtd, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(0, ((MyBO)obj).ID);
        }


        [TestMethod]
        public void CanCallFactoryMethodWithArgument()
        {
            var objType = typeof (MyBO);
            var mtd = "GetMyBO";
            object[] argValues = {1};
            
            var obj = new ModelInstantiator().CallFactoryMethod(objType, objType, mtd, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(1, ((MyBO)obj).ID);
        }


        [TestMethod]
        public void CanCallMethodWithinFactoryClassWithArgument()
        {
            var fType = typeof(MyFactory);
            var oType = typeof(MyBO);
            var mtd = "GetMyBO";
            object[] argValues = { 5 };

            var obj = new ModelInstantiator().CallFactoryMethod(fType, oType, mtd, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(5, ((MyBO)obj).ID);
        }

        [TestMethod]
        public void CanCallsNewMethodByConventionWhenActionMethodIsCreate()
        {
            var action = "Create";
            var type = typeof(MyBO);
            object[] argValues = { };

            var obj = new ModelInstantiator().CallFactoryMethod(action, type, type, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(0, ((MyBO)obj).ID);
        }

        [TestMethod]
        public void CanCallGetMethodByConventionWhenActionMethodIsEdit()
        {
            var action = "Edit";
            var type = typeof(MyBO);
            object[] argValues = { 10 };

            var obj = new ModelInstantiator().CallFactoryMethod(action, type, type, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(10, ((MyBO)obj).ID);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.AmbiguousMatchException))]
        public void ThrowsAmbiguousMatchExceptionWhenMultipleFactoryMethodsAreFoundByConvention()
        {
            var action = "Edit";
            var type = typeof(MyBO);
            object[] argValues = { "100" };

            new ModelInstantiator().CallFactoryMethod(action, type, type, argValues);
        }

        [TestMethod]
        public void CanAddFactoryMapPattern()
        {
            FactoryMethodLocator.PatternMappings.Add(new PatternMap { ActionPattern = "f*", MethodPatterns = new[] { "New*" } });

            var action = "FooFoo";
            var type = typeof(MyBO);
            object[] argValues = {  };

            var obj = new ModelInstantiator().CallFactoryMethod(action, type, type, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(0, ((MyBO)obj).ID);
        }

        [TestMethod]
        public void CanAddFactoryMapPatternNewPattern()
        {
            FactoryMethodLocator.PatternMappings.Add(new PatternMap { ActionPattern = "f*", MethodPatterns = new[] { "Get.*ByX" } });

            var action = "FooFoo";
            var type = typeof(MyBO);
            object[] argValues = { "100" };

            var obj = new ModelInstantiator().CallFactoryMethod(action, type, type, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(100, ((MyBO)obj).ID);
        }

        [TestMethod]
        public void CanCallMethodWithinFactoryClassWithUnknownButConvertibleTypeArgument()
        {
            var fType = typeof(MyFactory);
            var oType = typeof(MyBO);
            var mtd = "GetMyBO";
            object[] argValues = { new[]{"5"} };

            var obj = new ModelInstantiator().CallFactoryMethod(fType, oType, mtd, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(5, ((MyBO)obj).ID);
        }

        [TestMethod]
        public void CanCallMethodWithinFactoryClassWithUnknownButConvertibleTypeArgumentLongToInt()
        {
            var fType = typeof(MyFactory);
            var oType = typeof(MyBO);
            var mtd = "GetMyBO";
            object[] argValues = { new[] { 5L } };

            var obj = new ModelInstantiator().CallFactoryMethod(fType, oType, mtd, argValues);

            Assert.IsInstanceOfType(obj, typeof(MyBO));
            Assert.AreEqual(5, ((MyBO)obj).ID);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowInvalidOperationWhenCallMethodWithUnknownButNotConvertibleTypeArgument()
        {
            var fType = typeof(MyFactory);
            var oType = typeof(MyBO);
            var mtd = "GetMyBO";
            object[] argValues = { new[] { "5A" } };

            new ModelInstantiator().CallFactoryMethod(fType, oType, mtd, argValues);
        }

        class MyBO : Csla.BusinessBase<MyBO>
        {
            public int ID { get; set; }

            public MyBO NotAFactoryMethod() { return this;}
            public static MyBO NewMyBO() { return new MyBO(); }
            public static MyBO GetMyBO(int id) { return new MyBO { ID = id }; }
            public static MyBO GetMyBOByX(string str) { return new MyBO {ID = int.Parse(str)};}
            public static MyBO GetMyBOByY(string str) { return new MyBO { ID = int.Parse(str)}; }
        }

        class MyFactory
        {
            public MyBO NewMyBO() { return new MyBO(); }
            public MyBO GetMyBO(int id) { return new MyBO { ID = id }; }
        }
    }
}
