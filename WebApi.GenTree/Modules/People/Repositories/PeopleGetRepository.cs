using GenTree.Domain;
using GenTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.People.Repositories;

public record PersonRequest(Guid Id);
public record PeopleRequest(int Page, int Count);
public record PersonModel(Guid Id, string Given, string Family);

public interface IPeopleGetRepository
{
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(PeopleRequest request);
    public Task<PersonModel> GetPersonAsync(PersonRequest request);
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(IQueryable<Person> request);
}
public class PeopleGetRepository(GenTreeContext context) : IPeopleGetRepository
{
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(PeopleRequest request)
    {
        var query = context.People
            .AsQueryable()
            .OrderBy(x => x.Id)
            .Skip(request.Count * request.Page)
            .Take(request.Count);
        return Map(query).AsAsyncEnumerable();
    }
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(IQueryable<Person> request)
    {
        return Map(request.OrderBy(x => x.Id)).AsAsyncEnumerable();
    }

    public async Task<PersonModel> GetPersonAsync(PersonRequest request)
    {
        return await Map(context.People.Where(x => x.Id == request.Id)).FirstOrDefaultAsync();
    }

    static IQueryable<PersonModel> Map(IQueryable<Person> people) => people.Select(x => new PersonModel(x.Id, x.Given, x.Family));
}
