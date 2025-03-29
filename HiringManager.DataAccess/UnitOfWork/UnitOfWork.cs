using HiringManager.DataAccess.Data;
using HiringManager.DataAccess.Repository.IRepository;
using HiringManager.DataAccess.Repository;
using HiringManager.Models;

namespace HiringManager.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<PersonalDetail>? _personalDetails;
        private IRepository<EducationDetail>? _educationDetails;
        private IRepository<ExperienceDetail>? _experienceDetails;
        private IApplicationRepository? _applications;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<PersonalDetail> PersonalDetails =>
            _personalDetails ??= new Repository<PersonalDetail>(_context);

        public IRepository<EducationDetail> EducationDetails =>
            _educationDetails ??= new Repository<EducationDetail>(_context);

        public IRepository<ExperienceDetail> ExperienceDetails =>
            _experienceDetails ??= new Repository<ExperienceDetail>(_context);

        public IApplicationRepository Applications =>
            _applications ??= new ApplicationRepository(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
