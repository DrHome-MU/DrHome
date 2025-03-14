﻿using Dr_Home.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dr_Home.Data.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Doctor_Schedule>
    {
        public void Configure(EntityTypeBuilder<Doctor_Schedule> builder)
        {
            builder.ToTable("Schedules");

            //Schedules With Doctor 
           

            //Scheules With Clinic 
            builder.HasOne(s => s.clinic)
              .WithMany(c => c._schedules)
              .HasForeignKey(s => s.ClinicId)
              .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
