using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CarSwipe.API.Models;

public partial class CarSwipeContext : DbContext
{
    public CarSwipeContext(DbContextOptions<CarSwipeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarImage> CarImages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Swipe> Swipes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PRIMARY");

            entity
                .ToTable("cars")
                .UseCollation("utf8mb4_hungarian_ci");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("car_id");
            entity.Property(e => e.Ar)
                .HasColumnType("int(11)")
                .HasColumnName("ar");
            entity.Property(e => e.Evjarat)
                .HasColumnType("int(11)")
                .HasColumnName("evjarat");
            entity.Property(e => e.Futasteljesitmeny)
                .HasColumnType("int(11)")
                .HasColumnName("futasteljesitmeny");
            entity.Property(e => e.Karosszeria)
                .HasMaxLength(30)
                .HasColumnName("karosszeria");
            entity.Property(e => e.Leiras)
                .HasColumnType("text")
                .HasColumnName("leiras");
            entity.Property(e => e.Letrehozva)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("letrehozva");
            entity.Property(e => e.Marka)
                .HasMaxLength(50)
                .HasColumnName("marka");
            entity.Property(e => e.Model)
                .HasMaxLength(80)
                .HasColumnName("model");
            entity.Property(e => e.Teljesitmeny)
                .HasColumnType("int(11)")
                .HasColumnName("teljesitmeny");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Uzemanyag)
                .HasMaxLength(20)
                .HasColumnName("uzemanyag");
            entity.Property(e => e.Valto)
                .HasMaxLength(20)
                .HasColumnName("valto");

            entity.HasOne(d => d.User).WithMany(p => p.Cars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("cars_ibfk_1");
        });

        modelBuilder.Entity<CarImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity
                .ToTable("car_images")
                .UseCollation("utf8mb4_hungarian_ci");

            entity.HasIndex(e => e.CarId, "car_id");

            entity.Property(e => e.ImageId)
                .HasColumnType("int(11)")
                .HasColumnName("image_id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("car_id");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .HasColumnName("file_path");
            entity.Property(e => e.SortOrder)
                .HasColumnType("int(11)")
                .HasColumnName("sort_order");

            entity.HasOne(d => d.Car).WithMany(p => p.CarImages)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("car_images_ibfk_1");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity
                .ToTable("messages")
                .UseCollation("utf8mb4_hungarian_ci");

            entity.HasIndex(e => e.BuyerId, "buyer_id");

            entity.HasIndex(e => e.CarId, "car_id");

            entity.Property(e => e.MessageId)
                .HasColumnType("int(11)")
                .HasColumnName("message_id");
            entity.Property(e => e.Body)
                .HasColumnType("text")
                .HasColumnName("body");
            entity.Property(e => e.BuyerId)
                .HasColumnType("int(11)")
                .HasColumnName("buyer_id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("car_id");
            entity.Property(e => e.FromBuyer).HasColumnName("from_buyer");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.Letrehozva)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("letrehozva");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Messages)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("messages_ibfk_2");

            entity.HasOne(d => d.Car).WithMany(p => p.Messages)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("messages_ibfk_1");
        });

        modelBuilder.Entity<Swipe>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CarId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("swipes")
                .UseCollation("utf8mb4_hungarian_ci");

            entity.HasIndex(e => e.CarId, "car_id");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("car_id");
            entity.Property(e => e.Letrehozva)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("letrehozva");
            entity.Property(e => e.Liked).HasColumnName("liked");

            entity.HasOne(d => d.Car).WithMany(p => p.Swipes)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("swipes_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Swipes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("swipes_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("users")
                .UseCollation("utf8mb4_hungarian_ci");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Letrehozva)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("letrehozva");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
