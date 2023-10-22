namespace Pattern.Core.Entites.BaseEntity
{
    public class EntityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }

    public class EntityDto
    {
        public int Id { get; set; }
    }
}
