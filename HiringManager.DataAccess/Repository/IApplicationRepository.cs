using HiringManager.DataAccess.Services;
using HiringManager.Models;
using HiringManager.Models.ViewModel;

namespace HiringManager.DataAccess.Repository
{
    public interface IApplicationRepository
    {
        Task<ApplicationDetails> GetApplicationBySessionIdAsync(string sessionId);
        Task<ApplicationFormViewModel> GetApplicationFormViewModelAsync(string sessionId);
        Task SavePersonalDetailsAsync(PersonalDetails personalDetails, string sessionId);
        Task SaveEducationDetailsAsync(EducationDetails educationDetails, string sessionId);
        Task SaveExperienceDetailsAsync(ExperienceDetails experienceDetails, string sessionId);
        Task SubmitApplicationAsync(string sessionId, IIdGeneratorService idGenerator);
        Task<List<ApplicationDetails>> GetAllSubmittedApplicationsAsync();
        Task<bool> HasExistingApplicationAsync(string email);
        Task<ApplicationFormViewModel> GetSubmittedApplicationForEditAsync(string applicationId);
        Task<ApplicationDetails> GetApplicationForViewAsync(string applicationId);
        Task<bool> DeactivateApplicationAsync(string applicationId);
    }
}
