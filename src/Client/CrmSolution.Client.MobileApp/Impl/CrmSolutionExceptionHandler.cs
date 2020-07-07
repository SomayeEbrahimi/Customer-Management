using Acr.UserDialogs;
using Bit.Core.Exceptions;
using Bit.ViewModel;
using System;
using System.Collections.Generic;

namespace CrmSolution.Client.MobileApp.Impl
{
    public class CrmSolutionExceptionHandler : BitExceptionHandler
    {
        public IUserDialogs UserDialogs { get; set; }

        public override void OnExceptionReceived(Exception exp, IDictionary<string, string> properties = null)
        {
            if (exp is DomainLogicException)
            {
                UserDialogs.Alert(message: exp.Message, title: "Error");
            }
            else
            {
                UserDialogs.Alert("/-:", title: "Error");
            }

#if DEBUG

            System.Diagnostics.Debugger.Break();

#endif

            base.OnExceptionReceived(exp, properties);
        }
    }
}
