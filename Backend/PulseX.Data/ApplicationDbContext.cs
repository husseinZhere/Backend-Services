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
        public DbSet<PatientHealthInfo> PatientHealthInfos { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<DoctorRating> DoctorRatings { get; set; }
        public DbSet<RiskAssessment> RiskAssessments { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<DoctorNotification> DoctorNotifications { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

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
                entity.Property(e => e.PatientId).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.PatientId).IsUnique();
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
                entity.Property(e => e.AverageRating).HasColumnType("decimal(3,2)");
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
                entity.Property(e => e.RecordType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FileType).HasMaxLength(100);
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.PatientId, e.RecordType });
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

            // PatientHealthInfo configuration
            modelBuilder.Entity<PatientHealthInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Height).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Weight).HasColumnType("decimal(5,2)");
                entity.Property(e => e.BloodSugar).HasColumnType("decimal(6,2)");
                entity.HasOne(e => e.Patient)
                      .WithMany()
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.PatientId, e.IsActive });
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

            // DoctorRating configuration
            modelBuilder.Entity<DoctorRating>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Doctor)
                      .WithMany(d => d.Ratings)
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.DoctorRatings)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Appointment)
                      .WithOne(a => a.Rating)
                      .HasForeignKey<DoctorRating>(e => e.AppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasIndex(e => e.AppointmentId).IsUnique();
                entity.Property(e => e.Rating).IsRequired();
            });

            // RiskAssessment configuration
            modelBuilder.Entity<RiskAssessment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RiskScore).HasColumnType("decimal(5,2)");
                entity.HasOne(e => e.Patient)
                      .WithMany()
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PasswordResetToken configuration
            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(10);
                entity.Property(e => e.ResetToken).HasMaxLength(50);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.Email, e.Token });
                entity.HasIndex(e => e.ResetToken).IsUnique(false);
            });

            // DoctorNotification configuration
            modelBuilder.Entity<DoctorNotification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Priority).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.HasOne(e => e.Doctor)
                      .WithMany()
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.RelatedPatient)
                      .WithMany()
                      .HasForeignKey(e => e.RelatedPatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.RelatedAppointment)
                      .WithMany()
                      .HasForeignKey(e => e.RelatedAppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.DoctorId, e.IsRead });
            });

            // Prescription configuration
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MedicationsJson).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(e => e.LabRequestsJson).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(e => e.ClinicalNotes).HasColumnType("nvarchar(max)");
                entity.HasOne(e => e.Doctor)
                      .WithMany()
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Patient)
                      .WithMany()
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Appointment)
                      .WithMany()
                      .HasForeignKey(e => e.AppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.PatientId, e.CreatedAt });
                entity.HasIndex(e => new { e.DoctorId, e.CreatedAt });
            });
        }
    }
}
