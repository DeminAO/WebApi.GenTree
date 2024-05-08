using GenTree.Domain;
using GenTree.Domain.Entities;
using WebApi.GenTree.Modules.Relations.Repositories;

namespace WebApi.GenTree.Modules.People.Repositories;

/// <summary>
/// Модель запроса на добавление человека
/// </summary>
/// <param name="Given">Имя</param>
/// <param name="Family">Фамилия</param>
/// <param name="ParentId">Идентификатор родителя</param>
public record PersonInsertRequest(string Given, string Family, Guid? ParentId);
/// <summary>
/// Результат обработки запроса на добавление человека
/// </summary>
/// <param name="Id">Идентификатор человека в системе</param>
public record PersonInsertResult(Guid Id);

public interface IPersonInsertRepository
{
    public Task<PersonInsertResult> InsertAsync(PersonInsertRequest request);
}
public class PersonInsertRepository(GenTreeContext context, IRelationsInsertRepository relationsInsertRepository) : IPersonInsertRepository
{
    public async Task<PersonInsertResult> InsertAsync(PersonInsertRequest request)
    {
        var person = new Person
        {
            Given = request.Given,
            Family = request.Family
        };
        context.People.Add(person);
        await context.SaveChangesAsync();

        if (request.ParentId.HasValue)
        {
            await relationsInsertRepository.InsertChildAsync(new(person.Id, request.ParentId.Value));
        }

        return new(person.Id);
    }
}
