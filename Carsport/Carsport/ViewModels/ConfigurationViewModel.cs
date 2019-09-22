using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Carsport.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        #region Attributes
        private string aboutMessage;
        private string privacity;
        private string termsOfUse;
        #endregion

        #region Propierties
        public string AboutMessage
        {
            get { return this.aboutMessage; }
            set { this.SetValue(ref this.aboutMessage, value); }
        }

        public string Privacity
        {
            get { return this.privacity; }
            set { this.SetValue(ref this.privacity, value); }
        }

        public string TermsOfUse
        {
            get { return this.termsOfUse; }
            set { this.SetValue(ref this.termsOfUse, value); }
        }
        #endregion

        #region Constructors
        public ConfigurationViewModel()
        {
            this.AboutMessage = Application.Current.Resources["About"].ToString();
            this.Privacity = Application.Current.Resources["Privacity"].ToString();
            this.TermsOfUse = Application.Current.Resources["TermsOfUse"].ToString();
        }
        #endregion
    }
}