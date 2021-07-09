using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICommentRepositoryAsync : IGenericRepositoryAsync<Comment>
    {
        public Task<IReadOnlyCollection<Comment>> GetListCommentByPostId(int postId, int pageNumber, int pageSize);
    }
}
