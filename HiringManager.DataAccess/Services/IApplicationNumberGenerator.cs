using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiringManager.DataAccess.Servides
{
    public interface IApplicationNumberGenerator
    {
        Task<string> GenerateAsync();
    }
}
