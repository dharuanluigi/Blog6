using System;
using Blog.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
  public class BlogDataContext : DbContext
  {
    public DbSet<Category> Categories { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<User> Users { get; set; }

    // instance configuration
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseSqlServer("Server=localhost,1433;Database=Blog;User ID=sa;Password=Brasil@2022");
      options.LogTo(Console.WriteLine);
    }

    // model configuration when creating using maps do that.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new CategoryMap());
      modelBuilder.ApplyConfiguration(new PostMap());
      modelBuilder.ApplyConfiguration(new UserMap());
    }
  }
}