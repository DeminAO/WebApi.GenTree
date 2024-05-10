using Microsoft.AspNetCore.Mvc;
using WebApi.GenTree.Modules.People.Repositories;
using WebApi.GenTree.Modules.Relations.Repositories;
using WebApi.GenTree.Validation;

namespace WebApi.GenTree.Modules.Relations;

/// <summary>
/// Модель запроса данных по идентификатору
/// </summary>
/// <param name="Id"></param>
public record PeopleByLevelRequest(Guid Id);

/// <summary>
/// Запрос родственных связей
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[ProducesResponseType<ErrorModel>(500)]
public class RelationsController : ControllerBase
{
    /// <summary>
    /// Запрос правнуков
    /// </summary>
    /// <param name="request">Идентификатор прадеда</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResultModel<List<PersonModel>>>(200)]
    public async Task<ResultModel<IEnumerable<PersonModel>>> GetPeopleByThirdLevelAsync(
        [FromServices] IRelationsRepository repository,
        [FromServices] IPeopleGetRepository peopleGetRepository,
        [FromBody] PeopleByLevelRequest request)
    {
        var personIds = await repository.GetChildrenByLevelAsync(new(request.Id, 3));
        return new(await peopleGetRepository.GetPeopleAsync(personIds));
    }
    
    /// <summary>
    /// Запрос прадеда
    /// </summary>
    /// <param name="request">Идентификатор правнука</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResultModel<PersonModel>>(200)]
    public async Task<ResultModel<PersonModel>> GetPersonByThirdLevelAsync(
        [FromServices] IRelationsRepository relationsRepository,
        [FromServices] IPeopleGetRepository peopleGetRepository,
        [FromBody] PeopleByLevelRequest request)
    {
        var personIds = await relationsRepository.GetParentsByLevelAsync(new(request.Id, Level: 3));
        var people = await peopleGetRepository.GetPeopleAsync(personIds.Take(1));
        return new(people.FirstOrDefault());
    }
    
    /// <summary>
    /// Запрос деда
    /// </summary>
    /// <param name="request">Идентификатор внука</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResultModel<PersonModel>>(200)]
    public async Task<ResultModel<PersonModel>> GetPersonBySecondLevelAsync(
        [FromServices] IRelationsRepository relationsRepository,
        [FromServices] IPeopleGetRepository peopleGetRepository,
        [FromBody] PeopleByLevelRequest request)
    {
        var personIds = await relationsRepository.GetParentsByLevelAsync(new(request.Id, Level: 2));
        var people = await peopleGetRepository.GetPeopleAsync(personIds.Take(1));
        return new(people.FirstOrDefault());
    }

}