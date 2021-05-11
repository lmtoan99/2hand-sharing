using Application.DTOs.Group;
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

namespace Application.Features.GroupFeatures.Commands
{
    public class CreateGroupCommand : IRequest<Response<GroupDTO>>
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
        public int AdminId { get; set; }
    }

    class CreateGroupCommandHandle : IRequestHandler<CreateGroupCommand, Response<GroupDTO>>
    {
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminDetailRepository;
        private readonly IMapper _mapper;
        public CreateGroupCommandHandle(IGroupRepositoryAsync groupRepository, IGroupAdminDetailRepositoryAsync groupAdminDetailRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _groupAdminDetailRepository = groupAdminDetailRepository;
            _mapper = mapper;
        }
        public async Task<Response<GroupDTO>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            Group group = new Group { 
                GroupName = request.GroupName,
                Description = request.Description,
                Rules = request.Rules,
                CreateDate = DateTime.Now
            };
            var result = await _groupRepository.AddAsync(group);
            await _groupAdminDetailRepository.AddAsync(new GroupAdminDetail { 
                GroupId = result.Id,
                AdminId = request.AdminId,
                AppointDate = DateTime.Now
            });
            return new Response<GroupDTO>(_mapper.Map<GroupDTO>(result));
        }
    }
}
