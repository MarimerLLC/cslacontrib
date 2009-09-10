using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookStyle.Infrastructure
{
    /// <summary>
    /// Contract that allows me to share E-mail information between two modules
    /// </summary>
    public class EmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
