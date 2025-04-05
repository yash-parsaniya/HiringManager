using System.Collections.Generic;
using System.Threading.Tasks;
using HiringManager.Models;

namespace HiringManager.DataAccess.Services
{
    public interface IIdGeneratorService
    {
        Task<string> GenerateIdAsync();
    }
}
