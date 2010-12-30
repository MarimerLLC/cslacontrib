using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace CslaContrib.Windows
{

    /// <summary>
    /// Add link to StatusStrip with Status and Animation.
    /// <example>
    /// 1) Add a StatusStrip to the form and add 2 ToolStripStatusLabels.
    /// 2) Add the StatusStripExtender to the form.
    /// 3) Set statusStripExtender properties on StatusStrip
    ///        StatusLabel  to toolStripStatusLabel1
    ///    and AnimationLabel to toolStripStatusLabel2
    /// 4) Set status text with 
    ///         statusStripExtender1.SetStatus
    ///         statusStripExtender1.SetStatusStatic   
    ///         statusStripExtender1.SetStatusWaiting
    ///         statusStripExtender1.SetStatusWithDuration   
    /// </example>
    /// 
    /// </summary>
    [ProvideProperty("StatusLabel", typeof(Control))]
    [ProvideProperty("AnimationLabel", typeof(Control))]
    public class StatusStripExtender : Component, IExtenderProvider
    {
        private ToolStripStatusLabel _status;
        private ToolStripStatusLabel _animation;
        private string _statusDefault = "Ready";
        private readonly Timer _timer;
        private Timer _progressIndicatorTimer;
        private int _statusDefaultDuration = 5000;
        private readonly StringCollection _toolTipList;
        private int _maximumToolTipLines = 5;
        private object _syncRoot = new object();

        #region --- Interface IExtenderProvider ----

        public bool CanExtend(object extendee)
        {
            return extendee is StatusStrip;
        }

        #endregion

        #region --- Extender properties ---

        [Category("StatusStripExtender")]
        public ToolStripStatusLabel GetStatusLabel(Control control)
        {
            return StatusControl;
        }

        [Category("StatusStripExtender")]
        public void SetStatusLabel(Control control, ToolStripStatusLabel statusLabel)
        {
            StatusControl = statusLabel;
        }

        [Category("StatusStripExtender")]
        public ToolStripStatusLabel GetAnimationLabel(Control control)
        {
            return AnimationControl;
        }

        [Category("StatusStripExtender")]
        public void SetAnimationLabel(Control control, ToolStripStatusLabel animationLabel)
        {
            AnimationControl = animationLabel;
        }

        #endregion
        #region // --- Constructor ---

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusStripExtender"/> class.
        /// </summary>
        public StatusStripExtender()
        {
            _timer = new Timer { Enabled = false, Interval = _statusDefaultDuration };
            _timer.Tick += _timer_Tick;

            _toolTipList = new StringCollection();
        }


        #endregion

        #region // --- Public properties  ---



        /// <summary>
        /// Gets or sets the ToolStripStatusLabel for Status.
        /// </summary>
        /// <value>The status control.</value>
        [Category("ToolStrip"), Description("Gets or sets the ToolStripStatusLabel for Status.")]
        public ToolStripStatusLabel StatusControl
        {
            set
            {
                if (value == null && _status != null)
                {
                    ReleaseStatus();
                }
                else
                {
                    if (_animation == value && value != null)
                    {
                        throw new ArgumentException("StatusControl and AnimationControl can't be the same control.");
                    }
                    _status = value;
                    InitStatus();
                }
            }
            get
            {
                return _status;
            }
        }

        /// <summary>
        /// Gets or sets the default Status.
        /// </summary>
        /// <value>The status default text.</value>
        [Category("StatusControl"), Description("Gets or sets the default Status.")]
        [Localizable(true)]
        [DefaultValue("Klar")]
        public string StatusDefault
        {
            set
            {
                _statusDefault = value;
                SetStatusToDefault();
            }
            get
            {
                return _statusDefault;
            }
        }


        /// <summary>
        /// Gets or sets the maximum Tool Tip Lines to show (1-10, default 5).
        /// </summary>
        /// <value>The maximum number of Tool Tip Lines.</value>
        [Category("StatusControl"), Description("Maximum lines to show in the ToolTip text. Valid value is 1 to 10.")]
        [DefaultValue(5)]
        public int MaximumToolTipLines
        {
            set
            {
                _maximumToolTipLines = value;
                if (_maximumToolTipLines < 1)
                {
                    _maximumToolTipLines = 1;
                }
                if (_maximumToolTipLines > 10)
                {
                    _maximumToolTipLines = 10;
                }
            }
            get
            {
                return _maximumToolTipLines;
            }
        }

        private void SetStatusToolTip(string text)
        {
            if (!DesignMode)
            {
                _toolTipList.Insert(0, "(" + DateTime.Now.ToLongTimeString() + ") " + text);
                if (_toolTipList.Count > _maximumToolTipLines)
                {
                    _toolTipList.RemoveAt(_maximumToolTipLines);
                }

                string[] toolTipArray = new string[_maximumToolTipLines];
                _toolTipList.CopyTo(toolTipArray, 0);
                _status.ToolTipText = string.Join("\n", toolTipArray).TrimEnd();
            }
        }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        [Category("StatusControl"), Description("Delay in milliseconds to show the Status")]
        [DefaultValue(5000)]
        public int Delay
        {
            set
            {
                _statusDefaultDuration = value;
                _timer.Interval = _statusDefaultDuration;
            }
            get
            {
                return _statusDefaultDuration;
            }
        }

        public void SetStatusToDefault()
        {
            UpdateStatusStrip(_statusDefault, false, false, -1);
        }

        #endregion

        #region --- Public funtions  --

        #region // --- Public Animation ---

        /// <summary>
        /// Gets or sets the Animation control.
        /// </summary>
        /// <value>The animation control.</value>
        [Category("ToolStrip"), Description("Gets or sets the Animation control.")]
        public ToolStripStatusLabel AnimationControl
        {
            set
            {
                if (value == null && _animation != null)
                {
                    ReleaseAnimation();
                }
                else
                {
                    if (_status == value && value != null)
                    {
                        throw new ArgumentException("AnimationControl and StatusControl can't be the same control.");
                    }
                    _animation = value;
                    InitAnimation();
                }
            }
            get
            {
                return _animation;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Animation control is visible].
        /// </summary>
        /// <value><c>true</c> if the Animation control is visible; otherwise, <c>false</c>.</value>        
        [Browsable(false)]
        [ReadOnly(true)]
        [DefaultValue(true)]
        public bool AnimationVisible
        {
            set
            {
                if (_animation != null)
                {
                    _animation.Visible = value;
                }
            }
            get
            {
                if (_animation != null)
                {
                    return _animation.Visible;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion


        /// <summary>
        /// Sets the statu to defaultstatus and stop progress indicator from running.
        /// </summary>
        public void SetStatus()
        {
            SetStatus(_statusDefault);
        }

        /// <summary>
        /// Set status message and stops the progress indicator from running
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetStatus(string text)
        {
            SetStatus(text, _statusDefaultDuration);
        }

        /// <summary>
        /// Set status message and stops the progress indicator from running
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="durationMilliseconds">The duration milliseconds.</param>
        public void SetStatus(string text, int durationMilliseconds)
        {
            UpdateStatusStrip(text, false, false, durationMilliseconds);
        }

        /// <summary>
        /// Sets the Status and keep it until new text.
        /// </summary>
        public void SetStatusStatic()
        {
            SetStatusStatic(_statusDefault);
        }

        /// <summary>
        /// Sets the Status and keep it until new text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetStatusStatic(string text)
        {
            UpdateStatusStrip(text, false, false, -1);
        }


        /// <summary>
        /// Sets the duration of the status with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="duration">The duration.</param>
        public void SetStatusWithDuration(string text, int duration)
        {
            UpdateStatusStrip(text, false, false, duration);
        }

        /// <summary>
        /// Updates the status bar with the specified message. The message is reset after a few seconds.
        /// Progress indicator is NOT shown
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatParams">Formatting parameters</param>
        public void SetStatusText(string message, params object[] formatParams)
        {
            var formattedMessage = string.Format(message, formatParams);
            UpdateStatusStrip(formattedMessage, false, false, _statusDefaultDuration);
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
            UpdateStatusStrip(formattedMessage, true, displayLargeProgressindicator, -1);
        }

        /// <summary>
        /// Updates the status strip.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="showProgressIndicator">if set to <c>true</c> [show progress indicator].</param>
        /// <param name="showLargeProgressIndicator">if set to <c>true</c> [show large progress indicator].</param>
        /// <param name="durationMilliseconds"></param>
        public void UpdateStatusStrip(string message, bool showProgressIndicator, bool showLargeProgressIndicator, int durationMilliseconds)
        {
            //if (this..InvokeRequired)
            //{
            //  ParentForm.Invoke(new StatusStripDelegate(UpdateStatusStrip), message, showProgressIndicator, showLargeProgressIndicator);
            //}
            lock (_syncRoot)
            {
                if (_progressIndicatorTimer != null)
                    _progressIndicatorTimer.Stop();
                SplashPanel.Close();

                if (showProgressIndicator)
                {
                    SetStatusTextStaticPrivate(message);

                    if (showLargeProgressIndicator)
                    {
                        //If still waiting after 2 seconds, show a larger progressindicator
                        _progressIndicatorTimer = new Timer { Interval = 2000 };
                        _progressIndicatorTimer.Tick += TimerShowBusyIndicator;
                        _progressIndicatorTimer.Start();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(message))
                        SetStatusToDefaultPrivate();
                    else
                        SetStatusTextPrivate(message, durationMilliseconds);
                }
                AnimationVisible = showProgressIndicator;
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the Status.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SetStatusTextPrivate(string text)
        {
            SetStatusWithDurationPrivate(text, _statusDefaultDuration);
        }

        /// <summary>
        /// Sets the Status.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="durationMilliseconds">The duration milliseconds.</param>
        private void SetStatusTextPrivate(string text, int durationMilliseconds)
        {
            SetStatusWithDurationPrivate(text, durationMilliseconds);
        }

        /// <summary>
        /// Sets the Status and keep it until new text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SetStatusTextStaticPrivate(string text)
        {
            SetStatusWithDurationPrivate(text, -1);
        }


        /// <summary>
        /// Sets the Status with delay.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="durationMilliseconds">The duration in milliseconds.</param>
        private void SetStatusWithDurationPrivate(string text, int durationMilliseconds)
        {
            _status.Text = text;
            SetStatusToolTip(text);
            if (durationMilliseconds < 0)
            {
                _timer.Enabled = false;
                _timer.Interval = _statusDefaultDuration;
            }
            else
            {
                if (_timer.Enabled)
                {
                    _timer.Enabled = false;
                    _timer.Interval = durationMilliseconds;
                    _timer.Enabled = true;
                }
                else
                {
                    _timer.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Sets the status to Default.
        /// </summary>
        private void SetStatusToDefaultPrivate()
        {
            if (_status != null)
            {
                if (_status.Text != _statusDefault || _toolTipList.Count == 0)
                {
                    _status.Text = _statusDefault;
                    SetStatusToolTip(_statusDefault);
                }
            }
        }

        /// <summary>
        /// Hides the temporary wait indicator.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HideTemporaryWaitIndicator(object sender, EventArgs e)
        {
            lock (_syncRoot)
            {
                var timer = sender as Timer;
                if (timer != null) timer.Stop();
                SplashPanel.Close();
                SetStatusToDefault();
                AnimationVisible = false;
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
            lock (_syncRoot)
            {
                var timer = sender as Timer;
                if (timer != null) timer.Stop();
                SplashPanel.Show((AnimationControl).Owner.FindForm(), StatusControl.Text);
            }
        }


        #endregion

        #region // --- Private Status ---

        private void InitStatus()
        {
            if (_status != null)
            {
                SetStatusToDefault();
                _status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                _status.Spring = true;
                if (DesignMode)
                {
                    _status.Owner.ShowItemToolTips = true;
                }
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            SetStatusToDefault();
            _timer.Interval = _statusDefaultDuration;
        }

        private void ReleaseStatus()
        {
            if (_status != null)
            {
                _status.Text = "# Not in use #";
                _status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                _status.Spring = false;
                _status = null;
            }
        }

        #endregion

        #region // --- Private Animation ---

        private void InitAnimation()
        {
            if (_animation != null)
            {
                _animation.DisplayStyle = ToolStripItemDisplayStyle.Image;
                _animation.ImageScaling = ToolStripItemImageScaling.None;
                _animation.Image = Properties.Resources.status_anim;
                if (!DesignMode)
                {
                    _animation.Visible = false;
                }
            }
        }

        private void ReleaseAnimation()
        {
            if (_animation != null)
            {
                _animation.Image = null;
                _animation.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _animation.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                _animation.Text = "# Not in use #";
                if (!DesignMode)
                {
                    _animation.Visible = true;
                }
                _animation = null;
            }
        }
        #endregion


    } // public class StatusStripExtender
}
