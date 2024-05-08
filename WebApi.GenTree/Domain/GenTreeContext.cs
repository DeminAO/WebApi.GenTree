using GenTree.Domain.Configurations;
using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenTree.Domain;

public class GenTreeContext(DbContextOptions<GenTreeContext> options) : DbContext(options)
{
    /// <summary>
    /// Люди
    /// </summary>
    public DbSet<Person> People { get; set; }
    /// <summary>
    /// Родственные связи людей
    /// </summary>
    public DbSet<PersonRelation> Relationships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new PersonRelationConfiguration());


        base.OnModelCreating(modelBuilder);
    }
}
