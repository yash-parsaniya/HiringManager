using HiringManager.DataAccess.Servides;
using HiringManager.DataAccess.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiringManager.DataAccess.Services
{
    public class ApplicationNumberGenerator : IApplicationNumberGenerator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApplicationNumberGenerator> _logger;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ApplicationNumberGenerator(
            IUnitOfWork unitOfWork,
            ILogger<ApplicationNumberGenerator> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string> GenerateAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var today = DateTime.Today.ToString("yyyyMMdd");
                var lastApp = await GetLastApplicationNumberAsync(today);

                int sequence = 1;
                if (!string.IsNullOrEmpty(lastApp))
                {
                    var parts = lastApp.Split('-');
                    if (parts.Length == 2 && parts[0] == today)
                    {
                        if (int.TryParse(parts[1], out int lastSeq))
                        {
                            sequence = lastSeq + 1;
                        }
                    }
                }

                if (sequence > 9999)
                {
                    _logger.LogWarning("Daily application limit reached for {date}", today);
                    throw new InvalidOperationException("Maximum daily application limit reached");
                }

                return $"{today}-{sequence.ToString("D4")}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating application number");
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<string?> GetLastApplicationNumberAsync(string today)
        {
            return await _unitOfWork.Applications
                .FindAsync(a => a.ApplicationId != null && a.ApplicationId.StartsWith(today))
                .ContinueWith(task => task.Result
                    .OrderByDescending(a => a.ApplicationId)
                    .FirstOrDefault()?
                    .ApplicationId);
        }
    }
}
