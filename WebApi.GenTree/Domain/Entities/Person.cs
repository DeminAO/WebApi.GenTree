namespace GenTree.Domain.Entities;

/// <summary>
/// Человек
/// </summary>
public class Person
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
