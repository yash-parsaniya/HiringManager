using HiringManager.DataAccess.Data;
using HiringManager.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiringManager.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IApplicationRepository Application { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Application = new ApplicationRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
