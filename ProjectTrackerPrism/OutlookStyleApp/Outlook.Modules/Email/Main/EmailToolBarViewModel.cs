using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation;

namespace Outlook.Modules.Email
{
    public class EmailToolBarViewModel
    {
        public EmailToolBarViewModel()
        {
            this.NewEmailCommand = new ObservableObject<ICommand>();
            this.ReplyCommand = new ObservableObject<ICommand>();
            this.ReplyToAllCommand = new ObservableObject<ICommand>();
        }
        public ObservableObject<ICommand> NewEmailCommand
        {
            get; private set;
        }

        public ObservableObject<ICommand> ReplyCommand
        {
            get; private set;
        }


        public ObservableObject<ICommand> ReplyToAllCommand
        {
            get;
            private set;
        }
    }
}
