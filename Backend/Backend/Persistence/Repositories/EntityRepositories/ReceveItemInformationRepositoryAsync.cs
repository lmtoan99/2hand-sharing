﻿using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.EntityRepositories
{
    class ReceveItemInformationRepositoryAsync : GenericRepositoryAsync<ReceiveItemInformation>, IReceiveItemInformationRepositoryAsync
    {
        private readonly DbSet<ReceiveItemInformation> _receiveItemInformation;
        public ReceveItemInformationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _receiveItemInformation = dbContext.Set<ReceiveItemInformation>();
        }

        public async Task<IReadOnlyList<ReceiveItemInformation>> GetAllByItemId(int itemId)
        {
            return await _receiveItemInformation.Where(e => e.ItemId == itemId).ToListAsync();
        }



        public async Task<ReceiveItemInformation> GetReceiveRequestByItemIdAndUserId(int itemId, int userId)
        {
            return await _receiveItemInformation.Where(req => req.ItemId == itemId && req.ReceiverId == userId).FirstOrDefaultAsync();
        }

        public async Task<ReceiveItemInformation> GetReceiveRequestByWithReceiverInfoById(int requestId)
        {
            return await _receiveItemInformation.Where(req => req.Id == requestId).Include(req => req.Receiver).FirstOrDefaultAsync();
        }

        public async Task<ReceiveItemInformation> GetReceiveRequestWithItemInfoById(int requestId)
        {
            return await _receiveItemInformation
                .Where(req => req.Id == requestId)
                .Include(req => req.Items)
                .FirstOrDefaultAsync();
        }
    }
}
