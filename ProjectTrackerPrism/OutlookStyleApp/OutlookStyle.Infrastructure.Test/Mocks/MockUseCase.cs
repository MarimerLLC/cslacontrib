using OutlookStyle.Infrastructure.UseCase;

namespace OutlookStyleApp.Tests.Mocks
{
    internal class MockUseCase : ActiveAwareUseCaseController
    {
        public MockUseCase() : base(new MockViewToRegionBinder())
        {
            
        }

        public override string Name
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}