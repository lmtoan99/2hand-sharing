﻿using Application.DTOs.Assignment;
using Application.Enums;
using Application.Exceptions;
using Application.Features.GroupFeatures.Commands;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AssignmentFeatures.Commands
{
    public class AssignItemToMemberCommand : AssignMemberDTO,IRequest<Response<AssignmentDTO>>
    {
        public int AssignByAccountId { get; set; }
    }
    public class AssignItemToMemberCommandHandler : IRequestHandler<AssignItemToMemberCommand, Response<AssignmentDTO>>
    {
        private readonly IDonateEventInformationRepositoryAsync _donateEventInformation;
        private readonly IAssignmentRepositoryAsync _assignmentRepository;
        private readonly IMapper _mapper;
        public AssignItemToMemberCommandHandler(IDonateEventInformationRepositoryAsync donateEventInformation, IAssignmentRepositoryAsync assignmentRepository, IMapper mapper)
        {
            _donateEventInformation = donateEventInformation;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }
        public async Task<Response<AssignmentDTO>> Handle(AssignItemToMemberCommand request, CancellationToken cancellationToken)
        {
            var checkExist = await _assignmentRepository.GetByConditionAsync(a => a.DonateEventInformationId == request.DonateEventInformationId);
            if (checkExist.Count > 0)
            {
                throw new ApiException("Assigned item");
            }
            var donateEvent = await _donateEventInformation.CheckPermissonForAssignItem(request.DonateEventInformationId, request.AssignByAccountId, request.AssignedMemberId);
            if (donateEvent == null)
            {
                throw new ApiException("Invalid request");
            }

            var result = await _assignmentRepository.AddAsync(new Domain.Entities.Assignment
            {
                AssignByAccountId = request.AssignByAccountId,
                AssignedMemberId = request.AssignedMemberId,
                AssignmentDate = DateTime.UtcNow,
                DonateEventInformationId = request.DonateEventInformationId,
                ExpirationDate = request.ExpirationDate,
                Note = request.Note,
                Status = (int)ReceiveItemInformationStatus.RECEIVING
            });
            return new Response<AssignmentDTO>(_mapper.Map<AssignmentDTO>(result));
        }
    }
}