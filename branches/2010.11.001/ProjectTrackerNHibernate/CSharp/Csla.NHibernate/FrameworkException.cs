using System;

namespace Csla.NHibernate
{
    /// <summary>
    /// Represents an <see cref="Exception"/> that occured in the framework.
    /// </summary>
    [Serializable]
    public class FrameworkException : Exception
    {
        /// <overloads>
        /// Overload.  Initializes a new instance of the <see cref="FrameworkException"/> class.
        /// </overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkException"/> class
        /// with a specified format string and list of parameters.
        /// </summary>
        /// <param name="formatString">A format string.</param>
        /// <param name="parameterList">An array of parameters to be used in the format string.</param>
        public FrameworkException(string formatString, params object[] parameterList)
            : this(string.Format(formatString, parameterList))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FrameworkException(string message) : base(message)
        {
        }
    }
}