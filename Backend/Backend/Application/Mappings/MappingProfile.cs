using Application.DTOs.Account;
using Application.DTOs.Address;
using Application.DTOs.Assignment;
using Application.DTOs.Award;
using Application.DTOs.Comment;
using Application.DTOs.Event;
using Application.DTOs.Group;
using Application.DTOs.Item;
using Application.DTOs.Message;
using Application.DTOs.Notification;
using Application.DTOs.ReceiveRequest;
using Application.Features.AccountFeatures.Commands;
using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Queries.GetAllCategories;
using Application.Features.Events.Commands;
using Application.Features.Events.Queries;
using Application.Features.GroupFeatures.Queries;
using Application.Features.ItemFeatures.Commands;
using Application.Features.ItemFeatures.Queries;
using Application.Features.PostGroupFeatures.Commands;
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
            CreateMap<GetAllDonateItemForEventQuery, GetAllItemsParameter>();
            CreateMap<Item, GetAllItemViewModel>()
                .ForMember(dest => dest.ImageUrl, o => o.MapFrom(source => source.ItemImageRelationships.ToList().FirstOrDefault().Image.FileName))
                .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.DonateAccount.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source =>
                    source.DonateAccount.Avatar.FileName));
            CreateMap<Item, GetMyDonatedItemsViewModel>()
                .ForMember(dest => dest.ImageUrl, o => o.MapFrom(source => source.ItemImageRelationships.ToList().FirstOrDefault().Image.FileName))
                .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.DonateAccount.FullName))
                .ForMember(dest => dest.EventName, o => o.MapFrom(source =>
                    source.DonateEventInformation.Event.EventName))
                                .ForMember(dest => dest.EventId, o => o.MapFrom(source =>
                    source.DonateEventInformation.Event.Id))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.DonateAccount.Avatar.FileName));
            CreateMap<Item, GetAllItemDonateForEventViewModel>()
                .ForMember(dest => dest.ImageUrl, o => o.MapFrom(source => source.ItemImageRelationships.ToList().FirstOrDefault().Image.FileName))
                .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.DonateAccount.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.DonateAccount.Avatar.FileName));
            CreateMap<PostItemCommand, Item>();
            CreateMap<DonateItemForEventCommand, Item>();
            CreateMap<GetAllItemByCategoryIdQuery, GetAllItemsParameter>();
            CreateMap<AddressDTO, Address>().ReverseMap();
            CreateMap<Item, GetItemByIdViewModel>()
                 .ForMember(dest => dest.ImageUrl, o => o.MapFrom(source => source.ItemImageRelationships.ToList().Select(e => e.Image.FileName)))
                 .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.DonateAccount.FullName))
                                  .ForMember(dest => dest.EventId, o => o.MapFrom(source =>
                    source.DonateEventInformation.EventId))
                 .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.DonateAccount.Avatar.FileName));
            CreateMap<ReceiveItemInformation, ReceiveRequestDonorViewModel>()
                .ForMember(dest => dest.ReceiverName, o => o.MapFrom(source => source.Receiver.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Receiver.Avatar.FileName));
            CreateMap<User, UserInfoDTO>()
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Avatar.FileName));
            CreateMap<ReceiveItemInformation, GetAllItemViewModel>()
                .ForAllMembers(o => o.MapFrom(source => source.Items));
            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Avatar.FileName));
            CreateMap<Group, GroupViewModel>()
                .ForMember(dest => dest.AvatarURL, o => o.MapFrom(source => source.Avatar.FileName));
            CreateMap<ReceiveItemInformation, ReceiveRequestDoneeViewModel>();
            CreateMap<Message, MessageDTO>();
            CreateMap<Message, RecentMessagesDTO>().ForMember(dest => dest.SendFromAccountName, o => o.MapFrom(source => source.SendFromAccount.FullName))
                .ForMember(dest => dest.SendToAccountName, o => o.MapFrom(source => source.SendToAccount.FullName))
                .ForMember(dest => dest.SendFromAccountAvatarUrl, o => o.MapFrom(source => source.SendFromAccount.Avatar.FileName))
                .ForMember(dest => dest.SendToAccountAvatarUrl, o => o.MapFrom(source => source.SendToAccount.Avatar.FileName));
            CreateMap<Notification, NotificationDTO>();
            CreateMap<GetAllGroupMemberByGroupIdQuery, GetAllGroupMemberByGroupIdParameter>();
            CreateMap<GroupMemberDetail, GetAllGroupMemberViewModel>()
                .ForMember(dest => dest.UserId, o => o.MapFrom(source => source.MemberId))
                .ForMember(dest => dest.FullName, o => o.MapFrom(source =>
                    source.Member.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Member.Avatar.FileName));

            CreateMap<GroupMemberDetail, GroupMemberDTO>().ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Member.Avatar.FileName));
            CreateMap<GroupMemberDetail, Invitation>()
            .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Group.Avatar.FileName)).ForMember(dest => dest.GroupName, o => o.MapFrom(source => source.Group.GroupName)).ForMember(dest => dest.InvitationTime, o => o.MapFrom(source => source.JoinDate));
            CreateMap<GetAllGroupJoinedQuery, GetAllGroupJoinedParameter>();
            CreateMap<GetAllGroupQuery, GetAllGroupParameter>();
            CreateMap<Award, GetAwardsViewModel>()
                 .ForMember(dest => dest.DonateAccountName, o => o.MapFrom(source =>
                    source.Account.FullName))
                 .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Account.Avatar.FileName));
            CreateMap<Event, CreateEventDTO>().ReverseMap();
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.GroupAvatar, o => o.MapFrom(source => source.Group.Avatar.FileName))
                .ForMember(dest => dest.GroupName, o => o.MapFrom(source => source.Group.GroupName))
                .ReverseMap();
            CreateMap<Event, GetEventByEventIdViewModel>();
            CreateMap<GroupAdminDetail, GetAllGroupMemberViewModel>()
                .ForMember(dest => dest.JoinDate, o => o.MapFrom(source => source.AppointDate))
                .ForMember(dest => dest.FullName, o => o.MapFrom(source => source.Admin.FullName))
                .ForMember(dest => dest.UserId, o => o.MapFrom(source => source.AdminId))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Admin.Avatar.FileName));
            CreateMap<GroupMemberDetail, MemberJoinedRequestViewModel>()
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(source => source.JoinDate))
                .ForMember(dest => dest.RequesterId, o => o.MapFrom(source => source.MemberId))
                .ForMember(dest => dest.RequesterName, o => o.MapFrom(source => source.Member.FullName))
                .ForMember(dest => dest.AvatarUrl, o => o.MapFrom(source => source.Member.Avatar.FileName));
            CreateMap<GetListJoinRequestQuery, GetListJoinGroupRequestParameter>();
            CreateMap<Assignment, AssignmentDTO>();
            CreateMap<Assignment, AssignmentViewDTO>()
                .ForMember(dest => dest.AssignByAccountName, o => o.MapFrom(source => source.AssignByAccount.FullName))
                .ForMember(dest => dest.AssignedMemberName, o => o.MapFrom(source => source.AssignedMember.FullName));
            CreateMap<CreatePostInGroupCommand, GroupPost>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<Comment, ListCommentDTO>()
                .ForMember(c => c.AvatarUrl, o => o.MapFrom(source => source.PostByAccount.Avatar.FileName));
        }
    }
}