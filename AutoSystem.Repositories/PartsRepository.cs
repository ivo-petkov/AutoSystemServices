using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSystem.Repositories
{
    public class PartsRepository : EfRepository<Parts>
    {
        private AutoSystemContext dbContext;

        public PartsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }
    }
}
