/*UserManagement1Context.cs*/

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Petrosia.Models;  // Ensure this matches the actual namespace

namespace Petrosia.Models;

public partial class UserManagement1Context : DbContext
{
    public UserManagement1Context()
    {
    }

    public UserManagement1Context(DbContextOptions<UserManagement1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<Guest> Guests { get; set; }
    public virtual DbSet<HouseKeeping> HouseKeepings { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; } // Added Bookings DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PRIMARY");

            entity.ToTable("admin");

            entity.Property(e => e.AdminId)
                .HasColumnType("int(11)")
                .HasColumnName("Admin_ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PRIMARY");

            entity.ToTable("guest");

            entity.Property(e => e.GuestId)
                .HasColumnType("int(11)")
                .HasColumnName("Guest_ID");
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("First_Name");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("Password");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<HouseKeeping>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("house_keeping");

            entity.HasIndex(e => e.RoomId, "assigned_room");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("employee_id");
            entity.Property(e => e.Firstname)
                .HasColumnType("text")
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasColumnType("text")
                .HasColumnName("lastname");
            entity.Property(e => e.RoomId)
                .HasColumnType("int(11)")
                .HasColumnName("room_id");

            entity.HasOne(d => d.Room).WithMany(p => p.HouseKeepings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assigned_room");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PRIMARY");

            entity.ToTable("room");

            entity.HasIndex(e => e.GuestId, "guest_room");

            entity.Property(e => e.RoomId)
                .HasColumnType("int(11)")
                .HasColumnName("Room_ID");
            entity.Property(e => e.Capacity).HasColumnType("int(11)");
            entity.Property(e => e.GuestId)
                .HasColumnType("int(11)")
                .HasColumnName("guest_id");
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(10)
                .HasColumnName("Room_Number");
            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .HasColumnName("Room_Type");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Guest).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guest_room");
        });

        // Configure the new Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings");
            entity.HasKey(e => e.BookingId);

            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Status)
                .HasDefaultValue("Confirmed");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
