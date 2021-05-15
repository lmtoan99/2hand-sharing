using Application.DTOs.ReceiveRequest;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ReceiveItemInformationFeatures.Queries
{
    public class DoneeGetReceiveRequestByIdQuery : IRequest<Response<ReceiveRequestDoneeViewModel>>
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }
    }
    public class DoneeGetReceiveRequestByIdQueryHandler : IRequestHandler<DoneeGetReceiveRequestByIdQuery, Response<ReceiveRequestDoneeViewModel>>
    {
        private IReceiveItemInformationRepositoryAsync _receiveItemInformationRepository;
        private IMapper _mapper;
        public DoneeGetReceiveRequestByIdQueryHandler(IReceiveItemInformationRepositoryAsync receiveItemInformationRepository, IMapper mapper)
        {
            _receiveItemInformationRepository = receiveItemInformationRepository;
            _mapper = mapper;
        }

        public async Task<Response<ReceiveRequestDoneeViewModel>> Handle(DoneeGetReceiveRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var receiveReq = await _receiveItemInformationRepository.GetByIdAsync(request.RequestId);
            if (receiveReq == null) throw new KeyNotFoundException("RequestId not found");
            if (receiveReq.ReceiverId != request.UserId) throw new UnauthorizedAccessException("You are not authoried to view this request");
            return new Response<ReceiveRequestDoneeViewModel>(_mapper.Map<ReceiveRequestDoneeViewModel>(receiveReq));
        }
    }
}
