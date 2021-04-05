using Application.DTOs.Account;
using Application.Features.AccountFeatures.Commands;
using Application.Features.CategoryFeatures.Queries.GetAllCategories;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAccountCommand, User>();
            CreateMap<RegisterRequest, User>();
            CreateMap<User, RegisterResponse>();
            CreateMap<User, AuthenticateResponse>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<GetAllPostItemQuery, GetAllItemsParameter>();
            CreateMap<Item, GetAllItemViewModel>()
                .ForMember(dest => dest.ImageUrl, o => o.MapFrom(source => source.ItemImageRelationships.ToList().FirstOrDefault().Image.FileName))
                .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.DonateAccount.FullName));
            CreateMap<PostItemCommand, Item>();
        }
    }
}
