using ReadyHire.Models.Authentication;
using ReadyHire.Models.CompanyProfile;
using ReadyHire.Models.UserProfile;
using ReadyHire.Models.UserProfilePic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ReadyHire.Models.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {



        }


        public DbSet<UserProfilePic.UserProfilePic> userProfilePictures { get; set; }
        public DbSet<UserOverView> UserOverViews { get; set; }
        public DbSet<UserProfiles> UserProfiles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<UserLanguage> UserLanguages { get; set; }
        public DbSet<Cv> Cvs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobExamSubmission> JobExamSubmissions { get; set; }
        public DbSet<JobExamAnswer> JobExamAnswers { get; set; }
        public DbSet<CompanyProfiles> CompanyProfiles { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobExam> JobExams { get; set; }
        public DbSet<JobQuestion> JobQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobApplication>()
    .HasOne(j => j.UserProfile)
    .WithMany()
    .HasForeignKey(j => j.UserProfileId)
    .OnDelete(DeleteBehavior.Restrict); // 👈 هنا الحل


            modelBuilder.Entity<JobExamSubmission>()
    .HasOne(j => j.UserProfile)
    .WithMany() // أو .WithMany(u => u.JobExamSubmissions) لو عندك navigation
    .HasForeignKey(j => j.UserProfileId)
    .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<JobExamAnswer>()
    .HasOne(e => e.JobQuestion)
    .WithMany()
    .HasForeignKey(e => e.QuestionId)
    .OnDelete(DeleteBehavior.Restrict); // أو DeleteBehavior.NoAction

            modelBuilder.Entity<JobExamAnswer>()
    .HasOne(a => a.JobQuestion)
    .WithMany() // أو WithOne حسب تصميمك
    .HasForeignKey(a => a.QuestionId)
    .OnDelete(DeleteBehavior.Restrict); // أو .NoAction


            modelBuilder.Entity<ApplicationUser>()
             .HasOne(u => u.CompanyProfiles)
             .WithOne(c => c.ApplicationUser)
             .HasForeignKey<CompanyProfiles>(c => c.ApplicationUserId);


            modelBuilder.Entity<ApplicationUser>()
               .HasOne(u => u.UserProfiles)
               .WithOne(p => p.ApplicationUser)
               .HasForeignKey<UserProfiles>(p => p.ApplicationUserId);

            modelBuilder.Entity<Cv>()
                .HasOne(cv => cv.UserProfile)
                .WithOne(up => up.cv)
                .HasForeignKey<Cv>(cv => cv.UserProfileId);


            modelBuilder.Entity<UserLanguage>()
    .HasOne<UserProfiles>(ul => ul.UserProfile)
    .WithMany(up => up.Languages)
    .HasForeignKey(ul => ul.UserProfileId)
    .OnDelete(DeleteBehavior.Cascade);


        }
    }
}


