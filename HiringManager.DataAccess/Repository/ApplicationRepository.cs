﻿using HiringManager.DataAccess.Data;
using HiringManager.DataAccess.Repository.IRepository;
using HiringManager.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiringManager.DataAccess.Repository
{
    public class ApplicationRepository : Repository<ApplicationDetail>, IApplicationRepository
    {
        private ApplicationDbContext _db;

        public ApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(ApplicationDetail entity)
        {
            _db.ApplicationSet.Update(entity);
        }
    }
}
