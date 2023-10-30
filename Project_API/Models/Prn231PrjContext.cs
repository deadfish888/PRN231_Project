using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_API.Models;

public partial class Prn231PrjContext : DbContext
{
    public Prn231PrjContext()
    {
    }

    public Prn231PrjContext(DbContextOptions<Prn231PrjContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseStudent> CourseStudents { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentSubmission> StudentSubmissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=Appuru;database= PRN231_PRJ;uid=sa;pwd=123;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Assignme__52C2182089EBB180");

            entity.ToTable("Assignment");

            entity.Property(e => e.AssignmentId).HasColumnName("assignmentId");
            entity.Property(e => e.DueDate)
                .HasDefaultValueSql("(dateadd(day,(7),getdate()))")
                .HasColumnType("datetime")
                .HasColumnName("dueDate");
            entity.Property(e => e.ItemId).HasColumnName("itemId");

            entity.HasOne(d => d.Item).WithOne(p => p.Assignment)
                .HasForeignKey<Assignment>(p => p.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Assignmen__itemI__3E52440B");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D81B0B9494");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__2AA84FD134052244");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("courseId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CourseName)
                .HasMaxLength(250)
                .HasColumnName("courseName");
            entity.Property(e => e.Creator)
                .HasMaxLength(255)
                .HasColumnName("creator");
            entity.Property(e => e.Teacher)
                .HasMaxLength(255)
                .HasColumnName("teacher");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Course__category__3A81B327");
        });

        modelBuilder.Entity<CourseStudent>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CourseId }).HasName("PK__CourseSt__C93098021C315E1C");

            entity.ToTable("CourseStudent");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.CourseId).HasColumnName("courseId");
            entity.Property(e => e.LastAccess)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("lastAccess");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseStudents)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CourseStu__cours__398D8EEE");

            entity.HasOne(d => d.User).WithMany(p => p.CourseStudents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseStu__userI__38996AB5");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__56A128AA53E95E2A");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("itemId");
            entity.Property(e => e.ItemName)
                .HasMaxLength(500)
                .HasColumnName("itemName");
            entity.Property(e => e.SectionId).HasColumnName("sectionId");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");

            entity.HasOne(d => d.Section).WithMany(p => p.Items)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Item__sectionId__3C69FB99");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__Resource__D8867D32650E923F");

            entity.ToTable("Resource");

            entity.Property(e => e.ResourceId).HasColumnName("resourceId");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.ItemId).HasColumnName("itemId");
            entity.Property(e => e.UploadedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("uploadedDate");

            entity.HasOne(d => d.Item).WithOne(p => p.Resource)
                .HasForeignKey<Resource>(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Resource__itemId__3D5E1FD2");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__Section__3F58FD52E4879E2C");

            entity.ToTable("Section");

            entity.Property(e => e.SectionId).HasColumnName("sectionId");
            entity.Property(e => e.CourseId).HasColumnName("courseId");
            entity.Property(e => e.SectionName)
                .HasMaxLength(255)
                .HasColumnName("sectionName");

            entity.HasOne(d => d.Course).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Section__courseI__3B75D760");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Student__CB9A1CFF6CD947EB");

            entity.ToTable("Student");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .HasColumnName("emailAddress");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName");
            entity.Property(e => e.Img)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<StudentSubmission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.AssignmentId }).HasName("PK__StudentS__DEB63D7DB6909C8D");

            entity.ToTable("StudentSubmission");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.AssignmentId).HasColumnName("assignmentId");
            entity.Property(e => e.FileData).HasColumnName("fileData");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .HasColumnName("fileName");
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("lastModified");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Assignment).WithMany(p => p.StudentSubmissions)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("FK__StudentSu__assig__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.StudentSubmissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentSu__userI__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
