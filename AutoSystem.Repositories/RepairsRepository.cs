﻿using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;

namespace AutoSystem.Repositories
{
    public class RepairsRepository: EfRepository<Repair>
    {
        private AutoSystemContext dbContext;

        public RepairsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

    }
}
