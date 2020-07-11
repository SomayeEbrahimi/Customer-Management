using Bit.OData.ODataControllers;
using CrmSolution.Server.Model;
using CrmSolution.Shared.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrmSolution.Server.Api.Controllers
{
    public class CustomersController : DtoSetController<CustomerDto, Customer, int>
    {
        public async override Task<SingleResult<CustomerDto>> Create(CustomerDto dto, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            return await base.Create(dto, cancellationToken);
        }

        public async override Task<SingleResult<CustomerDto>> Update(int key, CustomerDto dto, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            return await base.Update(key, dto, cancellationToken);
        }

        public async override Task Delete(int key, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            await base.Delete(key, cancellationToken);
        }

        public async override Task<IQueryable<CustomerDto>> GetAll(CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            return await base.GetAll(cancellationToken);
        }
    }
}
