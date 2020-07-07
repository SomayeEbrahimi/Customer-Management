using Bit.OData.ODataControllers;
using CrmSolution.Server.Model;
using CrmSolution.Shared.Dto;

namespace CrmSolution.Server.Api.Controllers
{
    public class CustomersController : DtoSetController<CustomerDto, Customer, int>
    {
    }
}
