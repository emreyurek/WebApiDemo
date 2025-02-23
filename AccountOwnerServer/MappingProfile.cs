using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace AccountOwnerServer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Owner
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerForCreationDto, Owner>();
            CreateMap<OwnerForUpdateDto, Owner>();
            
            //Account
            CreateMap<Account, AccountDto>();
            CreateMap<Account, AccountForDetailsDto>();
            CreateMap<Owner, OwnerForDetailsDto>();
            CreateMap<AccountForCreationDto,Account>();
            CreateMap<AccountForUpdateDto,Account>();
        }
    }
}
