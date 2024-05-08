namespace GenTree.Domain.Entities;

public interface IPersonId
{
    public Guid Id { get; }
}

/// <summary>
/// Человек
/// </summary>
public class Person : IPersonId
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Имя
    /// </summary>
    public string Given { get; set; }
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Family { get; set; }

    /// <summary>
    /// Родственные связи человека, где он выступает со стороны предков
    /// </summary>
    public virtual ICollection<PersonRelation> TopRelations { get; set; }

    /// <summary>
    /// Родственные связи человека, где он выступает со стороны потомков
    /// </summary>
    public virtual ICollection<PersonRelation> DownRelations { get; set; }
}
