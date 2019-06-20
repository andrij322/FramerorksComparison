using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace frameworksComparison
{
    class WorkerContext: DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=newDB.db");
        }
    }
}
