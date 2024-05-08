using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenTree.Domain.Configurations;

class PersonRelationConfiguration : IEntityTypeConfiguration<PersonRelation>
{
    public void Configure(EntityTypeBuilder<PersonRelation> builder)
    {
        builder.HasKey(x => new { x.TopPersonId, x.DownPersonId });
        builder
            .HasOne(x => x.TopPerson)
            .WithMany(x => x.TopRelations)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.TopPersonId)
            .IsRequired();
        builder
            .HasOne(x => x.DownPerson)
            .WithMany(x => x.DownRelations)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.DownPersonId)
            .IsRequired();
    }
}
