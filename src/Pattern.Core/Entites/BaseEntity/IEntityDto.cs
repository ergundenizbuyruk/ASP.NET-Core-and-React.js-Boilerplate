namespace Pattern.Core.Entites.BaseEntity
{
	public interface IEntityDto : IEntityDto<int>
	{
	}

	public interface IEntityDto<TPrimaryKey>
	{
		TPrimaryKey Id { get; set; }
	}
}
