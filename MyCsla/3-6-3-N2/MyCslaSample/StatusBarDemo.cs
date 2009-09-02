using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyCslaSample
{
  public partial class StatusBarExtenderDemo : Form
  {
    public StatusBarExtenderDemo()
    {
      InitializeComponent();
    }

    #region BackgroundWorker
    private void button1_Click(object sender, EventArgs e)
    {
      // set status to waiting and message
      statusStripExtender1.SetStatusWaiting("Loading data. Please wait.");

      // start backgroundworker 
      backgroundWorker1.RunWorkerAsync();
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      Thread.Sleep(5000);
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (Disposing) return;
      
      statusStripExtender1.SetStatus("Data loaded. xx rows fetched");
    }

    #endregion

    #region SetStatus   

    private void button2_Click(object sender, EventArgs e)
    {
      // Set the status - will be shown for 5 seconds and then reset to default message
      statusStripExtender1.SetStatus("Data updated.");
    }

    #endregion 

    #region SetStatusStatic
    private void button3_Click(object sender, EventArgs e)
    {
      statusStripExtender1.SetStatusStatic("Message is not reset.");

    }
    #endregion 
  }
}
