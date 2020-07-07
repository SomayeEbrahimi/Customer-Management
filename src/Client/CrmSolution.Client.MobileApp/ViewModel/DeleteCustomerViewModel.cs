using Bit.ViewModel;
using CrmSolution.Client.MobileApp.Model;
using Prism.Navigation;
using System.Threading.Tasks;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class DeleteCustomerViewModel : BitViewModelBase
    {
        public DeleteCustomerViewModel()
        {
            DeleteCommand = new BitDelegateCommand(Delete);
        }

        public Customer Customer { get; set; }

        public BitDelegateCommand DeleteCommand { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            Customer = parameters.GetValue<Customer>("customer");
        }

        async Task Delete()
        {
            await Task.Delay(700);

            await NavigationService.GoBackAsync();
        }
    }
}
