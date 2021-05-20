using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IMessageRepositoryAsync : IGenericRepositoryAsync<Message>
    {
        Task<IReadOnlyCollection<Message>> GetListMessage(int user1,int user2, int pageNumber, int pageSize);
    }
}
