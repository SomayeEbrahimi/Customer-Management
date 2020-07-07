using Bit.Core.Exceptions;
using Bit.ViewModel;
using CrmSolution.Client.MobileApp.Enum;
using CrmSolution.Client.MobileApp.Model;
using CrmSolution.Client.MobileApp.Service;
using Prism.Navigation;
using System.Threading.Tasks;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class SaveCustomerViewModel : BitViewModelBase
    {
        public Action Action { get; set; } = Action.Edit;

        public ValidationService ValidationService { get; set; }

        public SaveCustomerViewModel()
        {
            SaveCommand = new BitDelegateCommand(Save);
        }

        public Customer Customer { get; set; }

        public BitDelegateCommand SaveCommand { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            Customer = parameters.GetValue<Customer>("customer");

            if (Customer == null)
            {
                Action = Action.Add;
                Customer = new Customer { };
            }
        }

        async Task Save()
        {
            await Task.Delay(700);

            if (string.IsNullOrEmpty(Customer.FirstName) || string.IsNullOrEmpty(Customer.LastName))
                throw new DomainLogicException("Please provide First Name and Last Name!");

            if (!ValidationService.IsEnglishLetters(Customer.FirstName) || !ValidationService.IsEnglishLetters(Customer.LastName) || Customer.FirstName.Length > 20 || Customer.LastName.Length > 30)
                throw new DomainLogicException("Invalid First Name or Last Name!");

            await NavigationService.GoBackAsync();
        }
    }
}
