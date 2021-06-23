using Application.Exceptions;
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
    public class RegisterFirebaseCommand : IRequest<Response<int>>
    {
        public int UserId { get; set; }
        public string FirebaseToken { get; set; }
    }
    public class RegisterFirebaseCommandHandler : IRequestHandler<RegisterFirebaseCommand, Response<int>>
    {
        private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;
        public RegisterFirebaseCommandHandler(IFirebaseTokenRepositoryAsync firebaseTokenRepository)
        {
            _firebaseTokenRepository = firebaseTokenRepository;
        }
        public async Task<Response<int>> Handle(RegisterFirebaseCommand request, CancellationToken cancellationToken)
        {
            if (request.FirebaseToken == null) throw new ApiException("Token must not be null");
            var tokens = await _firebaseTokenRepository.GetByConditionAsync(t => t.Token == request.FirebaseToken);
            if (tokens.Count > 0) throw new ApiException("Token already exists");
            await _firebaseTokenRepository.AddAsync(new Domain.Entities.FirebaseToken
            {
                Token = request.FirebaseToken,
                UserId = request.UserId
            });

            return new Response<int>("Register Successfully");
        }
    }
}
