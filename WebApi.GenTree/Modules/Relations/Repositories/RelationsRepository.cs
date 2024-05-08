using GenTree.Domain;
using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.Relations.Repositories;

public record RelationRequest(Guid Id, int Level);

public record PersonId(Guid Id) : IPersonId;

public interface IRelationsRepository
{
    public Task<PersonId> GetParentByLevelAsync(RelationRequest request);
    public Task<List<PersonId>> GetChildrenByLevelAsync(RelationRequest request);
}
public class RelationsRepository(GenTreeContext context) : IRelationsRepository
{
    public Task<PersonId> GetParentByLevelAsync(RelationRequest request)
    {
        return context.Relationships
            .Where(x => x.DownPersonId == request.Id)
            .Where(x => x.RelationLevel == request.Level)
            .Select(x => new PersonId(x.TopPersonId))
            .FirstOrDefaultAsync();
    }

    public Task<List<PersonId>> GetChildrenByLevelAsync(RelationRequest request)
    {
        return context.Relationships
            .Where(x => x.TopPersonId == request.Id)
            .Where(x => x.RelationLevel == request.Level)
            .Select(x => new PersonId(x.DownPersonId))
            .ToListAsync();
    }
}
