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

namespace Application.Features.AccountFeatures.Commands
{
    public partial class CreateAccountCommand : IRequest<Response<int>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response<int>>
    {
        private readonly IAccountRepositoryAsync _accountRepository;
        private readonly IMapper _mapper;
        public CreateAccountCommandHandler(IAccountRepositoryAsync accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Account>(request);
            await _accountRepository.AddAsync(product);
            return new Response<int>(product.Id);
        }
    }
}
