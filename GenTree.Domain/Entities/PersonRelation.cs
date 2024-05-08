namespace GenTree.Domain.Entities;

/// <summary>
/// Родственная связь двух человек
/// </summary>
public class PersonRelation
{
    public Guid TopPersonId { get; set; }
    public virtual Person TopPerson { get; set; }
    public Guid DownPersonId { get; set; }
    public virtual Person DownPerson { get; set; }
    public int RelationLevel { get; set; }
}
