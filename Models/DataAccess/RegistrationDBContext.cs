using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseReg.Models.DataAccess
{
    public partial class RegistrationDBContext : DbContext
    {
        public RegistrationDBContext()
        {
        }

        public RegistrationDBContext(DbContextOptions<RegistrationDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\CST8256FinalDB\\RegistrationDB.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("CourseID");

                entity.Property(e => e.CourseTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FeeBase).HasColumnType("decimal(6, 2)");

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "Registration",
                        l => l.HasOne<Student>().WithMany().HasForeignKey("Student").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Registration_ToStudent"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("Course").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Registration_ToCourse"),
                        j =>
                        {
                            j.HasKey("Course", "Student").HasName("PK__Registra__92ECCCE92AE4E752");

                            j.ToTable("Registration");

                            j.IndexerProperty<string>("Course").HasMaxLength(16).IsUnicode(false);

                            j.IndexerProperty<string>("Student").HasMaxLength(16).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentNum)
                    .HasName("PK__Student__B98EFCB567F3C9C7");

                entity.ToTable("Student");

                entity.Property(e => e.StudentNum)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
