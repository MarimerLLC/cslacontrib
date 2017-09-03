using Caliburn.Micro;

namespace CslaContrib.Caliburn.Micro.V2
{
    /// <summary>
    /// A basic implementation of <see cref="IHaveSubject{T}"/>
    /// </summary>
    /// <typeparam name="T">The screen's type.</typeparam>
    public class ScreenWithSubject<T> : Screen, IHaveSubject<T>
    {
        T _subject;

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <value>The subject.</value>
        object IHaveSubject.Subject
        {
            get { return Subject; }
        }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public virtual T Subject
        {
            get { return _subject; }
        }

        /// <summary>
        /// Configures the screen with the subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Self</returns>
        IHaveSubject IHaveSubject.WithSubject(object subject)
        {
            return WithSubject((T)subject);
        }

        /// <summary>
        /// Configures the screen with the subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Self</returns>
        public virtual IHaveSubject<T> WithSubject(T subject)
        {
            _subject = subject;
            NotifyOfPropertyChange(() => Subject);
            return this;
        }
    }
}
