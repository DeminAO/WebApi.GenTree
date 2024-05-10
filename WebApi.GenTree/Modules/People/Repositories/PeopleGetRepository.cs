using GenTree.Domain;
using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.People.Repositories;

/// <summary>
/// Модель запроса человека
/// </summary>
/// <param name="Id">Идентификатор человека</param>
public record PersonRequest(Guid Id);
/// <summary>
/// Модель запроса постраничной загрузки людей
/// </summary>
/// <param name="Page">Номер страницы</param>
/// <param name="Count">Количество записей на странице</param>
public record PeopleRequest(int Page, int Count);
/// <summary>
/// Информация о человеке
/// </summary>
/// <param name="Id">Идентификатор в системе</param>
/// <param name="Given">Имя</param>
/// <param name="Family">Фамилия</param>
public record PersonModel(Guid Id, string Given, string Family);

public interface IPeopleGetRepository
{
    public Task<List<PersonModel>> GetPeopleAsync(PeopleRequest request);
    public ValueTask<List<PersonModel>> GetPeopleAsync(IEnumerable<IPersonId> request);
}
public class PeopleGetRepository(GenTreeContext context) : IPeopleGetRepository
{
    public Task<List<PersonModel>> GetPeopleAsync(PeopleRequest request)
    {
        var query = context.People
            .AsQueryable()
            .OrderBy(x => x.Id)
            .Skip(request.Count * request.Page)
            .Take(request.Count);
        return Map(query).ToListAsync();
    }

    public ValueTask<List<PersonModel>> GetPeopleAsync(IEnumerable<IPersonId> request)
    {
        if (!request.Any())
        {
            return new([]);
        }
        var ids = request.Select(x => x.Id).ToList();
        var query = context.People
            .AsQueryable()
            .OrderBy(x => x.Id)
            .Where(x => ids.Contains(x.Id));

        return new(Map(query).ToListAsync());
    }

    static IQueryable<PersonModel> Map(IQueryable<Person> people) => people.Select(x => new PersonModel(x.Id, x.Given, x.Family));
}
