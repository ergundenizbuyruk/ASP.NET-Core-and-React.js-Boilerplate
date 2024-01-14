namespace Pattern.Application.Services.Base.Dtos
{
	public abstract class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
	{
		public TPrimaryKey Id { get; set; }
	}

	public abstract class EntityDto : EntityDto<int>, IEntityDto
	{
	}
}
