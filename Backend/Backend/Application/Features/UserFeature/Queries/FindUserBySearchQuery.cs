using Application.DTOs.Account;
using Application.Filter;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeature.Queries
{
    public class FindUserBySearchQuery : RequestParameter, IRequest<PagedResponse<IReadOnlyCollection<UserMiniInfoDTO>>>
    {
        public string Query { get; set; }
    }
    public class FindUserBySearchQueryHandler : IRequestHandler<FindUserBySearchQuery, PagedResponse<IReadOnlyCollection<UserMiniInfoDTO>>>
    {
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IImageRepository _imageRepository;
        public FindUserBySearchQueryHandler(IUserRepositoryAsync userRepository, IImageRepository imageRepository)
        {
            _userRepository = userRepository;
            _imageRepository = imageRepository;
        }
        public async Task<PagedResponse<IReadOnlyCollection<UserMiniInfoDTO>>> Handle(FindUserBySearchQuery request, CancellationToken cancellationToken)
        {
            var list = await _userRepository.GetListUserByQuery(request.Query, request.PageNumber, request.PageSize);
            var result = new List<UserMiniInfoDTO>();
            for (int i = 0; i < list.Count; i++)
            {
                var element = new UserMiniInfoDTO();
                element.Id = list[i].Id;
                element.FullName = list[i].FullName;
                if (list[i].Avatar != null)
                {
                    element.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(list[i].Avatar.FileName);
                }
                result.Add(element);
            }
            return new PagedResponse<IReadOnlyCollection<UserMiniInfoDTO>>(result, request.PageNumber, request.PageSize);
        }
    }
}
