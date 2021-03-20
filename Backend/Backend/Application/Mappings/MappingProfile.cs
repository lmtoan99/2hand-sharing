using Application.DTOs.Account;
using Application.Features.AccountFeatures.Commands;
using Application.Features.CategoryFeatures.Queries.GetAllCategories;
using Application.Features.ItemFeatures.Queries;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAccountCommand, Account>();
            CreateMap<RegisterRequest, Account>();
            CreateMap<Account, RegisterResponse>();
            CreateMap<Account, AuthenticateResponse>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<GetAllPostItemQuery, GetAllItemsParameter>();
            CreateMap<Item, GetAllItemViewModel>();
        }
    }
}
