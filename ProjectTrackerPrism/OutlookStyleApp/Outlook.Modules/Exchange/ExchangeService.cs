using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookStyle.Infrastructure;

namespace Outlook.Modules.Exchange
{
    public class ExchangeService: IExchangeService
    {
        public IEnumerable<EmailMessage> GetInbox()
        {
            yield return
                new EmailMessage()
                {
                    Body = "Here are your presents",
                    From = "Santa",
                    To = "Good Kid",
                    Subject = "Presents delivery notice"
                };

            yield return
                new EmailMessage()
                {
                    Body = "NO presents for you",
                    From = "Santa",
                    To = "Bad Kid",
                    Subject = "Reprimand"
                };

        }

        public void SendEmail(EmailMessage message)
        {
            // Imagine it sending...
        }

    }
}
