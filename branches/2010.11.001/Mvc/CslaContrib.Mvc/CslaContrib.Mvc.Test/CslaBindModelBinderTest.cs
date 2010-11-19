using System;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CslaContrib.Mvc.Test
{
    /// <summary>
    /// Summary description for CustomCslaModelBinderTest
    /// </summary>
    [TestClass]
    public class CslaBindModelBinderTest
    {
        [TestMethod]
        public void BindCallsCorrectMethodOnFactoryMethodWithoutArgument()
        {
            var fMethod = "NewMyBO";
            var attr = new CslaBindAttribute { Method = fMethod };
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod(typeof(MyBO), typeof(MyBO), fMethod, It.Is<object[]>(arg => arg == null || arg.Length == 0)))
                .Returns(MyBO.NewMyBO)
                .Verifiable();

            var binder = new CslaBindModelBinder(attr, mockInst.Object);
            var modelContext = new ModelBindingContext()
                                {
                                    ModelType = typeof(MyBO),
                                    ModelName = "MyBO",
                                    ValueProvider = new ValueProviderDictionary(null),
                                    FallbackToEmptyPrefix = true
                                };

            var result = binder.BindModel(null, modelContext);

            Assert.IsInstanceOfType(result, typeof(MyBO));
            mockInst.Verify();
        }

        [TestMethod]
        public void BindCallsCorrectMethodOnFactoryMethodWithArgument()
        {
            var fMethod = "GetMyBO";
            var attr = new CslaBindAttribute { Method = fMethod, Arguments = "id" };
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod(typeof(MyBO), typeof(MyBO), fMethod, It.Is<object[]>(arg => arg.Length == 1 && (int)arg[0] == 10)))
                .Returns(MyBO.GetMyBO(10))
                .Verifiable();

            var binder = new CslaBindModelBinder(attr, mockInst.Object);

            var modelContext = new ModelBindingContext()
                                {
                                    ModelType = typeof(MyBO),
                                    ModelName = "MyBO",
                                    ValueProvider = new ValueProviderDictionary(null) {
                                                        { "id", new ValueProviderResult("10", "10", null) }
                                                    },
                                    FallbackToEmptyPrefix = true
                                };

            var result = binder.BindModel(null, modelContext);

            Assert.IsInstanceOfType(result, typeof(MyBO));
            mockInst.Verify();
        }

        [TestMethod]
        public void BindTriesToCallBasedOnActionWhenFactoryMethodIsEmpty()
        {
            var actionName = "Create";
            var attr = new CslaBindAttribute { Method = "" };
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod(actionName, typeof(MyBO), typeof(MyBO), It.Is<object[]>(arg => arg == null || arg.Length == 0)))
                .Returns(MyBO.NewMyBO)
                .Verifiable();

            var binder = new CslaBindModelBinder(attr, mockInst.Object);

            var controllerContext = GetControllerContext();
            controllerContext.RouteData.Values["action"] = actionName;

            var modelContext = new ModelBindingContext()
                                {
                                    ModelType = typeof(MyBO),
                                    ModelName = "MyBO",
                                    ValueProvider = new ValueProviderDictionary(null),
                                    FallbackToEmptyPrefix = true
                                };

            var result = binder.BindModel(controllerContext, modelContext);

            Assert.IsInstanceOfType(result, typeof(MyBO));
            mockInst.Verify();
        }

        [TestMethod]
        public void BindTriesToCallBasedOnActionWhenFactoryMethodIsEmptyWithArgument()
        {
            var actionName = "Edit";
            var attr = new CslaBindAttribute { Method = "", Arguments = "id" };
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod(actionName, typeof(MyBO), typeof(MyBO), It.Is<object[]>(arg => arg.Length == 1 && (int)arg[0] == 10)))
                .Returns(MyBO.GetMyBO(10))
                .Verifiable();

            var binder = new CslaBindModelBinder(attr, mockInst.Object);

            var controllerContext = GetControllerContext();
            controllerContext.RouteData.Values["action"] = actionName;

            var modelContext = new ModelBindingContext()
            {
                ModelType = typeof(MyBO),
                ModelName = "MyBO",
                ValueProvider = new ValueProviderDictionary(null) {
                                    { "id", new ValueProviderResult("10", "10", null) }
                                },
                FallbackToEmptyPrefix = true
            };

            var result = binder.BindModel(controllerContext, modelContext);

            Assert.IsInstanceOfType(result, typeof(MyBO));
            mockInst.Verify();
        }


        private static ControllerContext GetControllerContext()
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Expect(r => r.Form).Returns(new NameValueCollection(StringComparer.OrdinalIgnoreCase));
            mockRequest.Expect(r => r.QueryString).Returns(new NameValueCollection(StringComparer.OrdinalIgnoreCase));
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Expect(c => c.Request).Returns(mockRequest.Object);
            var ctx = new ControllerContext(mockContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            return ctx;
        }

        private static IModelInstantiator GetModelInstantiator()
        {
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod(typeof(MyBO), typeof(MyBO), "NewMyBO", It.IsAny<object[]>()))
                .Returns(MyBO.NewMyBO);
            return mockInst.Object;
        }

        class MyBO : Csla.BusinessBase<MyBO>
        {
            public int ID { get; set; }
            public static MyBO NewMyBO() { return new MyBO(); }
            public static MyBO GetMyBO(int id) { return new MyBO { ID = id }; }
        }

    }
}
