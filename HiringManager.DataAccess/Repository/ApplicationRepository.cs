using HiringManager.DataAccess.Data;
using HiringManager.DataAccess.Repository.IRepository;
using HiringManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HiringManager.DataAccess.Repository
{
    public class ApplicationRepository : Repository<ApplicationDetail>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ApplicationDetail?> GetDraftApplicationAsync(string sessionId)
        {
            return await _context.ApplicationDetails
                .Include(a => a.PersonalDetail)
                .Include(a => a.EducationDetails)
                .Include(a => a.ExperienceDetails)
                .FirstOrDefaultAsync(a => a.SessionId == sessionId && !a.Submitted);
        }

        public async Task<ApplicationDetail> CreateDraftApplicationAsync(string sessionId)
        {
            var application = new ApplicationDetail
            {
                SessionId = sessionId,
                StageId = 1,
                CreatedDate = DateTime.UtcNow,
                ActiveStatus = true
            };

            await AddAsync(application);
            return application;
        }

        public async Task AddPersonalDetailsAsync(ApplicationDetail application, PersonalDetail personalDetail)
        {
            if (application.PersonalDetail != null)
            {
                _context.Entry(application.PersonalDetail).CurrentValues.SetValues(personalDetail);
            }
            else
            {
                personalDetail.ApplicationDetailId = application.Id;
                _context.PersonalDetails.Add(personalDetail);
            }
        }

        public async Task SubmitApplicationAsync(ApplicationDetail application)
        {
            application.Submitted = true;
            application.StageId = 2;
            application.UpdatedDate = DateTime.UtcNow;
            Update(application);
        }

        public async Task<bool> HasExistingApplicationAsync(string email)
        {
            return await _context.PersonalDetails
                .AnyAsync(p => p.Email == email &&
                              p.ApplicationDetail != null &&
                              p.ApplicationDetail.Submitted);
        }
    }
}
