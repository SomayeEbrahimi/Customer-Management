using Bit.Core.Models;
using CrmSolution.Client.MobileApp.Enum;
using System.ComponentModel;

namespace CrmSolution.Client.MobileApp.Model
{
    public class Customer : Bindable
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
