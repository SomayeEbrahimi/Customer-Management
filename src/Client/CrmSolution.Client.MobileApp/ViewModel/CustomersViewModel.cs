using Bit.ViewModel;
using Prism.Navigation;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms.StateSquid;
using System.Linq;
using CrmSolution.Shared.Dto;
using Simple.OData.Client;
using System.Collections.ObjectModel;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class CustomersViewModel : BitViewModelBase, INotifyPropertyChanged
    {
        public IODataClient ODataClient { get; set; }

        int index = 0; int size = 5;

        public CustomersViewModel()
        {
            LoadMoreCommand = new BitDelegateCommand(Get);
            SearchCommand = new BitDelegateCommand(Search);
            AddCommand = new BitDelegateCommand(Save);
            EditCommand = new BitDelegateCommand<CustomerDto>(Save);
            DeleteCommand = new BitDelegateCommand<CustomerDto>(Delete);
        }

        public State CurrentState { get; set; }

        public int ItemsThreshold { get; set; }

        public ObservableCollection<CustomerDto> Customers { get; set; }

        public BitDelegateCommand LoadMoreCommand { get; set; }

        public BitDelegateCommand AddCommand { get; set; }

        public BitDelegateCommand<CustomerDto> EditCommand { get; set; }

        public BitDelegateCommand<CustomerDto> DeleteCommand { get; set; }

        public BitDelegateCommand SearchCommand { get; set; }

        public string SearchText { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            CurrentState = State.Loading;

            try
            {
                ItemsThreshold = 0;
                await Get();
            }
            finally
            {
                CurrentState = State.None;
            }
        }

        async Task Get()
        {
            var customers = (await ODataClient.Customers().Skip(index * size).Take(size)
                .OrderByDescending(o => o.Id).FindEntriesAsync()).ToList();

            if (customers.Count == 0)
                ItemsThreshold = -1;
            else ItemsThreshold = 0;

            Customers = new ObservableCollection<CustomerDto>(customers);

            index++;
        }

        async Task Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var customers = (await ODataClient.Customers()
               .Where(c => c.FirstName.Contains(SearchText) || c.LastName.Contains(SearchText))
               .FindEntriesAsync()).ToList();

                Customers = new ObservableCollection<CustomerDto>(customers);
            }
            else await Get();
        }

        async Task Save()
        {
            await Save(null);
        }

        async Task Save(CustomerDto customer)
        {
            await NavigationService.NavigateAsync("SaveCustomer", new NavigationParameters
            {
               { "customer", customer}
            });
        }

        async Task Delete(CustomerDto customer)
        {
            await NavigationService.NavigateAsync("DeleteCustomer", new NavigationParameters
            {
               { "customer", customer}
            });
        }
    }
}
