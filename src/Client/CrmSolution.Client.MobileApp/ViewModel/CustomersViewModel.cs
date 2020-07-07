using Bit.ViewModel;
using CrmSolution.Client.MobileApp.Enum;
using CrmSolution.Client.MobileApp.Model;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms.StateSquid;
using System.Linq;
using System;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class CustomersViewModel : BitViewModelBase, INotifyPropertyChanged
    {
        public CustomersViewModel()
        {
            AddCommand = new BitDelegateCommand(Save);
            EditCommand = new BitDelegateCommand<Customer>(Save);
            DeleteCommand = new BitDelegateCommand<Customer>(Delete);
        }

        public State CurrentState { get; set; }

        public List<Customer> AllCustomers { get; set; }

        public Customer[] CustomersView => string.IsNullOrEmpty(SearchText) ? AllCustomers?.ToArray() : AllCustomers?.Where(c => c.FullName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))?.ToArray();

        public BitDelegateCommand AddCommand { get; set; }

        public BitDelegateCommand<Customer> EditCommand { get; set; }

        public BitDelegateCommand<Customer> DeleteCommand { get; set; }

        public BitDelegateCommand SearchCommand { get; set; }

        public string SearchText { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                CurrentState = State.Loading;

                await Task.Delay(7000);

                try
                {
                    AllCustomers = new List<Customer>
                    {
                        new Customer { Id = 1, FirstName = "Sasan", LastName = "Ebrahimi" },
                        new Customer { Id = 2, FirstName = "Ali", LastName = "Rahimi" },
                        new Customer { Id = 3, FirstName = "Mohammad", LastName = "Rostami" }
                    };
                }
                finally
                {
                    CurrentState = State.None;
                }
            }
        }

        async Task Save()
        {
            await Save(null);
        }

        async Task Save(Customer customer)
        {
            await NavigationService.NavigateAsync("SaveCustomer", new NavigationParameters
            {
               { "customer", customer }
            });
        }

        async Task Delete(Customer customer)
        {
            await NavigationService.NavigateAsync("DeleteCustomer", new NavigationParameters
            {
               { "customer", customer }
            });
        }
    }
}
