using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AssignmentFeatures.Commands
{
    public class CancelAssignReceiveItemToMemberCommand : IRequest<Response<int?>>
    {
        public int AssignmentId { get; set; }
        public int UserId { get; set; }
        public class CancelAssignReceiveItemToMemberCommandHandler : IRequestHandler<CancelAssignReceiveItemToMemberCommand, Response<int?>>
        {
            private readonly IAssignmentRepositoryAsync _assignmentRepository;
            public CancelAssignReceiveItemToMemberCommandHandler(IAssignmentRepositoryAsync assignmentRepository)
            {       
                _assignmentRepository = assignmentRepository;
            }
            public async Task<Response<int?>> Handle(CancelAssignReceiveItemToMemberCommand command, CancellationToken cancellationToken)
            {
                var checkAdmin = await _assignmentRepository.CheckAssignBefore(command.UserId);
                if (checkAdmin == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var assignment = await _assignmentRepository.GetByIdAsync(command.AssignmentId);
                if (assignment == null)
                {
                    throw new ApiException($"Assignment Not Found.");
                }

                await _assignmentRepository.DeleteAsync(assignment);
                return new Response<int?>(null);
            }
        }
    }
}
