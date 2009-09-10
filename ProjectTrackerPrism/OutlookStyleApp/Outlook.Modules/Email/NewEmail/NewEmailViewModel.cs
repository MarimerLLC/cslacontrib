using Microsoft.Practices.Composite.Presentation;
using OutlookStyle.Infrastructure;

namespace Outlook.Modules.Email
{
    public class NewEmailViewModel
    {
        public ObservableObject<EmailMessage> Email
        {
            get; private set;
        }
        public NewEmailViewModel()
        {
            this.Email = new ObservableObject<EmailMessage>();
        }
    }
}