using HiringManager.DataAccess.Repository.IRepository;
using HiringManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiringManager.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<PersonalDetail> PersonalDetails { get; }
        IRepository<EducationDetail> EducationDetails { get; }
        IRepository<ExperienceDetail> ExperienceDetails { get; }
        IApplicationRepository Applications { get; }
        Task<int> CompleteAsync();
    }
}
