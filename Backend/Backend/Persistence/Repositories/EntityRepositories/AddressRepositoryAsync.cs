using Application.DTOs.Address;
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
    public class AddressRepositoryAsync : GenericRepositoryAsync<Address>, IAddressRepositoryAsync
    {
        private readonly DbSet<Address> _address;
        public AddressRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _address = dbContext.Set<Address>();
        }

        public async Task<Address> findSameAddress(AddressDTO address)
        {
            return await _address
                .Where(a => a.Street.Equals(address.Street) &&
                    a.WardId == address.WardId &&
                    a.DistrictId == address.DistrictId &&
                    a.CityId == address.CityId).FirstOrDefaultAsync();
        }
    }
}
