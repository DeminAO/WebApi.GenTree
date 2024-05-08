﻿using Microsoft.AspNetCore.Mvc;
using WebApi.GenTree.Modules.People.Repositories;
using WebApi.GenTree.Modules.Relations.Repositories;

namespace WebApi.GenTree.Modules.Relations;

/// <summary>
/// Модель запроса данных по идентификатору
/// </summary>
/// <param name="Id"></param>
public record PeopleByThirdLevelRequest(Guid Id);

[ApiController]
[Route("[controller]/[action]")]
public class RelationsController
{
    /// <summary>
    /// Запрос правнуков
    /// </summary>
    /// <param name="request">Идентификатор прадеда</param>
    /// <returns></returns>
    [HttpPost]
    public IAsyncEnumerable<PersonModel> GetPeopleByThirdLevelAsync(
        [FromServices] IRelationsRepository repository,
        [FromBody] PeopleByThirdLevelRequest request)
    {
        return repository.GetChildrenByLevelAsync(new(request.Id, 3));
    }
    
    /// <summary>
    /// Запрос прадеда
    /// </summary>
    /// <param name="request">Идентификатор правнука</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PersonModel> GetPersonByThirdLevelAsync(
        [FromServices] IRelationsRepository relationsRepository,
        [FromServices] IPeopleGetRepository peopleGetRepository,
        [FromBody] PeopleByThirdLevelRequest request)
    {
        var personId = await relationsRepository.GetParentByLevelAsync(new(request.Id, Level: 3));
        return await peopleGetRepository.GetPersonAsync(new(personId.Id));
    }
    
    /// <summary>
    /// Запрос деда
    /// </summary>
    /// <param name="request">Идентификатор внука</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PersonModel> GetPersonBySecondLevelAsync(
        [FromServices] IRelationsRepository relationsRepository,
        [FromServices] IPeopleGetRepository peopleGetRepository,
        [FromBody] PeopleByThirdLevelRequest request)
    {
        var personId = await relationsRepository.GetParentByLevelAsync(new(request.Id, Level: 2));
        return await peopleGetRepository.GetPersonAsync(new(personId.Id));
    }

}