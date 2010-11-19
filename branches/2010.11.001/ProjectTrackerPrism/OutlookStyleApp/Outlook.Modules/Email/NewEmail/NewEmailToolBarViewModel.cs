using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation;

namespace Outlook.Modules.Email.NewEmail
{
    public class NewEmailToolBarViewModel
    {
        public NewEmailToolBarViewModel()
        {
            this.SendEmailCommand = new ObservableObject<ICommand>();
        }
        public ObservableObject<ICommand> SendEmailCommand
        {
            get;
            private set;
        }
    }
}
