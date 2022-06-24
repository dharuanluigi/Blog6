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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
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