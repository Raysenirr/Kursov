using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Education.Domain.Entities;
using Education.Domain.Entities.Base;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name)
                .HasConversion(name => name.Value, name => new GroupName(name))
                .IsRequired()
                .HasMaxLength(10);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasMany(x => x.Students).WithOne(x => x.Group);
    }
}
