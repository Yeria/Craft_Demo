using CraftBackEnd.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Services.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<User> User { get; }
        DbSet<Auth> Auth { get; }
        //DbSet<Assessment> Assessment { get; }
        //DbSet<AssessmentCategory> AssessmentCategory { get; }
        //DbSet<MultipleChoiceOption> MultipleChoiceOption { get; }
        //DbSet<Question> Question { get; }
        //DbSet<Answer> Answer { get; }
        int SaveChanges();
        DatabaseFacade Database { get; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
