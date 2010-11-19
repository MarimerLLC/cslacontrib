using System;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csla;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CslaContrib.Mvc.Test
{
    /// <summary>
    /// Summary description for DefaultCslaModelBinderTest
    /// </summary>
    [TestClass]
    public class DefaultCslaModelBinderTest
    {
        public DefaultCslaModelBinderTest()
        {
            ModelBinders.Binders.DefaultBinder = new DefaultCslaModelBinder();
        }

        [TestMethod]
        public void ShouldMapAllPropertiesFromRequestForm()
        {
            var ctx = GetControllerContext();
            var form = ctx.HttpContext.Request.Form;
            form.Add("employee.id", "123456");
            form.Add("employee.Name", "John");
            form.Add("employee.salary", "45000.00");
            form.Add("employee.hiredate", "12/25/2005");

            form.Add("employee.homeaddress.street", "123 Nowhere Lane");
            form.Add("employee.homeaddress.city", "Nowhere");
            form.Add("employee.homeaddress.state", "AK");
            form.Add("employee.homeaddress.zip", "99999");

            form.Add("employee.workaddress.street", "1 Microsoft Way");
            form.Add("employee.workaddress.city", "Redmond");
            form.Add("employee.workaddress.state", "WA");
            form.Add("employee.workaddress.zip", "98052");

            form.Add("employee.dependents[0].id", "9");
            form.Add("employee.dependents[0].name", "Son");

            form.Add("employee.dependents[1].id", "8");
            form.Add("employee.dependents[1].name", "Wife");

            var binder = new DefaultCslaModelBinder();
            var model = Employee.GetEmployee(1);
            var modelContext = new ModelBindingContext()
                                   {
                                       Model = model,
                                       ModelType = typeof (Employee),
                                       ModelName = "employee",
                                       ValueProvider = new ValueProviderDictionary(ctx)
                                   };
            var result = binder.BindModel(ctx, modelContext);

            Employee o = (Employee)result;

            Assert.AreEqual(123456, o.ID);
            Assert.AreEqual("John", o.Name);
            Assert.AreEqual(45000m, o.Salary);
            Assert.AreEqual(new SmartDate("12/25/2005"), o.HireDate);

            Assert.AreEqual("123 Nowhere Lane", o.HomeAddress.Street);
            Assert.AreEqual("Nowhere", o.HomeAddress.City);
            Assert.AreEqual("AK", o.HomeAddress.State);
            Assert.AreEqual(99999, o.HomeAddress.Zip);

            Assert.IsNotNull(o.WorkAddress, "Work address should not be null.");
            Assert.AreEqual("1 Microsoft Way", o.WorkAddress.Street);
            Assert.AreEqual("Redmond", o.WorkAddress.City);
            Assert.AreEqual("WA", o.WorkAddress.State);
            Assert.AreEqual(98052, o.WorkAddress.Zip);

            Assert.AreEqual(9, o.Dependents[0].ID);
            Assert.AreEqual("Son", o.Dependents[0].Name);
            Assert.AreEqual(8, o.Dependents[1].ID);
            Assert.AreEqual("Wife", o.Dependents[1].Name);
        }

        [TestMethod]
        public void ShouldMapAllPropertiesFromRequestFormWithoutPrefix()
        {
            var ctx = GetControllerContext();
            var form = ctx.HttpContext.Request.Form;
            form.Add("id", "123456");
            form.Add("Name", "John");
            form.Add("salary", "45000.00");
            form.Add("hiredate", "12/25/2005");

            form.Add("homeaddress.street", "123 Nowhere Lane");
            form.Add("homeaddress.city", "Nowhere");
            form.Add("homeaddress.state", "AK");
            form.Add("homeaddress.zip", "99999");

            form.Add("workaddress.street", "1 Microsoft Way");
            form.Add("workaddress.city", "Redmond");
            form.Add("workaddress.state", "WA");
            form.Add("workaddress.zip", "98052");

            form.Add("dependents[0].id", "9");
            form.Add("dependents[0].name", "Son");

            form.Add("dependents[1].id", "8");
            form.Add("dependents[1].name", "Wife");

            var binder = new DefaultCslaModelBinder();
            var model = Employee.GetEmployee(1);
            var modelContext = new ModelBindingContext()
                                    {
                                        Model = model,
                                        ModelType = typeof(Employee),
                                        ModelName = "employee",
                                        ValueProvider = new ValueProviderDictionary(ctx),
                                        FallbackToEmptyPrefix = true
                                    };
            var result = binder.BindModel(ctx, modelContext);

            Employee o = (Employee)result;

            Assert.AreEqual(123456, o.ID);
            Assert.AreEqual("John", o.Name);
            Assert.AreEqual(45000m, o.Salary);
            Assert.AreEqual(new SmartDate("12/25/2005"), o.HireDate.Date);

            Assert.AreEqual("123 Nowhere Lane", o.HomeAddress.Street);
            Assert.AreEqual("Nowhere", o.HomeAddress.City);
            Assert.AreEqual("AK", o.HomeAddress.State);
            Assert.AreEqual(99999, o.HomeAddress.Zip);

            Assert.IsNotNull(o.WorkAddress, "Work address should not be null.");
            Assert.AreEqual("1 Microsoft Way", o.WorkAddress.Street);
            Assert.AreEqual("Redmond", o.WorkAddress.City);
            Assert.AreEqual("WA", o.WorkAddress.State);
            Assert.AreEqual(98052, o.WorkAddress.Zip);

            Assert.AreEqual(9, o.Dependents[0].ID);
            Assert.AreEqual("Son", o.Dependents[0].Name);
            Assert.AreEqual(8, o.Dependents[1].ID);
            Assert.AreEqual("Wife", o.Dependents[1].Name);
        }

        [TestMethod]
        public void ShouldTryToCallCslaFactoryWhenModelIsEmpty()
        {
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod("Create", typeof(Employee), typeof(Employee), It.IsAny<object[]>()))
                .Returns(Employee.NewEmployee())
                .Verifiable();

            var ctx = GetControllerContext();
            ctx.RouteData.Values["action"] = "Create";

            var form = ctx.HttpContext.Request.Form;
            form.Add("employee.id", "123456");
            form.Add("employee.Name", "John");

            var binder = new DefaultCslaModelBinder(mockInst.Object);
            var modelContext = new ModelBindingContext()
            {
                Model = null,
                ModelType = typeof(Employee),
                ModelName = "employee",
                ValueProvider = new ValueProviderDictionary(ctx)
            };

            var result = binder.BindModel(ctx, modelContext);

            Employee o = (Employee)result;

            Assert.AreEqual(123456, o.ID);
            Assert.AreEqual("John", o.Name);
            mockInst.VerifyAll();
        }

        [TestMethod]
        public void ShouldMapBusinessCollection()
        {
            var mockInst = new Mock<IModelInstantiator>();
            mockInst
                .Expect(i => i.CallFactoryMethod("Edit", typeof(DevEmployees), typeof(DevEmployees), It.IsAny<object[]>()))
                .Returns(DevEmployees.GetDevEmployees(1))
                .Verifiable();

            var ctx = GetControllerContext();
            ctx.RouteData.Values["action"] = "Edit";

            var form = ctx.HttpContext.Request.Form;
            form.Add("devemployees[0].id", "123456");
            form.Add("devemployees[0].Name", "John");
            form.Add("devemployees[1].id", "567890");
            form.Add("devemployees[1].Name", "Bob");

            var binder = new DefaultCslaModelBinder(mockInst.Object);
            var modelContext = new ModelBindingContext()
            {
                Model = null,
                ModelType = typeof(DevEmployees),
                ModelName = "devEmployees",
                ValueProvider = new ValueProviderDictionary(ctx)
            };

            var result = binder.BindModel(ctx, modelContext);

            DevEmployees o = (DevEmployees)result;

            Assert.AreEqual(123456, o[0].ID);
            Assert.AreEqual("John", o[0].Name);
            Assert.AreEqual(567890, o[1].ID);
            Assert.AreEqual("Bob", o[1].Name);
            mockInst.VerifyAll();
            
        }

        [TestMethod]
        public void ModelStateShouldBeInvalidWhenBindToPropertyRaisedException()
        {
            var ctx = GetControllerContext();
            var form = ctx.HttpContext.Request.Form;
            form.Add("id", "123456");
            form.Add("UnauthorizedProperty", "Test");

            var binder = new DefaultCslaModelBinder();
            var model = Employee.GetEmployee(1);
            var modelContext = new ModelBindingContext()
            {
                Model = model,
                ModelType = typeof(Employee),
                ModelName = "employee",
                ValueProvider = new ValueProviderDictionary(ctx),
                FallbackToEmptyPrefix = true
            };
            var result = binder.BindModel(ctx, modelContext);

            Employee o = (Employee)result;

            Assert.AreEqual(123456, o.ID);
            Assert.IsFalse(modelContext.ModelState.IsValid);
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
    }

    public class Employee : Csla.BusinessBase<Employee>
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Address HomeAddress { get; private set; }
        public Address WorkAddress { get; set; }

        public decimal Salary { get; set; }
        public SmartDate HireDate { get; set; }

        public string UnauthorizedProperty
        {
            get { return "UnauthorizedProperty";}
            set { throw new UnauthorizedAccessException("unauthorize to assign"); }
        }
        public DependentCollection Dependents { get; private set; }

        private Employee()
        {
            HomeAddress = Address.NewAddress();
            WorkAddress = Address.NewAddress();
            Dependents = DependentCollection.NewDependentCollection();
        }

        public static Employee NewEmployee() { return new Employee(); }
        public static Employee GetEmployee(int id)
        {
            var e = new Employee() {ID = id};
            e.Dependents.Add(Dependent.NewDependent());
            e.Dependents.Add(Dependent.NewDependent());
            return e;
        }
    }

    public class Address : Csla.BusinessBase<Employee>
    {
        private Address() { }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }

        public static Address NewAddress() { return new Address(); }
    }
    public class  DependentCollection : Csla.BusinessListBase<DependentCollection, Dependent>
    {
        private DependentCollection() { }
        public static DependentCollection NewDependentCollection() { return new DependentCollection();}
    }
    public class  Dependent : Csla.BusinessBase<Dependent>
    {
        public int ID { get; set; }
        public string Name { get; set; }

        private Dependent() {}
        public static Dependent NewDependent() { return new Dependent();}
    }

    public class DevEmployees : Csla.BusinessListBase<DevEmployees, Employee>
    {
        protected override object AddNewCore()
        {
            var newEmployee = Employee.NewEmployee();
            this.Add(newEmployee);
            return newEmployee;
        }
        public static DevEmployees NewDevEmployees() { return new DevEmployees();}
        public static DevEmployees GetDevEmployees(int companyID)
        {
            var emps = new DevEmployees();
            emps.AddNewCore();
            emps.AddNewCore();
            return emps;
        }
    }
}
