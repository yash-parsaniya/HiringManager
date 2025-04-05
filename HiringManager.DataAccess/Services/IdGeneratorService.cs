using HiringManager.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace HiringManager.DataAccess.Services
{
    public class IdGeneratorService : IIdGeneratorService
    {
        private readonly ApplicationDbContext _context;

        public IdGeneratorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateIdAsync()
        {
            var today = DateTime.Today.ToString("yyyyMMdd");
            var lastApplication = await _context.ApplicationDetails
                .Where(a => a.ApplicationId != null && a.ApplicationId.StartsWith(today))
                .OrderByDescending(a => a.ApplicationId)
                .FirstOrDefaultAsync();

            var sequenceNumber = 1;
            if (lastApplication != null && !string.IsNullOrEmpty(lastApplication.ApplicationId))
            {
                var parts = lastApplication.ApplicationId.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }

            return $"{today}-{sequenceNumber:D4}";
        }
    }
}
