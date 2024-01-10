namespace Pattern.Core.Entites.BaseEntity
{
	public interface IEntity : IEntity<int>
	{
	}

	public interface IEntity<TPrimaryKey>
	{
		TPrimaryKey Id { get; set; }
	}
}
