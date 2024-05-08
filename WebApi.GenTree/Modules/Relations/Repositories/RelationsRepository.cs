using GenTree.Domain;
using Microsoft.EntityFrameworkCore;
using WebApi.GenTree.Modules.People.Repositories;

namespace WebApi.GenTree.Modules.Relations.Repositories;

public record RelationRequest(Guid Id, int Level);
public record PersonId(Guid Id);
public interface IRelationsRepository
{
    public Task<PersonId> GetParentByLevelAsync(RelationRequest request);
    public IAsyncEnumerable<PersonModel> GetChildrenByLevelAsync(RelationRequest request);
}
public class RelationsRepository(GenTreeContext context, IPeopleGetRepository peopleGetRepository) : IRelationsRepository
{
    public Task<PersonId> GetParentByLevelAsync(RelationRequest request)
    {
        return context.Relationships
            .Where(x => x.DownPersonId == request.Id)
            .Where(x => x.RelationLevel == request.Level)
            .Select(x => new PersonId(x.TopPersonId))
            .FirstOrDefaultAsync();
    }

    public IAsyncEnumerable<PersonModel> GetChildrenByLevelAsync(RelationRequest request)
    {
        var ancestry = context.Relationships
            .Where(x => x.TopPersonId == request.Id)
            .Where(x => x.RelationLevel == request.Level)
            .Select(x => x.DownPerson);
        return peopleGetRepository.GetPeopleAsync(ancestry);
    }
}
