using Application.DTOs.Group;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupFeatures.Commands
{
    public class UpdateGroupCommand : IRequest<Response<GroupDTO>>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
    }
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Response<GroupDTO>>
    {
        private readonly IGroupRepositoryAsync _groupRepository;
        private readonly IGroupAdminDetailRepositoryAsync _groupAdminRepository;
        private readonly IMapper _mapper;
        public UpdateGroupCommandHandler(IGroupRepositoryAsync groupRepository, IMapper mapper, IGroupAdminDetailRepositoryAsync groupAdminRepository)
        {
            _groupRepository = groupRepository;
            _groupAdminRepository = groupAdminRepository;
            _mapper = mapper;
        }
        public async Task<Response<GroupDTO>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            if (group == null) throw new KeyNotFoundException();
            var checkAdmin =  await _groupAdminRepository.GetByConditionAsync(e => e.AdminId == request.UserId && e.GroupId == request.GroupId);
            if (checkAdmin.Count == 0)
            {
                throw new ApiException("You are not admin of this group.");
            }
            if (request.GroupName != null)
            {
                group.GroupName = request.GroupName;
            }
            if (request.Rules != null)
            {
                group.Rules = request.Rules;
            }
            if (request.Description != null)
            {
                group.Description = request.Description;
            }

            await _groupRepository.UpdateAsync(group);
            return new Response<GroupDTO>(_mapper.Map<GroupDTO>(group));
        }
    }
}
