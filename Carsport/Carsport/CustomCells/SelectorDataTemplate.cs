using Carsport.Common.Models;
using Carsport.ViewModels;
using Xamarin.Forms;

namespace Carsport.CustomCells
{
    public class SelectorDataTemplate : DataTemplateSelector
    {
        private readonly DataTemplate textInDataTemplate;
        private readonly DataTemplate textOutDataTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messages = item as MessageItemViewModel;
            if (messages == null)
                return null;
            
            var loggerUser = MainViewModel.GetInstance().UserASP;

            if (messages.Sender.Equals(loggerUser.Id))
            {
                return textInDataTemplate;
            }
            else
            {
                return textOutDataTemplate;
            }
        }
        
        public SelectorDataTemplate()
        {
            textInDataTemplate = new DataTemplate(typeof(TextInViewCell));
            textOutDataTemplate = new DataTemplate(typeof(TextOutViewCell));
        }
    }
}
