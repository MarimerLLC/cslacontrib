namespace MEFSample.Business.FactoryInterfaces
{
  public interface IMyRootFactory
  {
    object Create();

    object Fetch(object criteria);
  }
}