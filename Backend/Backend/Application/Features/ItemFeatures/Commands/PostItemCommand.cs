using Application.Enums;
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

namespace Application.Features.ItemFeatures.Commands
{
    public partial class PostItemCommand : IRequest<Response<int>>
    {
        public string ItemName { get; set; }
        public string ReceiveAddress { get; set; }
        public int CategoryId { get; set; }
        public int DonateAccountId { get; set; }
        public string Description { get; set; }
    }
    public class PostItemCommandHandle : IRequestHandler<PostItemCommand, Response<int>>
    {
        private readonly IItemRepositoryAsync _itemRepository;
        private readonly IMapper _mapper;
        public PostItemCommandHandle(IItemRepositoryAsync itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(PostItemCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Item>(request);
            item.DonateType = (int)eDonateType.DonatePost;
            item.Status = (int)ItemStatus.NOT_YET;
            item.PostTime = DateTime.Now;
            await _itemRepository.AddAsync(item);
            return new Response<int>(item.Id);
        }
    }
}
