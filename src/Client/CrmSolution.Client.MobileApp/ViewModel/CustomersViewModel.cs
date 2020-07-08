using Bit.ViewModel;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms.StateSquid;
using System.Linq;
using System;
using CrmSolution.Shared.Dto;
using Simple.OData.Client;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class CustomersViewModel : BitViewModelBase, INotifyPropertyChanged
    {
        public IODataClient ODataClient { get; set; }

        public CustomersViewModel()
        {
            AddCommand = new BitDelegateCommand(Save);
            EditCommand = new BitDelegateCommand<CustomerDto>(Save);
            DeleteCommand = new BitDelegateCommand<CustomerDto>(Delete);
        }

        public State CurrentState { get; set; }

        public int CustomerCounts { get; set; }

        public List<CustomerDto> AllCustomers { get; set; }

        public CustomerDto[] CustomersView => string.IsNullOrEmpty(SearchText) ? AllCustomers?.ToArray() : AllCustomers?.Where(c => c.FullName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))?.ToArray();

        public BitDelegateCommand AddCommand { get; set; }

        public BitDelegateCommand<CustomerDto> EditCommand { get; set; }

        public BitDelegateCommand<CustomerDto> DeleteCommand { get; set; }

        public BitDelegateCommand SearchCommand { get; set; }

        public string SearchText { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            CurrentState = State.Loading;
            CustomerCounts = 1;

            try
            {
                AllCustomers = (await ODataClient.Customers().FindEntriesAsync()).ToList();
                CustomerCounts = AllCustomers.Count;
            }
            finally
            {
                CurrentState = State.None;
            }
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
