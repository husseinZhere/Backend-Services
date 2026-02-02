using Microsoft.EntityFrameworkCore;
using PulseX.Core.Models;

namespace PulseX.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<HealthData> HealthData { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithOne(u => u.Patient)
                      .HasForeignKey<Patient>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Doctor configuration
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ConsultationPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.User)
                      .WithOne(u => u.Doctor)
                      .HasForeignKey<Doctor>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Appointment configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Message configuration
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Appointment)
                      .WithMany(a => a.Messages)
                      .HasForeignKey(e => e.AppointmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // MedicalRecord configuration
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // HealthData configuration
            modelBuilder.Entity<HealthData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.HealthData)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Story configuration
            modelBuilder.Entity<Story>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Stories)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ActivityLog configuration
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
