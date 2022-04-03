using AutoMapper;
using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountViewModel>();
            CreateMap<TransactionUpdateViewModel, Transaction>().ReverseMap();
        }
    }
}
