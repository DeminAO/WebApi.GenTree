using GenTree.Domain;
using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.Relations.Repositories;

/// <summary>
/// Модель запроса на добавление родственной связи двух людей
/// </summary>
/// <param name="ChildId">Идентификатор ребенка</param>
/// <param name="ParentId">Идентификатор родителя</param>
public record RelationInsertRequest(Guid ChildId, Guid ParentId);

public interface IRelationsInsertRepository
{
    public Task InsertChildAsync(RelationInsertRequest request);
}

public class RelationsInsertRepository(GenTreeContext context) : IRelationsInsertRepository
{
    public async Task InsertChildAsync(RelationInsertRequest request)
    {
        // выборка предков родителя
        var ancestry = await context.Relationships
            .Where(x => x.DownPersonId == request.ParentId)
            .Select(x => new { x.TopPersonId, x.RelationLevel })
            .ToListAsync();

        // создание связи с предками родителя
        var entries = ancestry
            .Select(x => new PersonRelation
            {
                DownPersonId = request.ChildId,
                TopPersonId = x.TopPersonId,
                RelationLevel = x.RelationLevel + 1
            });

        // создание связи с родителем
        entries = entries.Append(new PersonRelation
        {
            DownPersonId = request.ChildId,
            TopPersonId = request.ParentId,
            RelationLevel = 1
        });

        context.Relationships.AddRange(entries);
        await context.SaveChangesAsync();
    }
}
