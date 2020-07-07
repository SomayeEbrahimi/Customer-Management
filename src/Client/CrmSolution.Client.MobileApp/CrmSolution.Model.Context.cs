
namespace Simple.OData.Client
{
	[System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "1.0.0.0")]
    public static class CrmSolutionV1ContextExt
    {
		
			public static IBoundClient<CrmSolution.Shared.Dto.CustomerDto> Customers(this IODataClient odataClient)
			{
				return odataClient.For<CrmSolution.Shared.Dto.CustomerDto>("Customers");
			}

			

		    }
}
