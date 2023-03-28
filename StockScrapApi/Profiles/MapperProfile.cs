using AutoMapper;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CompanyTypeDto, Company>();
            CreateMap<CompanyTypeDto, Security>();
            CreateMap<ShareHoldingPerct, ShareHoldingPerctReadDto>();
            CreateMap<Company, CompanyReadDto>();
            CreateMap<PersonRawDTO, Person>();
        }
    }
}
