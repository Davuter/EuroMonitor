using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subsricption.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Storage.Configurations
{
    public class SubscriptionsConfiguration : IEntityTypeConfiguration<Subscriptions>
    {
        public void Configure(EntityTypeBuilder<Subscriptions> builder)
        {
            builder.ToTable("Subscriptions");
            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.Status).WithMany().HasForeignKey(r => r.StatusId);
        }
    }
}
