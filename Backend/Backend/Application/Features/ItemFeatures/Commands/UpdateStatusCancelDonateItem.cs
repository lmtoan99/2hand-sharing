using Application.Enums;
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

namespace Application.Features.ItemFeatures.Commands
{
    public class UpdateStatusCancelDonateItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class UpdateStatusCancelDonateItemCommandHandler : IRequestHandler<UpdateStatusCancelDonateItemCommand, Response<int>>
        {
            private readonly IItemRepositoryAsync _itemRepository;
            public UpdateStatusCancelDonateItemCommandHandler(IItemRepositoryAsync itemRepository)
            {
                _itemRepository = itemRepository;
            }
            public async Task<Response<int>> Handle(UpdateStatusCancelDonateItemCommand command, CancellationToken cancellationToken)
            {
                var item = await _itemRepository.GetByIdAsync(command.Id);
                if (item == null)
                {
                    throw new ApiException($"Item Not Found.");
                }
                if (item.DonateAccountId != command.UserId)
                {
                    throw new UnauthorizedAccessException();
                }
                if (item.Status==(int)ItemStatus.NOT_YET)
                {
                    item.Status = (int)ItemStatus.CANCEL;
                    await _itemRepository.UpdateAsync(item);
                    return new Response<int>(item.Id);
                }
                throw new ApiException($"You only can cancel when do not accept any receive information."); 
            }
        }
    }
}
