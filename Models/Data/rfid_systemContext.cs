using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID_SYSTEM
{
    public partial class rfid_systemContext : DbContext
    {
        public rfid_systemContext()
        {
        }

        public rfid_systemContext(DbContextOptions<rfid_systemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Entrances> Entrances { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<RfidNumbers> RfidNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["ConnectToRfid_system"].ConnectionString, x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Companies>(entity =>
            {
                entity.HasKey(e => e.IdComp)
                    .HasName("PRIMARY");

                entity.ToTable("companies");

                entity.Property(e => e.IdComp).HasColumnName("ID_comp");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.IdEmp)
                    .HasName("PRIMARY");

                entity.ToTable("employees");

                entity.HasIndex(e => e.Company)
                    .HasName("emp_comp_idx");

                entity.HasIndex(e => e.Post)
                    .HasName("emp_post_idx");

                entity.HasIndex(e => e.Rfid)
                    .HasName("emp_rfid_num_idx");

                entity.Property(e => e.IdEmp).HasColumnName("ID_emp");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Photo).HasColumnType("blob");

                entity.Property(e => e.Rfid).HasColumnName("RFID");

                entity.HasOne(d => d.CompanyNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Company)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("emp_comp");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Post)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("emp_post");

                entity.HasOne(d => d.Rf)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Rfid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("emp_rfid_num");
            });

            modelBuilder.Entity<Entrances>(entity =>
            {
                entity.HasKey(e => e.IdEntr)
                    .HasName("PRIMARY");

                entity.ToTable("entrances");

                entity.HasIndex(e => e.Employee)
                    .HasName("entr_emp_idx");

                entity.Property(e => e.IdEntr).HasColumnName("ID_entr");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Direction)
                    .IsRequired()
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Entrances)
                    .HasForeignKey(d => d.Employee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("entr_emp");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.HasKey(e => e.IdPost)
                    .HasName("PRIMARY");

                entity.ToTable("posts");

                entity.Property(e => e.IdPost).HasColumnName("ID_post");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RfidNumbers>(entity =>
            {
                entity.HasKey(e => e.IdRfid)
                    .HasName("PRIMARY");

                entity.ToTable("rfid_numbers");

                entity.Property(e => e.IdRfid).HasColumnName("ID_rfid");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ValidFrom)
                    .HasColumnName("Valid_from")
                    .HasColumnType("date");

                entity.Property(e => e.ValidTo)
                    .HasColumnName("Valid_to")
                    .HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
