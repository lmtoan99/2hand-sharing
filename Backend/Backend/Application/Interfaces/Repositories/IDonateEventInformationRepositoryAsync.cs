using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IDonateEventInformationRepositoryAsync : IGenericRepositoryAsync<DonateEventInformation>
    {
        Task<DonateEventInformation> CheckPermissonForAssignItem(int donateEventId, int adminId, int memberId);
    }
}
