namespace OutlookStyle.Infrastructure
{
    /// <summary>
    ///  INterface for a class that can create a specific instance of an object. This allows
    /// delayed creation of an object, but still have a dependency on the object type. 
    /// </summary>
    public interface IObjectFactory

    {
        /// <summary>
        /// The value of the created object
        /// </summary>
        object Value { get;}

        /// <summary>
        /// Create the specified object. 
        /// </summary>
        /// <returns></returns>
        object CreateInstance();

    }
}