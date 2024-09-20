using System.Linq;
using AutoMapper;
using src.Dtos.Auth;
using src.Models;

namespace src.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}
