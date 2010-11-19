using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Composite;
using System.Windows.Threading;
using System.Windows;

namespace PTWpf.Library
{
    public sealed class DelegateCommandReplacement<T> : ICommand, IActiveAware
    {
        public Action<T> Execute { get; set; }
        public Func<T, bool> CanExecute { get; set; }
        private bool _isActive;

        /// <summary>
        /// Erzeugt eine neue Instanz des DelegateCommandReplacements
        /// </summary>
        /// <param name="execute">Delegate der ausgeführt wird, wenn das Execute des Commands aufgerufen wird</param>
        /// <remarks><seealso cref="CanExecute"/> gibt immer true zurück</remarks>
        public DelegateCommandReplacement(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Erzeugt eine neue Instanz des DelegateCommandReplacements
        /// </summary>
        /// <param name="execute">Delegate der ausgeführt wird, wenn das Execute des Commands aufgerufen wird</param>
        /// <param name="canExecute">Delegate der ausgeführt wird wenn CanExecute auf dem COmmand aufgerufen wird</param>
        public DelegateCommandReplacement(Action<T> execute, Func<T, bool> canExecute)
        {
            this.Execute = execute;
            this.CanExecute = canExecute;
        }

        /// <summary>
        /// Weak Event, Meldet sich am RequerySuggested an und wird daher öfter aufgerufen als das normale DelegateCommand.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (CanExecute == null) return true;

            return this.CanExecute((T)parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }

        #region IActiveAware members

        /// <summary>
        /// Fired if the <see cref="IsActive"/> property changes.
        /// </summary>
        public event EventHandler IsActiveChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the object is active.
        /// </summary>
        /// <value><see langword="true" /> if the object is active; otherwise <see langword="false" />.</value>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        /// <summary>
        /// This raises the <see cref="IsActiveChanged"/> event.
        /// </summary>
        protected void OnIsActiveChanged()
        {
            EventHandler isActiveChangedHandler = IsActiveChanged;
            if (isActiveChangedHandler != null) isActiveChangedHandler(this, EventArgs.Empty);
        }

        #endregion

    }
}
