using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyCsla.Windows
{
    /// <summary>
    /// Helperclass to make statusstrip utdates easier
    /// </summary>
    public partial class StatusStripHelper : Component
    {
        #region Properties

        /// <summary>
        /// Gets or sets the status strip extender to use with this component.
        /// </summary>
        /// <value>My status strip extender.</value>
        public StatusStripExtender MyStatusStripExtender { get; set; }

        /// <summary>
        /// Gets or sets the parent form.
        /// </summary>
        /// <value>The parent form.</value>
        public Form ParentForm { get; set; }

        // delegate declaration for the status strip
        private delegate void StatusStripDelegate(string message, bool showProgressIndicator, bool showLargeProgressIndicator);

        private Timer _progressIndicatorTimer;

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusStripHelper"/> class.
        /// </summary>
        public StatusStripHelper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusStripHelper"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StatusStripHelper(IContainer container) : this()
        {
            container.Add(this);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Stops the progress indicator from running
        /// </summary>
        public void SetStatus()
        {
            SetStatus(string.Empty);
        }

        /// <summary>
        /// Updates the status bar with the specified message. The message is reset after a few seconds.
        /// Progress indicator is NOT shown
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatParams">Formatting parameters</param>
        public void SetStatus(string message, params object[] formatParams)
        {
            var formattedMessage = string.Format(message, formatParams);
            UpdateStatusStrip(formattedMessage, false, false);
        }

        /// <summary>
        /// Updates the status bar with the specified message. The message is not reset until "SetStatus()" is called
        /// Progress indicator IS shown
        /// Large IS shown
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatParams">The format params.</param>
        public void SetStatusWaiting(string message, params object[] formatParams)
        {
            SetStatusWaiting(message, true, formatParams);
        }

        /// <summary>
        /// Updates the status bar with the specified message. The message is not reset until "SetStatus()" is called
        /// Progress indicator IS shown
        /// Large progress indicator is displayed depending on "displayLargeProgressIndicator"
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="displayLargeProgressindicator">if set to <c>true</c> [display large progressindicator].</param>
        /// <param name="formatParams">Formatting parameters</param>
        public void SetStatusWaiting(string message, bool displayLargeProgressindicator, params object[] formatParams)
        {
            var formattedMessage = string.Format(message, formatParams);
            UpdateStatusStrip(formattedMessage, true, displayLargeProgressindicator);
        }

        /// <summary>
        /// Updates the status strip.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="showProgressIndicator">if set to <c>true</c> [show progress indicator].</param>
        /// <param name="showLargeProgressIndicator">if set to <c>true</c> [show large progress indicator].</param>
        public void UpdateStatusStrip(string message, bool showProgressIndicator, bool showLargeProgressIndicator)
        {
            if (ParentForm.InvokeRequired)
            {
                ParentForm.Invoke(new StatusStripDelegate(UpdateStatusStrip), message, showProgressIndicator, showLargeProgressIndicator);
            }
            lock (MyStatusStripExtender)
            {
                if (_progressIndicatorTimer != null)
                    _progressIndicatorTimer.Stop();
                SplashPanel.Close();

                if (showProgressIndicator)
                {
                    MyStatusStripExtender.SetStatusStatic(message);

                    if (showLargeProgressIndicator)
                    {
                        //If still waiting after 2 seconds, show a larger progressindicator
                        _progressIndicatorTimer = new Timer {Interval = 2000};
                        _progressIndicatorTimer.Tick += TimerShowBusyIndicator;
                        _progressIndicatorTimer.Start();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(message))
                        MyStatusStripExtender.SetStatusToDefault();
                    else
                        MyStatusStripExtender.SetStatus(message);
                }
                MyStatusStripExtender.AnimationVisible = showProgressIndicator;
            }
        }

 
        #endregion

        #region Private Methods

        /// <summary>
        /// Hides the temporary wait indicator.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HideTemporaryWaitIndicator(object sender, EventArgs e)
        {
            lock (MyStatusStripExtender)
            {
                var timer = sender as Timer;
                timer.Stop();
                SplashPanel.Close();
                MyStatusStripExtender.SetStatusToDefault();
                MyStatusStripExtender.AnimationVisible = false;
            }
        }

        /// <summary>
        /// Handles the Tick event of the _progressIndicatorTimer
        /// Displays a large progressindicator
        /// </summary>
        /// <param name="sender">The timer that triggered the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TimerShowBusyIndicator(object sender, EventArgs e)
        {
            lock (MyStatusStripExtender)
            {
                var timer = sender as Timer;
                timer.Stop();
                SplashPanel.Show(ParentForm, MyStatusStripExtender.StatusControl.Text);
            }
        }

        #endregion
    }
}
