﻿using Acr.UserDialogs;
using Bit.ViewModel;
using CrmSolution.Shared.Dto;
using Prism.Navigation;
using Simple.OData.Client;
using System.Threading.Tasks;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class DeleteCustomerViewModel : BitViewModelBase
    {
        public IODataClient ODataClient { get; set; }
        public IUserDialogs UserDialogs { get; set; }

        public DeleteCustomerViewModel()
        {
            DeleteCommand = new BitDelegateCommand(Delete);
        }

        public CustomerDto Customer { get; set; }

        public BitDelegateCommand DeleteCommand { get; set; }

        public async override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            Customer = parameters.GetValue<CustomerDto>("customer");
        }

        async Task Delete()
        {
            UserDialogs.ShowLoading();

            await ODataClient.Customers().Key(Customer.Id)
                .DeleteEntryAsync();

            UserDialogs.HideLoading();

            await NavigationService.GoBackAsync();
        }
    }
}
