using AutoMapper;
using Customer.ServiceInterface.Requests;
using Customers.Domain;

namespace Customer.ServiceInterface.Mappings;

public class CustomersProfile : Profile
{
    public CustomersProfile()
    {
        CreateMap<CustomerModel, Contracts.Customer>()
            .ForMember(p => p.Id, o => o.MapFrom(p => p.UniqueId));
        CreateMap<Contracts.Customer, CustomerModel>()
            .ForMember(p => p.Id, m => m.Ignore());

        CreateMap<CreateCustomerRequest, CustomerModel>()
            .ForMember(p => p.Id, o => o.Ignore())
            .ForMember(p => p.UniqueId, o => o.Ignore());
        
        CreateMap<UpdateCustomerRequest, CustomerModel>()
            .ForMember(p => p.Id, o => o.Ignore())
            .ForMember(p => p.UniqueId, o => o.Ignore());
    }
}