using ElectronicsShop_service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ElectronicsShop_service;
public class ApplicationDbContext : DbContext
{


    public DbSet<Cloth>? Clothes { get; set; }
    public DbSet<Bill>? Bills { get; set; }
    public DbSet<Money>? Moneys{ get; set; }

    public DbSet<Worker>? ShopWorkers { get; set; }
    public DbSet<Shop>? Shops { get; set; }





    private readonly IConfiguration configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }

    public ApplicationDbContext()
    {
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
        var shop = new Shop
        {
            Id = Guid.NewGuid(),
            UserName = "reda",
            Password = "Reda12@",  // Plain password
            Role = "Admin",
            WhatToSee = "shop1",
            CreationDate = DateTime.Now
        };

        builder.Entity<Shop>().HasData(shop);
        // Seed roles

        // Seed users
        



      
    }
}


