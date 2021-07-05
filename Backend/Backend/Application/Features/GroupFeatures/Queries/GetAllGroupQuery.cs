using Application.DTOs.Group;
using Application.Filter;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Queries
{
    public class GetAllGroupQuery : RequestParameter, IRequest<Response<IEnumerable<GroupViewModel>>>
    {
        public string Query { get; set; }
    }
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, Response<IEnumerable<GroupViewModel>>>
    {
        private readonly IGroupRepositoryAsync _groupRepositoryAsync;
        private readonly IImageRepository _imageRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupQueryHandler(IGroupRepositoryAsync groupRepositoryAsync, IMapper mapper, IImageRepository imageRepositoryAsync)
        {
            _groupRepositoryAsync = groupRepositoryAsync;
            _mapper = mapper;
            _imageRepositoryAsync = imageRepositoryAsync;
        }
        public async Task<Response<IEnumerable<GroupViewModel>>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Group> res; 
            if (request.Query != null)
            {
                res = await _groupRepositoryAsync.SearchGroupAsync(request.Query, request.PageNumber, request.PageSize);
            }
            else
            {
                res = await _groupRepositoryAsync.GetAllGroupAsync(request.PageNumber, request.PageSize);
            }
            List<GroupViewModel> list = _mapper.Map<List<GroupViewModel>>(res);
            list.ForEach(i =>
            {
                i.AvatarURL = _imageRepositoryAsync.GenerateV4SignedReadUrl(i.AvatarURL);
            });
            return new Response<IEnumerable<GroupViewModel>>(list);
        }
    }
}

