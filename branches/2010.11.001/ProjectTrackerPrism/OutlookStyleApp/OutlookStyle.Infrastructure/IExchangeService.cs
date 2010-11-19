using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookStyle.Infrastructure
{
    /// <summary>
    /// Interface for the shared Exchange service. 
    /// </summary>
    public interface IExchangeService
    {
        /// <summary>
        /// GEt all the email in the inbox
        /// </summary>
        /// <returns></returns>
        IEnumerable<EmailMessage> GetInbox();

        /// <summary>
        /// Send an email message. 
        /// </summary>
        /// <param name="message"></param>
        void SendEmail(EmailMessage message);
    }
}
