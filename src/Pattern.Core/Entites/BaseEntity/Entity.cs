namespace Pattern.Core.Entites.BaseEntity
{
    public class Entity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }

    public class Entity
    {
        public int Id { get; set; }
    }
}
