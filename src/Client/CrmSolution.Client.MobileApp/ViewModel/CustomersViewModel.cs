using Bit.ViewModel;
using Prism.Navigation;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms.StateSquid;
using System.Linq;
using CrmSolution.Shared.Dto;
using Simple.OData.Client;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class CustomersViewModel : BitViewModelBase, INotifyPropertyChanged
    {
        public IODataClient ODataClient { get; set; }

        int size = 5;
        List<CustomerDto> customerList = new List<CustomerDto>();

        public CustomersViewModel()
        {
            LoadMoreCommand = new BitDelegateCommand(LoadMore);
            RefreshCommand = new BitDelegateCommand(Refresh);
            AddCommand = new BitDelegateCommand(Save);
            EditCommand = new BitDelegateCommand<CustomerDto>(Save);
            DeleteCommand = new BitDelegateCommand<CustomerDto>(Delete);
        }

        public State CurrentState { get; set; }

        public int ItemsThreshold { get; set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<CustomerDto> Customers { get; set; }

        public BitDelegateCommand LoadMoreCommand { get; set; }

        public BitDelegateCommand RefreshCommand { get; set; }

        public BitDelegateCommand AddCommand { get; set; }

        public BitDelegateCommand<CustomerDto> EditCommand { get; set; }

        public BitDelegateCommand<CustomerDto> DeleteCommand { get; set; }

        public string SearchText { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            CurrentState = State.Loading;

            try
            {
                await Get();
            }
            finally
            {
                CurrentState = State.None;
            }
        }

        async Task Get()
        {
            ItemsThreshold = 0;

            customerList = (await ODataClient.Customers().Take(size).OrderByDescending(o => o.Id).FindEntriesAsync(_CancellationTokenSource.Token)).ToList();
            Customers = new ObservableCollection<CustomerDto>(customerList);
        }

        async Task LoadMore()
        {
            if (IsRefreshing)
                return;
            else
            {
                IsRefreshing = true;

                try
                {
                    int page = customerList.Count / size;
                    int skip = page * size;

                    var customers = (await ODataClient.Customers().Skip(skip).Take(size)
                        .OrderByDescending(o => o.Id).FindEntriesAsync()).ToList();

                    if (customers.Any())
                        customerList.AddRange(customers);
                    else
                        ItemsThreshold = -1;

                    Customers = new ObservableCollection<CustomerDto>(customerList);
                }
                finally
                {
                    IsRefreshing = false;
                }
            }
        }

        async Task Refresh()
        {
            await Get();
            IsRefreshing = false;
        }

        private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        public async void OnSearchTextChanged()
        {
            _CancellationTokenSource.Cancel();

            _CancellationTokenSource = new CancellationTokenSource();

            if (!string.IsNullOrEmpty(SearchText) && SearchText.Length > 2)
            {
                var customers = (await ODataClient.Customers()
                   .Where(c => c.FirstName.Contains(SearchText) || c.LastName.Contains(SearchText))
                   .FindEntriesAsync(_CancellationTokenSource.Token)).ToList();

                Customers = new ObservableCollection<CustomerDto>(customers);
            }
            else
            {
                await Get();
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
