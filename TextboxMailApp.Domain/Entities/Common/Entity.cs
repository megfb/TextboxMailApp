namespace TextboxMailApp.Domain.Entities.Common
{
    public abstract class Entity : IEntity
    {
        public string Id { get; set; }
        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
