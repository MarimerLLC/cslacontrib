using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;
using OutlookStyleApp.Tests.Mocks;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class ApplicationModelFixture
    {
        private ApplicationModel CreateApplicationModel()
        {
            return new ApplicationModel(null, null);
        }

        [TestMethod]
        public void CanAddUseCases()
        {
            IApplicationModel applicationModel = CreateApplicationModel();
            IActiveAwareUseCaseController mockActiveAwareUseCaseController = new MockUseCase();
            applicationModel.AddMainUseCase(mockActiveAwareUseCaseController);

            Assert.IsTrue(applicationModel.MainUseCases.Contains(mockActiveAwareUseCaseController));
        }

        [TestMethod]
        public void CallingActivateCommandActivatesUseCase()
        {
            ApplicationModel applicationModel = CreateApplicationModel();
            IActiveAwareUseCaseController mockUseCase1 = new MockUseCase();

            applicationModel.AddMainUseCase(mockUseCase1);
            
            Assert.IsFalse(mockUseCase1.IsActive);
            Assert.AreEqual(0, applicationModel.ActiveUseCases.Count());

            applicationModel.ActivateUseCaseCommand.Execute(mockUseCase1);
            Assert.AreEqual(mockUseCase1, applicationModel.ActiveUseCases.FirstOrDefault());

            Assert.IsTrue(mockUseCase1.IsActive);
        }


        [TestMethod]
        public void SettingActiveOnUseCaseWillSetActiveUseCase()
        {
            ApplicationModel applicationModel = CreateApplicationModel();
            IActiveAwareUseCaseController mockUseCase1 = new MockUseCase();
            IActiveAwareUseCaseController mockUseCase2 = new MockUseCase();

            applicationModel.AddMainUseCase(mockUseCase1);
            applicationModel.AddMainUseCase(mockUseCase2);

            Assert.IsFalse(mockUseCase1.IsActive);
            Assert.AreEqual(0, applicationModel.ActiveUseCases.Count());

            mockUseCase1.IsActive = true;
            Assert.AreEqual(mockUseCase1, applicationModel.ActiveUseCases.FirstOrDefault());
            Assert.IsTrue(mockUseCase1.IsActive);
            Assert.IsFalse(mockUseCase2.IsActive);

            mockUseCase2.IsActive = true;
            Assert.AreEqual(mockUseCase2, applicationModel.ActiveUseCases.FirstOrDefault());
            Assert.IsFalse(mockUseCase1.IsActive);
            Assert.IsTrue(mockUseCase2.IsActive);

        }

        [TestMethod]
        public void SettingActiveUseCaseWilSetIsActive()
        {
            ApplicationModel applicationModel = CreateApplicationModel();
            IActiveAwareUseCaseController mockUseCase1 = new MockUseCase();

            applicationModel.AddMainUseCase(mockUseCase1);

            Assert.IsFalse(mockUseCase1.IsActive);
            Assert.AreEqual(0, applicationModel.ActiveUseCases.Count());

            applicationModel.ActivateUseCase(mockUseCase1);

            Assert.AreEqual(mockUseCase1, applicationModel.ActiveUseCases.FirstOrDefault());
            Assert.IsTrue(mockUseCase1.IsActive);
        }

        [TestMethod]
        public void ActivatingOneUseCaseDeactivatesOther()
        {
            ApplicationModel applicationModel = CreateApplicationModel();
            IActiveAwareUseCaseController mockUseCase1 = new MockUseCase();
            IActiveAwareUseCaseController mockUseCase2 = new MockUseCase();

            applicationModel.AddMainUseCase(mockUseCase1);
            applicationModel.AddMainUseCase(mockUseCase2);

            Assert.IsFalse(mockUseCase1.IsActive);
            Assert.AreEqual(0, applicationModel.ActiveUseCases.Count());

            applicationModel.ActivateUseCase(mockUseCase1);

            Assert.AreEqual(mockUseCase1, applicationModel.ActiveUseCases.FirstOrDefault());
            Assert.IsTrue(mockUseCase1.IsActive);

            applicationModel.ActivateUseCase(mockUseCase2);

            Assert.AreEqual(mockUseCase2, applicationModel.ActiveUseCases.FirstOrDefault());
            Assert.IsTrue(mockUseCase2.IsActive);
            Assert.IsFalse(mockUseCase1.IsActive);
        }

        // Still need to test this method..
        //[TestMethod]
        //public void CanCreateViewInScopedRegionManager()
        //{
        //    var mockContainer = new MockUnityContainer();
        //    var mockRegionManager = new MockRegionManager();
        //    ApplicationModel applicationModel = new ApplicationModel(mockContainer, mockRegionManager);

        //    var view = applicationModel.CreateObjectInScopedRegionManager<MockView>();

        //    // Scoped container created
        //    Assert.IsNotNull(mockContainer.ChildContainer);
            
        //    // RegionManager Resolved

        //    // scoped RegionManager created 
        //    Assert.IsNotNull(mockRegionManager.ChildRegionManager);

        //    // scoped regionmanager added to container
        //    Assert.AreSame(mockContainer.InstanceRegistered, mockRegionManager.ChildRegionManager);

        //    // View created by child region manager
        //    Assert.IsInstanceOfType(view, typeof (MockView));
        //}
    }



    //public class MockRegionManager : IRegionManager
    //{
    //    public MockRegionManager ChildRegionManager;

    //    public IRegionManager CreateRegionManager()
    //    {
    //        return ChildRegionManager = new MockRegionManager();
    //    }

    //    public IRegionCollection Regions
    //    {
    //        get { return null; }
    //    }
    //}

    //public class MockUnityContainer : IUnityContainer
    //{
    //    public IUnityContainer ChildContainer;
    //    public object InstanceRegistered;

    //    public void Dispose()
    //    {
    //    }

    //    public IUnityContainer RegisterType<T>(params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers) where TTo : TFrom
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<T>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<T>(string name, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType<T>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type t, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type from, Type to, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type from, Type to, string name, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type from, Type to, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type t, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type t, string name, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type t, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterInstance<TInterface>(TInterface instance)
    //    {
    //        InstanceRegistered = instance;
    //        return this;
    //    }

    //    public IUnityContainer RegisterInstance<TInterface>(TInterface instance, LifetimeManager lifetimeManager)
    //    {
    //        InstanceRegistered = instance;
    //        return this;
    //    }

    //    public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance, LifetimeManager lifetimeManager)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterInstance(Type t, object instance)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterInstance(Type t, object instance, LifetimeManager lifetimeManager)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterInstance(Type t, string name, object instance)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
    //    {
    //        return null;
    //    }

    //    public T Resolve<T>()
    //    {
    //        if (typeof(T) == typeof(IRegionManager))
    //        {
    //            return Activator.CreateInstance(typeof(MockRegionManager)) as T;
    //        }

    //        return default(T);
    //    }

    //    public T Resolve<T>(string name)
    //    {
    //        return default(T);
    //    }

    //    public object Resolve(Type t)
    //    {
    //        return null;
    //    }

    //    public object Resolve(Type t, string name)
    //    {
    //        return null;
    //    }

    //    public IEnumerable<T> ResolveAll<T>()
    //    {
    //        yield break;
    //    }

    //    public IEnumerable<object> ResolveAll(Type t)
    //    {
    //        yield break;
    //    }

    //    public T BuildUp<T>(T existing)
    //    {
    //        return default(T);
    //    }

    //    public T BuildUp<T>(T existing, string name)
    //    {
    //        return default(T);
    //    }

    //    public object BuildUp(Type t, object existing)
    //    {
    //        return null;
    //    }

    //    public object BuildUp(Type t, object existing, string name)
    //    {
    //        return null;
    //    }

    //    public void Teardown(object o)
    //    {
    //    }

    //    public IUnityContainer AddExtension(UnityContainerExtension extension)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer AddNewExtension<TExtension>() where TExtension : UnityContainerExtension, new()
    //    {
    //        return null;
    //    }

    //    public TConfigurator Configure<TConfigurator>() where TConfigurator : IUnityContainerExtensionConfigurator
    //    {
    //        return default(TConfigurator);
    //    }

    //    public object Configure(Type configurationInterface)
    //    {
    //        return null;
    //    }

    //    public IUnityContainer RemoveAllExtensions()
    //    {
    //        return null;
    //    }

    //    public IUnityContainer CreateChildContainer()
    //    {
    //        return ChildContainer = new MockUnityContainer();
    //    }

    //    public IUnityContainer Parent
    //    {
    //        get { return null; }
    //    }
    //}
}