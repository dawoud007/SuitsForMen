using ElectronicsShop_service.IdentityHandler;
using ElectronicsShop_service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ElectronicsShop_service;
public class ApplicationDbContext : DbContext
{


    public DbSet<Category>? Categories { get; set; }
    public DbSet<Cloth>? Clothes { get; set; }
    public DbSet<Bill>? Bills { get; set; }
    public DbSet<Store>? Stors { get; set; }
    public DbSet<User>? Users { get; set; }






    private readonly IConfiguration configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /*   if (!optionsBuilder.IsConfigured)
           {
               string connectionString = configuration.GetConnectionString("Default");
               optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
           }*/
        /* "DefaultConnection": "Server=DESKTOP-06NBEEH\\SQLEXPRESS;Database=pushing;Trusted_Connection=True;TrustServerCertificate=True;",*/
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed roles

        builder.Entity<ApplicationRole>().HasData(
          new ApplicationRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", userId = 1 },
          new ApplicationRole { Id = "2", Name = "User", NormalizedName = "USER", userId = 2 }
      );
        // Seed users
        var user1 = new User
        {
            Id = 50,
            UserName = "reda",
            NormalizedUserName = "REDA",
            Password = "Reda12@",
            Role = "Admin",
            WhatToSee = "shop1",
            CreationDate = DateTime.Now
        };

        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        user1.PasswordHash = passwordHasher.HashPassword(user1, user1.Password);

        builder.Entity<User>().HasData(user1);

          /* builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = $"{user1.Id}", RoleId = "1" } // Assigning "Admin" role to user1
            );*/


        // Seed user roles


        // Ignore unnecessary identity related tables
        /*   builder.Ignore<IdentityUserLogin<string>>();
           builder.Ignore<IdentityUserToken<string>>();
           builder.Ignore<IdentityUserClaim<string>>();
           builder.Ignore<IdentityRoleClaim<string>>();*/
    }
}


