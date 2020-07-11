using Bit.Core.Exceptions;
using Bit.ViewModel;
using CrmSolution.Client.MobileApp.Enum;
using CrmSolution.Client.MobileApp.Service;
using CrmSolution.Shared.Dto;
using Prism.Navigation;
using Simple.OData.Client;
using System.Threading.Tasks;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class SaveCustomerViewModel : BitViewModelBase
    {
        public IODataClient ODataClient { get; set; }

        public Action Action { get; set; } = Action.Edit;

        public ValidationService ValidationService { get; set; }

        public SaveCustomerViewModel()
        {
            SaveCommand = new BitDelegateCommand(Save);
        }

        public CustomerDto Customer { get; set; }

        public BitDelegateCommand SaveCommand { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            Customer = parameters.GetValue<CustomerDto>("customer");

            if (Customer == null)
            {
                Action = Action.Add;
                Customer = new CustomerDto { };
            }
        }

        async Task Save()
        {
            if (string.IsNullOrEmpty(Customer.FirstName) || string.IsNullOrEmpty(Customer.LastName))
                throw new DomainLogicException("Please provide First Name and Last Name!");

            if (!ValidationService.IsEnglishLetters(Customer.FirstName) || !ValidationService.IsEnglishLetters(Customer.LastName) || Customer.FirstName.Length > 20 || Customer.LastName.Length > 30)
                throw new DomainLogicException("Invalid First Name or Last Name!");

            if (Action == Action.Add)
                await Add();
            else 
                await Update();

            await NavigationService.GoBackAsync();
        }

        async Task Add()
        {
            await ODataClient.Customers()
                .Set(Customer)
                .InsertEntryAsync();
        }

        async Task Update()
        {
            await ODataClient.Customers().Key(Customer.Id)
                .Set(Customer)
                .UpdateEntryAsync();
        }
    }
}
