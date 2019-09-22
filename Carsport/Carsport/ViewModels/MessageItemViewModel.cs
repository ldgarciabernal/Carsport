using System;

namespace Carsport.ViewModels
{
    public class MessageItemViewModel : BaseViewModel
    {
        public string Message
        {
            get { return this.message; }
            set { this.SetValue(ref this.message, value); }
        }

        private string message;

        public DateTime Date
        {
            get { return this.date; }
            set { this.SetValue(ref this.date, value); }
        }

        private DateTime date;

        public string Sender
        {
            get { return this.sender; }
            set { this.SetValue(ref this.sender, value); }
        }

        private string sender;

        private string conectionId;

        public string ConectionId
        {
            get { return this.conectionId; }
            set { this.SetValue(ref this.conectionId, value); }
        }
    }
}
