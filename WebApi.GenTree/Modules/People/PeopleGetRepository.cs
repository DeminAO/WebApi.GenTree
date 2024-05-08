using GenTree.Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApi.GenTree.Modules.People;

public record PeopleRequest(int Page, int Count);
public record PersonModel(Guid Id, string Given, string Family);

public interface IPeopleGetRepository
{
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(PeopleRequest request);

}
public class PeopleGetRepository(GenTreeContext context) : IPeopleGetRepository
{
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(PeopleRequest request)
    {
        return context.People
            .AsQueryable()
            .OrderBy(x => x.Id)
            .Skip(request.Count * request.Page)
            .Take(request.Count)
            .Select(x => new PersonModel(x.Id, x.Given, x.Family))
            .AsAsyncEnumerable();
    }
}
