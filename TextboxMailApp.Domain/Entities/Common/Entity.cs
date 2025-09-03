namespace TextboxMailApp.Domain.Entities.Common
{
    public abstract class Entity : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
