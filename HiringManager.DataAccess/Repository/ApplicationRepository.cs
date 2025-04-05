using HiringManager.DataAccess.Data;
using HiringManager.DataAccess.Services;
using HiringManager.Models;
using HiringManager.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HiringManager.DataAccess.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationDetails> GetApplicationBySessionIdAsync(string sessionId)
        {
            return await _context.ApplicationDetails
                .Include(a => a.PersonalDetails)
                .Include(a => a.EducationDetails)
                .Include(a => a.ExperienceDetails)
                .FirstOrDefaultAsync(a => a.SessionId == sessionId);
        }

        public async Task<ApplicationFormViewModel> GetApplicationFormViewModelAsync(string sessionId)
        {
            var application = await GetApplicationBySessionIdAsync(sessionId);

            if (application == null)
            {
                application = new ApplicationDetails
                {
                    SessionId = sessionId,
                    StageId = 1
                };
                _context.ApplicationDetails.Add(application);
                await _context.SaveChangesAsync();
            }

            return new ApplicationFormViewModel
            {
                ApplicationDetails = application,
                PersonalDetails = application.PersonalDetails ?? new PersonalDetails(),
                EducationDetails = application.EducationDetails ?? new EducationDetails(),
                ExperienceDetails = application.ExperienceDetails ?? new ExperienceDetails(),
                CurrentStage = application.StageId
            };
        }

        public async Task SavePersonalDetailsAsync(PersonalDetails personalDetails, string sessionId)
        {
            var application = await GetApplicationBySessionIdAsync(sessionId);
            if (application == null) return;

            if (application.PersonalDetails == null)
            {
                personalDetails.ApplicationDetailsId = application.Id;
                _context.PersonalDetails.Add(personalDetails);
            }
            else
            {
                var existing = application.PersonalDetails;
                existing.Name = personalDetails.Name;
                existing.PhoneNumber = personalDetails.PhoneNumber;
                existing.Email = personalDetails.Email;
                existing.Address = personalDetails.Address;
                _context.PersonalDetails.Update(existing);
            }

            application.StageId = 2; // Move to next stage
            application.UpdatedDate = DateTime.Now;
            _context.ApplicationDetails.Update(application);

            await _context.SaveChangesAsync();
        }

        public async Task SaveEducationDetailsAsync(EducationDetails educationDetails, string sessionId)
        {
            var application = await GetApplicationBySessionIdAsync(sessionId);
            if (application == null) return;

            if (application.EducationDetails == null)
            {
                educationDetails.ApplicationDetailsId = application.Id;
                _context.EducationDetails.Add(educationDetails);
            }
            else
            {
                var existing = application.EducationDetails;
                existing.HighestQualification = educationDetails.HighestQualification;
                existing.CollegeUniversity = educationDetails.CollegeUniversity;
                existing.Stream = educationDetails.Stream;
                existing.PassoutYear = educationDetails.PassoutYear;
                existing.PointerPercentage = educationDetails.PointerPercentage;
                _context.EducationDetails.Update(existing);
            }

            application.StageId = 3; // Move to next stage
            application.UpdatedDate = DateTime.Now;
            _context.ApplicationDetails.Update(application);

            await _context.SaveChangesAsync();
        }

        public async Task SaveExperienceDetailsAsync(ExperienceDetails experienceDetails, string sessionId)
        {
            var application = await GetApplicationBySessionIdAsync(sessionId);
            if (application == null) return;

            if (application.ExperienceDetails == null)
            {
                experienceDetails.ApplicationDetailsId = application.Id;
                _context.ExperienceDetails.Add(experienceDetails);
            }
            else
            {
                var existing = application.ExperienceDetails;
                existing.TotalExp = experienceDetails.TotalExp;
                existing.PreviousCompanies = experienceDetails.PreviousCompanies;
                existing.CurrentCompany = experienceDetails.CurrentCompany;
                existing.Skills = experienceDetails.Skills;
                existing.Roles = experienceDetails.Roles;
                _context.ExperienceDetails.Update(existing);
            }

            application.UpdatedDate = DateTime.Now;
            _context.ApplicationDetails.Update(application);

            await _context.SaveChangesAsync();
        }

        public async Task SubmitApplicationAsync(string sessionId, IIdGeneratorService idGenerator)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var application = await GetApplicationBySessionIdAsync(sessionId);
                if (application == null || application.IsSubmitted) return;

                application.ApplicationId = await idGenerator.GenerateIdAsync();
                application.IsSubmitted = true;
                application.StageId = 4;
                application.UpdatedDate = DateTime.Now;

                _context.ApplicationDetails.Update(application);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<ApplicationDetails>> GetAllSubmittedApplicationsAsync()
        {
            return await _context.ApplicationDetails
                .Include(a => a.PersonalDetails)
                .Where(a => a.IsSubmitted && a.ActiveStatus)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> HasExistingApplicationAsync(string email)
        {
            return await _context.PersonalDetails
                .AnyAsync(p => p.Email == email &&
                             p.ApplicationDetails.IsSubmitted &&
                             p.ApplicationDetails.ActiveStatus);
        }

        public async Task<ApplicationFormViewModel> GetSubmittedApplicationForEditAsync(string applicationId)
        {
            var application = await _context.ApplicationDetails
                .Include(a => a.PersonalDetails)
                .Include(a => a.EducationDetails)
                .Include(a => a.ExperienceDetails)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && a.IsSubmitted);

            if (application == null) return null;

            return new ApplicationFormViewModel
            {
                ApplicationDetails = application,
                PersonalDetails = application.PersonalDetails,
                EducationDetails = application.EducationDetails,
                ExperienceDetails = application.ExperienceDetails,
                CurrentStage = 1 // Start from beginning when editing
            };
        }

        public async Task<ApplicationDetails> GetApplicationForViewAsync(string applicationId)
        {
            return await _context.ApplicationDetails
                .Include(a => a.PersonalDetails)
                .Include(a => a.EducationDetails)
                .Include(a => a.ExperienceDetails)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && a.IsSubmitted);
        }

        public async Task<bool> DeactivateApplicationAsync(string applicationId)
        {
            var application = await _context.ApplicationDetails
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (application == null) return false;

            application.ActiveStatus = false;
            application.UpdatedDate = DateTime.Now;
            application.UpdatedBy = "System"; // Or get current user

            _context.ApplicationDetails.Update(application);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
