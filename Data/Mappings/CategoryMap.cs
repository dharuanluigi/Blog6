using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
  public class CategoryMap : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      // primary key
      builder
      .HasKey(c => c.Id);

      // Identitiy seed
      builder
      .Property(c => c.Id)
      .ValueGeneratedOnAdd()
      .UseIdentityColumn();

      // others properties
      builder
      .Property(c => c.Name)
      .IsRequired()
      .HasColumnName("Name") // just e.g because it gets property name automatically
      .HasColumnType("NVARCHAR") // just e.g too, because it gets type implicit inside attrib
      .HasMaxLength(80);// just e.g too, optionally it can be add

      builder
      .Property(c => c.Slug)
      .IsRequired()
      .HasColumnName("Slug")
      .HasColumnType("VARCHAR")
      .HasMaxLength(80);

      // idexs
      builder
      .HasIndex(x => x.Slug, "IX_Category_Slug")
      .IsUnique();
    }
  }
}