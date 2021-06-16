using Application.DTOs.Account;
using Application.DTOs.Address;
using Application.DTOs.Group;
using Application.DTOs.Item;
using Application.DTOs.Message;
using Application.DTOs.Notification;
using Application.DTOs.ReceiveRequest;
using Application.Features.AccountFeatures.Commands;
using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Queries.GetAllCategories;
using Application.Features.GroupFeatures.Queries;
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
                    source.DonateAccount.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.DonateAccount.Avatar.FileName));
            CreateMap<PostItemCommand, Item>();
            CreateMap<GetAllItemByCategoryIdQuery, GetAllItemsParameter>();
            CreateMap<AddressDTO, Address>().ReverseMap();
            CreateMap<Item, GetItemByIdViewModel>()
                 .ForMember(dest => dest.ImageUrl, o => o.MapFrom(source => source.ItemImageRelationships.ToList().Select(e => e.Image.FileName)))
                 .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.DonateAccount.FullName))
                 .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.DonateAccount.Avatar.FileName));
            CreateMap<ReceiveItemInformation, ReceiveRequestDonorViewModel>()
                .ForMember(dest => dest.ReceiverName, o => o.MapFrom(source => source.Receiver.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Receiver.Avatar.FileName));
            CreateMap<User, UserInfoDTO>()
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Avatar.FileName));
            CreateMap<ReceiveItemInformation, GetAllItemViewModel>()
                .ForAllMembers(o => o.MapFrom(source => source.Items));
            CreateMap<Group, GroupDTO>();
            CreateMap<Group, GroupViewModel>();
            CreateMap<ReceiveItemInformation, ReceiveRequestDoneeViewModel>();
            CreateMap<Message, MessageDTO>();
            CreateMap<Message, RecentMessagesDTO>().ForMember(dest => dest.SendFromAccountName, o => o.MapFrom(source => source.SendFromAccount.FullName))
                .ForMember(dest => dest.SendToAccountName, o => o.MapFrom(source => source.SendToAccount.FullName))
                .ForMember(dest => dest.SendFromAccountAvatarUrl, o => o.MapFrom(source => source.SendFromAccount.Avatar.FileName))
                .ForMember(dest => dest.SendToAccountAvatarUrl, o => o.MapFrom(source => source.SendToAccount.Avatar.FileName ));
            CreateMap<Notification, NotificationDTO>();
            CreateMap<GetAllGroupMemberByGroupIdQuery, GetAllGroupMemberByGroupIdParameter>();
            CreateMap<GroupMemberDetail, GetAllGroupMemberViewModel>()
                .ForMember(dest => dest.FullName, o => o.MapFrom(source =>
                    source.Member.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Member.Avatar.FileName));

            CreateMap<GroupMemberDetail, GroupMemberDTO>();
            CreateMap<GetAllGroupJoinedQuery, GetAllGroupJoinedParameter>();
            CreateMap<GetAllGroupQuery, GetAllGroupParameter>();
        }
    }
}
