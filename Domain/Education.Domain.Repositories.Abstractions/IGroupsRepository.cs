using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Domain.Entities;

namespace Education.Domain.Abstractions
{
    public interface IGroupsRepository : IRepository<Group, Guid>
    {
        Task<Group?> GetGroupByNameAsync(string name);

    }
}
