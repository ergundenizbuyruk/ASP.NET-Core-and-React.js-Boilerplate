namespace Pattern.Application.Services.Base.Dtos
{
	public interface IEntityDto : IEntityDto<int>
	{
	}

	public interface IEntityDto<TPrimaryKey>
	{
		TPrimaryKey Id { get; set; }
	}
}
