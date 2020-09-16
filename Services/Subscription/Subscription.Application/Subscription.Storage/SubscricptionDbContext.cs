using Microsoft.EntityFrameworkCore;
using Subscription.Application.Interfaces;
using Subsricption.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Storage
{
    public class SubscriptionDbContext : DbContext, ISubscriptionDbContext
    {

        public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> dbContextOptions ): base(dbContextOptions)
        {
            
        }

        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriptionDbContext).Assembly);
        }

    }
}
