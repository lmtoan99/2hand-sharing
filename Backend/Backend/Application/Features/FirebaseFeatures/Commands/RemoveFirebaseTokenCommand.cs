using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.FirebaseFeatures.Commands
{
    public class RemoveFirebaseTokenCommand : IRequest<Response<int>>
    {
        public int UserId { get; set; }
        public string FirebaseToken { get; set; }
    }
    public class RemoveFirebaseTokenCommandHandler : IRequestHandler<RemoveFirebaseTokenCommand, Response<int>>
    {
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        public RemoveFirebaseTokenCommandHandler(IFirebaseTokenRepositoryAsync firebaseTokenRepository)
        {
            _firebaseTokenRepository = firebaseTokenRepository;
        }
        public async Task<Response<int>> Handle(RemoveFirebaseTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _firebaseTokenRepository.GetByConditionAsync(f => f.Token == request.FirebaseToken && f.UserId == request.UserId);
            await _firebaseTokenRepository.DeleteAsync(result[0]);

            return new Response<int>("Remove token successfully");
        }
    }
}
