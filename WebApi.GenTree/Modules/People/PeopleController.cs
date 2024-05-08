using Microsoft.AspNetCore.Mvc;

namespace WebApi.GenTree.Modules.People;

[ApiController]
[Route("[controller]/[action]")]
public class PeopleController
{
    [HttpPost]
    public IAsyncEnumerable<PersonModel> GetPeopleAsync(
        [FromServices] IPeopleGetRepository repository,
        [FromBody] PeopleRequest request)
    {
        return repository.GetPeopleAsync(request);
    }
    [HttpPost]
    public Task<PersonInsertResult> InsertPeopleAsync(
        [FromServices] IPersonInsertRepository repository,
        [FromBody] PersonInsertRequest request)
    {
        return repository.InsertAsync(request);
    }
}
