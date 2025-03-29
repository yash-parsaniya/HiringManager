using HiringManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiringManager.DataAccess.Repository.IRepository
{
    public interface IApplicationRepository : IRepository<ApplicationDetail>
    {
        void update(ApplicationDetail entity);
    }
}
