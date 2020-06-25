using CraftBackEnd.Common.Models;
using CraftBackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Services.Entity
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Auth> Auth { get; set; }
        //public DbSet<Auth> Auth { get; set; }
        //public DbSet<Assessment> Assessment { get; set; }
        //public DbSet<AssessmentCategory> AssessmentCategory { get; set; }
        //public DbSet<MultipleChoiceOption> MultipleChoiceOption { get; set; }
        //public DbSet<Question> Question { get; set; }
        //public DbSet<Answer> Answer { get; set; }
    }
}
