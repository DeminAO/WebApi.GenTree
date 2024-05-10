using Microsoft.AspNetCore.Mvc;
using WebApi.GenTree.Modules.People.Repositories;
using WebApi.GenTree.Validation;

namespace WebApi.GenTree.Modules.People;

/// <summary>
/// Запрос Данных о людях
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[ProducesResponseType<ErrorModel>(500)]
public class PeopleController : ControllerBase
{
    /// <summary>
    /// Запрос постраничной загрузки людей
    /// </summary>
    /// <param name="request">Модель запроса постраничной загрузки людей</param>
    /// <returns>Список людей</returns>
    [HttpPost]
    [ProducesResponseType<ResultModel<List<PersonModel>>>(200)]
    public async Task<ResultModel<List<PersonModel>>> GetPeopleAsync(
        [FromServices] IPeopleGetRepository repository,
        [FromBody] PeopleRequest request)
    {
        return new(await repository.GetPeopleAsync(request));
    }

    /// <summary>
    /// Запрос на добавление человека
    /// </summary>
    /// <param name="request">Информация о человеке</param>
    /// <returns>Идентификатор человека в системе</returns>
    [HttpPost]
    [ProducesResponseType<ResultModel<PersonInsertResult>>(200)]
    public async Task<ResultModel<PersonInsertResult>> InsertPeopleAsync(
        [FromServices] IPersonInsertRepository repository,
        [FromBody] PersonInsertRequest request)
    {
        return new(await repository.InsertAsync(request));
    }
}
