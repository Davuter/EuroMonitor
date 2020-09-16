using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subsricption.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscription.Storage.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Status");
            builder.HasKey(r => r.Id);
        }
    }
}
