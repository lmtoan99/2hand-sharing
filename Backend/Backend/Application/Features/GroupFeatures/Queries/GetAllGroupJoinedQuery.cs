﻿using Application.DTOs.Group;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Queries
{
    public class GetAllGroupJoinedQuery : IRequest<Response<IEnumerable<GroupViewModel>>>
    {
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllGroupJoinedQueryHandler : IRequestHandler<GetAllGroupJoinedQuery, Response<IEnumerable<GroupViewModel>>>
    {
        private readonly IGroupMemberDetailRepositoryAsync _groupMemberDetailRepositoryAsync;
        private readonly IImageRepository _imageRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupJoinedQueryHandler(IGroupMemberDetailRepositoryAsync groupMemberDetailRepositoryAsync, IMapper mapper, IImageRepository imageRepositoryAsync)
        {
            _groupMemberDetailRepositoryAsync = groupMemberDetailRepositoryAsync;
            _mapper = mapper;
            _imageRepositoryAsync = imageRepositoryAsync;
        }
        public async Task<Response<IEnumerable<GroupViewModel>>> Handle(GetAllGroupJoinedQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupJoinedParameter>(request);
            var res = await _groupMemberDetailRepositoryAsync.GetAllGroupJoinedByUserIdAsync(validFilter.PageNumber, validFilter.PageSize, request.UserId);
            List<GroupViewModel> list = _mapper.Map<List<GroupViewModel>>(res);
            list.ForEach(i =>
            {
                i.AvatarURL = _imageRepositoryAsync.GenerateV4SignedReadUrl(i.AvatarURL);
            });
            return new Response<IEnumerable<GroupViewModel>>(list);
        }
    }
}
