using HiringManager.Models;

namespace HiringManager.DataAccess.Repository.IRepository
{
    public interface IApplicationRepository : IRepository<ApplicationDetail>
    {
        Task<ApplicationDetail?> GetDraftApplicationAsync(string sessionId);
        Task<ApplicationDetail> CreateDraftApplicationAsync(string sessionId);
        Task AddPersonalDetailsAsync(ApplicationDetail application, PersonalDetail personalDetail);
        Task SubmitApplicationAsync(ApplicationDetail application);
        Task<bool> HasExistingApplicationAsync(string email);
    }
}
