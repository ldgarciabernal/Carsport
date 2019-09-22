using Carsport.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carsport.Models
{
    public class ChatMessage : BaseViewModel
    {
        private string user;
        public string User
        {
            get { return this.user; }
            set { this.SetValue(ref this.user, value); }
        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.SetValue(ref this.message, value); }
        }
    }
}
