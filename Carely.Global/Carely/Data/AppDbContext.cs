using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace Carely.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Mother> Mothers => Set<Mother>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Medication> Medications => Set<Medication>();
        public DbSet<Clinic> Clinics => Set<Clinic>();
        public DbSet<ClinicWorkTime> ClinicWorkTimes => Set<ClinicWorkTime>();
        public DbSet<Meeting> Meetings => Set<Meeting>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<Lullaby> Lullabies => Set<Lullaby>();

        public DbSet<MotherLullabyUsage> MotherLullabyUsages => Set<MotherLullabyUsage>();

        public DbSet<Baby> Babies => Set<Baby>();

        public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
        public DbSet<BabyVaccination> BabyVaccinations => Set<BabyVaccination>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }
    }
}
