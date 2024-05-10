using GenTree.Domain;
using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.Relations.Repositories;

/// <summary>
/// Модель запроса родственников человека
/// </summary>
/// <param name="Id">Идентификатор искомого человека</param>
/// <param name="Level">Уровень поиска записей</param>
public record RelationRequest(Guid Id, int Level);
/// <summary>
/// Модель идентификации человека
/// </summary>
/// <param name="Id">Идентификатор человека в системе</param>
public record PersonId(Guid Id) : IPersonId;

public interface IRelationsRepository
{
    public Task<List<PersonId>> GetParentsByLevelAsync(RelationRequest request);
    public Task<List<PersonId>> GetChildrenByLevelAsync(RelationRequest request);
}
public class RelationsRepository(GenTreeContext context) : IRelationsRepository
{
    public Task<List<PersonId>> GetParentsByLevelAsync(RelationRequest request)
    {
        return context.Relationships
            .Where(x => x.DownPersonId == request.Id)
            .Where(x => x.RelationLevel == request.Level)
            .Select(x => new PersonId(x.TopPersonId))
            .ToListAsync();
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
