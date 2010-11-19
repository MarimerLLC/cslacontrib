using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation;
using OutlookStyle.Infrastructure;

namespace Outlook.Modules.Email
{
    public class EmailMainViewModel
    {
        private readonly IExchangeService exchangeService;
        private ObservableCollection<EmailMessage> inbox;

        public EmailMainViewModel(IExchangeService exchangeService)
        {
            this.exchangeService = exchangeService;
            this.inbox = new ObservableCollection<EmailMessage>();
            this.inbox.AddRange(this.exchangeService.GetInbox());
            this.SelectedEmail = new ObservableObject<EmailMessage>();
        }

        public ObservableCollection<EmailMessage> Inbox
        {
            get
            {
                return this.inbox;
            }
        }


        public ObservableObject<EmailMessage> SelectedEmail
        {
            get; private set;
        }
    }
}
