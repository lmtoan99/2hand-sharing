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
    public class UpdateStatusConfirmSendItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public class UpdateStatusConfirmSendItemCommandHandler : IRequestHandler<UpdateStatusConfirmSendItemCommand, Response<int>>
        {
            private readonly IItemRepositoryAsync _itemRepository;
            public UpdateStatusConfirmSendItemCommandHandler(IItemRepositoryAsync itemRepository)
            {
                _itemRepository = itemRepository;
            }
            public async Task<Response<int>> Handle(UpdateStatusConfirmSendItemCommand command, CancellationToken cancellationToken)
            {
                var item = await _itemRepository.GetByIdAsync(command.Id);
                if (item == null) throw new ApiException($"Item Not Found.");
                if (item.DonateAccountId != command.UserId) throw new UnauthorizedAccessException();

                if (item.Status == (int)ItemStatus.PENDING_FOR_RECEIVER)
                {
                    item.Status = (int)ItemStatus.SUCCESS;
                    await _itemRepository.UpdateAsync(item);
                    return new Response<int>(item.Id);
                }
                throw new ApiException($"You can not confirm send.");
            }
        }
    }
}
