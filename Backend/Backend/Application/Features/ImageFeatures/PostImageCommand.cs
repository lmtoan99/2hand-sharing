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

namespace Application.Features.ImageFeatures
{
    public class PostImageCommand : IRequest<Response<int>>
    {

    }
    public class PostItemCommandHandle : IRequestHandler<PostImageCommand, Response<int>>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        public PostItemCommandHandle(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(PostImageCommand request, CancellationToken cancellationToken)
        {
            var image = _mapper.Map<Image>(request);
            await _imageRepository.AddAsync(image);
            return new Response<int>(image.Id);
        }
    }
}
