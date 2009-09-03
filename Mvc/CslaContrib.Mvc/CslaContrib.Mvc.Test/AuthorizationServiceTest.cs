using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web.Routing;
using System.Security.Principal;

namespace CslaContrib.Mvc.Test
{
    /// <summary>
    /// Summary description for AuthorizationServiceTest
    /// </summary>
    [TestClass]
    public class AuthorizationServiceTest
    {

        [TestMethod]
        public void WhenUserNotAuthenticatedReturnsHttpUnauthorizedResult()
        {
            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Expect(i=>i.IsAuthenticated).Returns(false).Verifiable();
            var mockUser = new Mock<IPrincipal>();
            mockUser.Expect(p => p.Identity).Returns(mockIdentity.Object);
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.User).Returns(mockUser.Object);
            
            var route = new RouteData();
            route.Values["action"] = "Detail";

            var mockController = new Mock<ControllerBase>();

            var mockFilterContext = new Mock<AuthorizationContext>();
            mockFilterContext.Expect(c => c.HttpContext).Returns(mockHttpContext.Object);
            mockFilterContext.Expect(c => c.Controller).Returns(mockController.Object);
            mockFilterContext.Expect(c => c.RouteData).Returns(route);

            var svc = new AuthorizationService()
                                           {ModelType = typeof (String), Access = AccessType.Read};

            svc.OnAuthorization(mockFilterContext.Object);

            Assert.IsInstanceOfType(mockFilterContext.Object.Result, typeof(HttpUnauthorizedResult));
            mockIdentity.Verify();
        }

        [TestMethod]
        public void WhenModelTypeNotProvidedFindModelTypeIsCalled()
        {
            var mockHttpContext = new Mock<HttpContextBase>();

            var route = new RouteData();
            route.Values["action"] = "Detail";

            var mockController = new Mock<ControllerBase>();

            var mockFilterContext = new Mock<AuthorizationContext>();
            mockFilterContext.Expect(c => c.HttpContext).Returns(mockHttpContext.Object);
            mockFilterContext.Expect(c => c.Controller).Returns(mockController.Object);
            mockFilterContext.Expect(c => c.RouteData).Returns(route);

            var svc = new FinderAuthorizationService(typeof (string), AccessType.Read);

            svc.OnAuthorization(mockFilterContext.Object);

            Assert.IsTrue(svc.FindModelTypeIsCalled);
            Assert.AreEqual(typeof (string), svc.ModelType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenModelTypeNotAvailableThrowInvalidOperationException()
        {
            var mockHttpContext = new Mock<HttpContextBase>();

            var route = new RouteData();
            route.Values["action"] = "Detail";

            var mockController = new Mock<ControllerBase>();

            var mockFilterContext = new Mock<AuthorizationContext>();
            mockFilterContext.Expect(c => c.HttpContext).Returns(mockHttpContext.Object);
            mockFilterContext.Expect(c => c.Controller).Returns(mockController.Object);
            mockFilterContext.Expect(c => c.RouteData).Returns(route);

            var svc = new FinderAuthorizationService(null, AccessType.Read);

            svc.OnAuthorization(mockFilterContext.Object);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenAccessTypeNotAvailableThrowInvalidOperationException()
        {
            var mockHttpContext = new Mock<HttpContextBase>();

            var route = new RouteData();
            route.Values["action"] = "Detail";

            var mockController = new Mock<ControllerBase>();

            var mockFilterContext = new Mock<AuthorizationContext>();
            mockFilterContext.Expect(c => c.HttpContext).Returns(mockHttpContext.Object);
            mockFilterContext.Expect(c => c.Controller).Returns(mockController.Object);
            mockFilterContext.Expect(c => c.RouteData).Returns(route);

            var svc = new FinderAuthorizationService(typeof(String), null);

            svc.OnAuthorization(mockFilterContext.Object);

        }

        [TestMethod]
        public void WhenModelTypeNotProvidedFindModelTypeReturnsMyBoType()
        {
            var controller = new TestController();
            var mockHttpContext = new Mock<HttpContextBase>();

            Mock<ControllerContext> mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Expect(c => c.Controller).Returns(controller);
            mockControllerContext.Expect(c => c.HttpContext).Returns(mockHttpContext.Object);

            controller.ControllerContext = mockControllerContext.Object;

            var svc = new TestFindModelType();
            var result = svc.PublicFindModelType(controller, "Edit");

            Assert.AreEqual(typeof(MyBO), result);
        }

        private class FinderAuthorizationService : AuthorizationService
        {
            private Type _expectedModelType;
            private AccessType? _expectedAccessType;
            public FinderAuthorizationService(Type expectedModelType, AccessType? expectedAccess)
            {
                _expectedModelType = expectedModelType;
                _expectedAccessType = expectedAccess;
            }

            public bool FindModelTypeIsCalled = false;
            protected override Type FindModelType(ControllerBase controller, string actionName)
            {
                FindModelTypeIsCalled = true;
                return _expectedModelType;
            }

            public bool FindAccessTypeIsCalled = false;
            protected override AccessType? FindAccessType(ControllerBase controller, string actionName)
            {
                FindAccessTypeIsCalled = true;
                return _expectedAccessType;
            }
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                return true;
            }
            protected override void OnAuthorizationBase(AuthorizationContext filterContext)
            {
                /* no-op */
            }
        }

        private class TestFindModelType : AuthorizationService
        {
            public Type PublicFindModelType(ControllerBase controller, string actionName)
            {
                return base.FindModelType(controller, actionName);
            }
        }

        private class TestController : Controller
        {
            public void Edit(int id, MyBO bo) { }
        }

        private class MyBO : Csla.BusinessBase<MyBO>
        { }

    }
}
