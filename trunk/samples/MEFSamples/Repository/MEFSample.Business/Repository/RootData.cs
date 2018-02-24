namespace MEFSample.Business.Repository
{
  public interface IRootDataAccess
  {
    RootData Get(int id);
  }

  public class RootData
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}