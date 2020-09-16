using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Subsricption.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Application.Interfaces
{
    public interface ISubscriptionDbContext
    {
        DbSet<Subsricption.Domain.Entities.Subscriptions> Subscriptions { get; set; }
        DbSet<Status> Statuses { get; set; }
        int SaveChanges();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
